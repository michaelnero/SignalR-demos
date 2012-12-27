using System;
using StockTrader.Web.Models;

namespace StockTrader.Web.Services {
    public class PendingAction {
        public PendingAction(string symbol, int quantity, Guid historyID, StockAction actionType, string accountID, string connectionID) {
            this.Symbol = symbol;
            this.Quantity = quantity;
            this.HistoryID = historyID;
            this.ActionType = actionType;
            this.AccountID = accountID;
            this.ConnectionID = connectionID;

            this.CyclesToWait = RandomNumber.GetRandomInt(5);
        }

        public string Symbol { get; private set; }

        public int Quantity { get; private set; }

        public Guid HistoryID { get; private set; }

        public string AccountID { get; private set; }

        public string ConnectionID { get; set; }

        public int CyclesToWait { get; set; }

        public StockAction ActionType { get; private set; } 
    }
}