using Microsoft.Practices.Prism.Events;

namespace StockTrader.Windows.Events {
    public class UnsubscribedFromStockEventArgs {
        public UnsubscribedFromStockEventArgs(string symbol) {
            this.Symbol = symbol;
        }

        public string Symbol { get; private set; }
    }

    public class UnsubscribedFromStockEvent : CompositePresentationEvent<UnsubscribedFromStockEventArgs> {
         
    }
}