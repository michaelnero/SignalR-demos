namespace StockTrader.Windows.Trader.Views.Stocks {
    public interface IStocksView {
        StocksPresenter Presenter { get; set; }

        StocksModel Model { get; set; }

        void AnimateStockItem(WatchedStockItemModel stockItem);
    }
}