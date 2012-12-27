using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using StockTrader.Web.Hubs;

namespace StockTrader.Web.Services {
    public class PendingStockActions {
        private readonly IStockPriceProvider priceProvider;
        private readonly IAccountLocator accountLocator;
        private readonly IAccountPersister accountPersister;
        private readonly IHubContext hubContext;
        private readonly List<PendingAction> pendingActions;
        private readonly Timer pendingActionsTimer;

        public PendingStockActions(IStockPriceProvider priceProvider, IAccountLocator accountLocator, IAccountPersister accountPersister) {
            this.priceProvider = priceProvider;
            this.accountLocator = accountLocator;
            this.accountPersister = accountPersister;
            this.hubContext = GlobalHost.ConnectionManager.GetHubContext<StockTraderHub>();
            this.pendingActions = new List<PendingAction>();
            this.pendingActionsTimer = new Timer(this.PendingActionsCallback, null, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
        }

        public void RegisterPendingAction(string symbol, int quantity, Guid historyID, StockAction actionType, string accountID, string connectionID) {
            lock (this.pendingActions) {
                this.pendingActions.Add(new PendingAction(symbol, quantity, historyID, actionType, accountID, connectionID));
            }
        }

        public void ClearPendingActions(string accountID) {
            lock (this.pendingActions) {
                this.pendingActions.RemoveAll(p => p.AccountID == accountID);
            }
        }

        private void PendingActionsCallback(object ignore) {
            List<PendingAction> actionsToProcess;
            lock (this.pendingActions) {
                actionsToProcess = new List<PendingAction>(this.pendingActions.Count);

                for (int i = this.pendingActions.Count - 1; i >= 0; i--) {
                    var pendingPurchase = this.pendingActions[i];
                    if (0 >= --pendingPurchase.CyclesToWait) {
                        actionsToProcess.Add(pendingPurchase);
                        this.pendingActions.RemoveAt(i);
                    }
                }
            }

            if (0 == actionsToProcess.Count) {
                return;
            }

            var allSymbols = actionsToProcess.Select(p => p.Symbol);
            var prices = this.priceProvider.GetPricesFor(allSymbols);
            var pairs = actionsToProcess.Zip(prices, (pending, price) => new { pending, price });

            foreach (var pair in pairs.AsParallel()) {
                decimal amount = pair.price.Price * pair.pending.Quantity;

                var account = this.accountLocator.GetAccount(pair.pending.AccountID);

                decimal newBalance;
                bool success = (StockAction.Buy == pair.pending.ActionType)
                    ? account.TryPurchaseStock(pair.pending.Symbol, pair.pending.Quantity, pair.price.Price, out newBalance)
                    : account.TrySellStock(pair.pending.Symbol, pair.pending.Quantity, pair.price.Price, out newBalance);

                if (success) {
                    this.accountPersister.SaveAccount(account);
                }

                dynamic connection = this.hubContext.Clients.Client(pair.pending.ConnectionID);
                
                connection.BalanceUpdated(account.ID, account.Balance, account.Stocks);
                
                connection.StockActionExecuted(
                    pair.pending.HistoryID,
                    success ? StockActionStatus.Completed : StockActionStatus.Rejected,
                    pair.pending.ActionType,
                    pair.pending.Symbol,
                    pair.pending.Quantity,
                    pair.price.Price,
                    amount
                );
            }
        }
    }
}