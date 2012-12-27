namespace StockTrader.Windows.Broker.Services {
    public interface IStockTraderHubGateway {
        void Connect(string accountID);
    }
}