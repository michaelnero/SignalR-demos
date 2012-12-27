using System;
using Microsoft.Practices.Prism.Events;

namespace StockTrader.Windows.Events {
    public class StockActionRequestedEventArgs {
        public StockActionRequestedEventArgs(Guid requestID, StockAction action, string symbol, int quantity) {
            this.RequestID = requestID;
            this.Action = action;
            this.Symbol = symbol;
            this.Quantity = quantity;
        }

        public Guid RequestID { get; private set; }

        public StockAction Action { get; private set; }

        public string Symbol { get; private set; }

        public int Quantity { get; private set; }
    }

    public class StockActionRequestedEvent : CompositePresentationEvent<StockActionRequestedEventArgs> {
         
    }
}