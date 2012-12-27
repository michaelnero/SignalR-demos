using Microsoft.Practices.Prism.ViewModel;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace StockTrader.Windows.Trader.Views.Stocks {
    public class ColorRange {
        public ColorRange(Color from, Color to) {
            this.From = from;
            this.To = to;
        }

        public Color From { get; set; }

        public Color To { get; set; }

        public SolidColorBrush FromBrush {
            get { return new SolidColorBrush(this.From); }
        }

        public SolidColorBrush ToBrush {
            get { return new SolidColorBrush(this.To); }
        }
    }
}