using StockTrader.Windows.Common.Configuration;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace StockTrader.Windows.StartUp.Views {
    [RegisterInContainer(typeof(IStartUpView))]
    public sealed partial class StartUpView : IStartUpView {
        public StartUpView() {
            this.InitializeComponent();
        }

        public StartUpPresenter Presenter { get; set; }

        public void ShowError(string errorText) {
            var messageDialog = new MessageDialog(errorText, "An error has occurred");
            messageDialog.ShowAsync();
        }

        public void TransitionToAccountEntryState() {
            VisualStateManager.GoToState(this, "AccountNumberState", true);
        }

        public void TransitionToConnectingState() {
            VisualStateManager.GoToState(this, "ConnectingState", true);
        }

        private void Continue_Click(object sender, RoutedEventArgs e) {
            if (!string.IsNullOrWhiteSpace(this.AccountNumber.Text)) {
                this.Presenter.OnContinue(this.AccountNumber.Text);
            }
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e) {
            this.AccountNumber.Focus(FocusState.Programmatic);
        }
    }
}
