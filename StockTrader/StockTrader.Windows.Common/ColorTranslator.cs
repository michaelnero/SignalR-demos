using System;
using Windows.UI;

namespace StockTrader.Windows.Common {
    public sealed class ColorTranslator {
        public static Color FromHtml(string htmlColor) {
            var color = default(Color);
            if (string.IsNullOrEmpty(htmlColor)) {
                return color;
            }

            if (htmlColor[0] == 35 && (htmlColor.Length == 7 || htmlColor.Length == 4)) {
                if (htmlColor.Length == 7) {
                    color = Color.FromArgb(255, Convert.ToByte(htmlColor.Substring(1, 2), 16), Convert.ToByte(htmlColor.Substring(3, 2), 16), Convert.ToByte(htmlColor.Substring(5, 2), 16));
                } else {
                    string str1 = char.ToString(htmlColor[1]);
                    string str2 = char.ToString(htmlColor[2]);
                    string str3 = char.ToString(htmlColor[3]);
                    color = Color.FromArgb(255, Convert.ToByte(str1 + str1, 16), Convert.ToByte(str2 + str2, 16), Convert.ToByte(str3 + str3, 16));
                }
            }
            
            return color;
        }
    }
}