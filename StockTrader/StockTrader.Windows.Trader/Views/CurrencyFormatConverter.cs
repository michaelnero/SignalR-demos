using System;
using StockTrader.Windows.Common;

namespace StockTrader.Windows.Trader.Views {
    public class CurrencyFormatConverter : StringFormatConverter {
        public override object Convert(object value, Type targetType, object parameter, string language) {
            return (null == value) ? "$ ---" : base.Convert(value, targetType, "{0:c}", language);
        }
    }
}