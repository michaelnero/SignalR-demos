using System.Globalization;
using StockTrader.Windows.Common.Configuration;

namespace StockTrader.Windows.Trader.Views.Balance {
    [RegisterInContainer(typeof(IBalanceView))]
    public sealed partial class BalanceView : IBalanceView {
        public BalanceView() {
            this.InitializeComponent();
        }

        public BalancePresenter Presenter { get; set; }

        public void SetBalance(decimal balance) {
            this.Balance.Text = balance.ToString("c", CultureInfo.CurrentUICulture);
        }

        public void SetAccountID(string accountID) {
            this.AccountID.Text = accountID;
        }
    }
}
