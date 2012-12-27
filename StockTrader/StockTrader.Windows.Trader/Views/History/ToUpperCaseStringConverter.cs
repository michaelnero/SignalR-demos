using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace StockTrader.Windows.Trader.Views.History {
    public class ToUpperCaseStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            value = System.Convert.ToString(value, CultureInfo.CurrentUICulture).ToUpper();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return value;
        }
    }
}