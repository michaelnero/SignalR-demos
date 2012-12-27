using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace StockTrader.Windows.Trader.Views.History {
    public class CollectionCountToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            int count = (int) value;
            if (0 == count) {
                return Visibility.Visible;
            } else {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return value;
        }
    }
}