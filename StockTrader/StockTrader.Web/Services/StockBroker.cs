using System;
using StockTrader.Web.Configuration;

namespace StockTrader.Web.Services {
    [RegisterInContainer(typeof(IStockBroker), RegistrationType.Singleton)]
    public class StockBroker : IStockBroker {
        private readonly PendingStockActions pendingStockActions;

        public StockBroker(PendingStockActions pendingStockActions) {
            this.pendingStockActions = pendingStockActions;
        }

        public void RequestStockAction(Guid requestID, string connectionID, string accountID, StockAction action, string symbol, int quantity) {
            this.pendingStockActions.RegisterPendingAction(symbol, quantity, requestID, action, accountID, connectionID);
        }

        public void ClearPendingStockActions(string accountID) {
            this.pendingStockActions.ClearPendingActions(accountID);
        }
    }
}