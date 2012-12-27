using System;
using StockTrader.Windows.Events;
using Windows.UI.Xaml.Data;

namespace StockTrader.Windows.Trader.Views.History {
    public class StatusToCharConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var actionStatus = (StockActionStatus) value;

            if (StockActionStatus.Completed == actionStatus) {
                return '\ue19f';
            } else if (StockActionStatus.Rejected == actionStatus) {
                return '\ue19e';
            } else {
                return '\ue19d';
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return value;
        }
    }
}