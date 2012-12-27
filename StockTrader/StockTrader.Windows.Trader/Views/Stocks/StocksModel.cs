using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;

namespace StockTrader.Windows.Trader.Views.Stocks {
    public class StocksModel : NotificationObject {
        private readonly ObservableCollection<IStockItemModel> stockItems = new ObservableCollection<IStockItemModel>();

        public ObservableCollection<IStockItemModel> StockItems {
            get { return this.stockItems; }
        }
    }
}