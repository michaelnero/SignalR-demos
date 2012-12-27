using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace StockTrader.Windows.Trader.Views.Stocks {
    public class ButtonColorRangeAnimator {
        private readonly Button button;
        private readonly ColorRange colorRange;

        public ButtonColorRangeAnimator(Button button, ColorRange colorRange) {
            this.button = button;
            this.colorRange = colorRange;
        }

        public void Begin() {
            var colorAnimation = new ColorAnimation {
                From = this.colorRange.From,
                To = this.colorRange.To,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(2)
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(colorAnimation);

            Storyboard.SetTargetProperty(colorAnimation, "(Control.Background).(SolidColorBrush.Color)");
            Storyboard.SetTarget(storyboard, button);

            storyboard.Begin();
        }
    }
}