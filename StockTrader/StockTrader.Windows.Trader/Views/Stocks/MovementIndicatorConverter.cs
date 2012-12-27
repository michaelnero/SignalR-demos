using System;
using Windows.UI.Xaml.Data;

namespace StockTrader.Windows.Trader.Views.Stocks {
    public class MovementIndicatorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var movementIndicator = (MovementIndicatorType) value;

            if (MovementIndicatorType.NoActivity == movementIndicator) {
                return string.Empty;
            }

            if (MovementIndicatorType.Increased == movementIndicator) {
                return "⬆";
            }

            return "⬇";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}