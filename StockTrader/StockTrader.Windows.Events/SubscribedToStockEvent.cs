using Microsoft.Practices.Prism.Events;

namespace StockTrader.Windows.Events {
    public class SubscribedToStockEventArgs {
        public SubscribedToStockEventArgs(string symbol) {
            this.Symbol = symbol;
        }

        public string Symbol { get; private set; }
    }

    public class SubscribedToStockEvent : CompositePresentationEvent<SubscribedToStockEventArgs> {
         
    }
}