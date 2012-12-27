namespace StockTrader.Windows.StartUp.Views {
    public interface IStartUpView {
        StartUpPresenter Presenter { get; set; }

        void ShowError(string errorText);

        void TransitionToAccountEntryState();

        void TransitionToConnectingState();
    }
}