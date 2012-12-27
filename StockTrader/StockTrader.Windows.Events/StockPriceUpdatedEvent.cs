using Microsoft.Practices.Prism.Events;

namespace StockTrader.Windows.Events {
    public class StockPriceUpdatedEventArgs {
        public StockPriceUpdatedEventArgs(string symbol, decimal? price) {
            this.Price = price;
            this.Symbol = symbol;
        }

        public string Symbol { get; private set; }

        public decimal? Price { get; private set; }
    }

    public class StockPriceUpdatedEvent : CompositePresentationEvent<StockPriceUpdatedEventArgs> {
    }
}