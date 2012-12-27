namespace StockTrader.Web.Services {
    public struct SymbolWithPrice {
        public SymbolWithPrice(string symbol, decimal price)
            : this() {
            this.Symbol = symbol;
            this.Price = price;
        }

        public string Symbol { get; private set; }

        public decimal Price { get; private set; } 
    }
}