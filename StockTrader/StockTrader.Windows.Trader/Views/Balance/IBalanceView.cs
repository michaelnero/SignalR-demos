namespace StockTrader.Windows.Trader.Views.Balance {
    public interface IBalanceView {
        BalancePresenter Presenter { get; set; }

        void SetBalance(decimal balance);

        void SetAccountID(string accountID);
    }
}