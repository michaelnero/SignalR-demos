namespace StockTrader.Windows.Trader.Views.History {
    public interface IHistoryView {
        HistoryPresenter Presenter { get; set; }

        HistoryModel Model { get; set; }
    }
}