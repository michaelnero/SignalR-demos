using System;
using Microsoft.Practices.Prism.Events;

namespace StockTrader.Windows.Events {
    public class StockActionExecutedEventArgs {
        public StockActionExecutedEventArgs(Guid requestID, StockActionStatus status, StockAction action, string symbol, int quantity, decimal? price, decimal? amount) {
            this.RequestID = requestID;
            this.Status = status;
            this.Action = action;
            this.Amount = amount;
            this.Quantity = quantity;
            this.Price = price;
            this.Symbol = symbol;
        }

        public Guid RequestID { get; private set; }

        public StockActionStatus Status { get; private set; }

        public StockAction Action { get; private set; }

        public string Symbol { get; private set; }

        public int Quantity { get; private set; }

        public decimal? Price { get; private set; }

        public decimal? Amount { get; private set; }
    }

    public class StockActionExecutedEvent : CompositePresentationEvent<StockActionExecutedEventArgs> {
         
    }
}