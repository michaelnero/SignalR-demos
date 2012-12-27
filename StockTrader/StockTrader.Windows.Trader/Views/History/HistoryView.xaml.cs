using StockTrader.Windows.Common.Configuration;

namespace StockTrader.Windows.Trader.Views.History {
    [RegisterInContainer(typeof(IHistoryView))]
    public sealed partial class HistoryView : IHistoryView {
        public HistoryView() {
            this.InitializeComponent();
        }

        public HistoryPresenter Presenter { get; set; }

        public HistoryModel Model {
            get { return (HistoryModel) this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}
