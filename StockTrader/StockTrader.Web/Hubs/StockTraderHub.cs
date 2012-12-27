using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using StockTrader.Web.Configuration;
using StockTrader.Web.Services;

namespace StockTrader.Web.Hubs {
    [HubName("trader")]
    [RegisterInContainer(typeof(StockTraderHub))]
    public class StockTraderHub : Hub {
        private readonly IAccountLocator accountLocator;
        private readonly IStocksUpdater stocksUpdater;
        private readonly IStockBroker stockBroker;

        public StockTraderHub(IAccountLocator accountLocator, IStocksUpdater stocksUpdater, IStockBroker stockBroker) {
            this.accountLocator = accountLocator;
            this.stocksUpdater = stocksUpdater;
            this.stockBroker = stockBroker;
        }

        public Task RequestAccountBalance() {
            string accountID = this.Clients.Caller.AccountID;
            var account = this.accountLocator.GetAccount(accountID);

            return this.Clients.Caller.BalanceUpdated(accountID, account.Balance, account.Stocks);
        }

        public async Task RequestStockAction(Guid requestID, StockAction action, string symbol, int quantity) {
            await this.Clients.Caller.StockActionExecuted(requestID, StockActionStatus.Submitted, action, symbol, quantity, null, null);

            string accountID = this.Clients.Caller.AccountID;
            this.stockBroker.RequestStockAction(requestID, this.Context.ConnectionId, accountID, action, symbol, quantity);
        }

        public Task SubscribeToStock(string symbol) {
            return this.stocksUpdater.Subscribe(symbol, this.Context.ConnectionId);
        }

        public Task UnsubscribeFromStock(string symbol) {
            return this.stocksUpdater.Unsubsribe(symbol, this.Context.ConnectionId);
        }
    }
}