using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace StockTrader.Windows.Common {
    public class StringFormatConverter : IValueConverter {
        public virtual object Convert(object value, Type targetType, object parameter, string language) {
            value = string.Format(CultureInfo.CurrentUICulture, (string) parameter, value);
            return value;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}