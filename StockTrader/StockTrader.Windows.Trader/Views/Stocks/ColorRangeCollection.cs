using System.Collections.Generic;
using StockTrader.Windows.Common;

namespace StockTrader.Windows.Trader.Views.Stocks {
    public class ColorRangeCollection : LinkedList<ColorRange> {
        public ColorRangeCollection() {
            this.AddLast(new ColorRange(ColorTranslator.FromHtml("#749a02"), ColorTranslator.FromHtml("#91bd09")));
            this.AddLast(new ColorRange(ColorTranslator.FromHtml("#007d9a"), ColorTranslator.FromHtml("#2daebf")));
            this.AddLast(new ColorRange(ColorTranslator.FromHtml("#bc330d"), ColorTranslator.FromHtml("#e33100")));
            this.AddLast(new ColorRange(ColorTranslator.FromHtml("#630030"), ColorTranslator.FromHtml("#a9014b")));
            this.AddLast(new ColorRange(ColorTranslator.FromHtml("#d45500"), ColorTranslator.FromHtml("#ff5c00")));
            this.AddLast(new ColorRange(ColorTranslator.FromHtml("#fc9200"), ColorTranslator.FromHtml("#ffb515")));
        }

        public ColorRange GetNext() {
            var colorRange = this.First.Value;

            this.RemoveFirst();
            this.AddLast(colorRange);

            return colorRange;
        }
    }
}
