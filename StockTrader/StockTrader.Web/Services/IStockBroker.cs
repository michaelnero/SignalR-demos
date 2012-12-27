using System;

namespace StockTrader.Web.Services {
    public interface IStockBroker {
        void RequestStockAction(Guid requestID, string connectionID, string accountID, StockAction action, string symbol, int quantity);

        void ClearPendingStockActions(string accountID);
    }
}