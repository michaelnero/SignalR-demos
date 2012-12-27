using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;

namespace StockTrader.Windows.Trader.Views.History {
    public class HistoryModel : NotificationObject {
        private readonly ObservableCollection<StockActionModel> actions = new ObservableCollection<StockActionModel>();

        public ObservableCollection<StockActionModel> Actions {
            get { return this.actions; }
        } 
    }
}