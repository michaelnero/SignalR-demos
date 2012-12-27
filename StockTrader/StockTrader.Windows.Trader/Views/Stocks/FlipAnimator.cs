using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace StockTrader.Windows.Trader.Views.Stocks {
    public class FlipAnimator {
        private readonly FrameworkElement elementToHide;
        private readonly FrameworkElement elementToShow;
        private readonly Storyboard hideStoryboard;
        private readonly Storyboard showStoryboard;

        public FlipAnimator(FrameworkElement elementToHide, FrameworkElement elementToShow) {
            this.elementToHide = elementToHide;
            this.elementToShow = elementToShow;

            this.hideStoryboard = new Storyboard();

            var hideAnimation = new DoubleAnimation { From = 1, To = 0, Duration = new Duration(TimeSpan.FromSeconds(0.1)) };
            Storyboard.SetTargetProperty(hideAnimation, "(FrameworkElement.RenderTransform).(ScaleTransform.ScaleX)");
            Storyboard.SetTarget(this.hideStoryboard, this.elementToHide);

            this.hideStoryboard.Children.Add(hideAnimation);

            this.showStoryboard = new Storyboard();

            var showAnimation = new DoubleAnimation { From = 0, To = 1, Duration = new Duration(TimeSpan.FromSeconds(0.1)) };
            Storyboard.SetTargetProperty(showAnimation, "(FrameworkElement.RenderTransform).(ScaleTransform.ScaleX)");
            Storyboard.SetTarget(this.showStoryboard, this.elementToShow);

            this.showStoryboard.Children.Add(showAnimation);
        }

        public void Start() {
            this.StartHideAnimation();
        }

        private void StartHideAnimation() {
            try {
                this.hideStoryboard.Completed += this.HideElementToHide;
                this.hideStoryboard.Begin();
            } catch {
                this.hideStoryboard.Completed -= this.HideElementToHide;
            }
        }

        private void HideElementToHide(object sender, object eventArgs) {
            this.hideStoryboard.Completed -= this.HideElementToHide;
            
            this.elementToHide.Visibility = Visibility.Collapsed;

            this.StartShowAnimation();
        }

        private void StartShowAnimation() {
            try {
                this.showStoryboard.Completed += this.ShowElementToShow;
                this.showStoryboard.Begin();
            } catch {
                this.showStoryboard.Completed -= this.ShowElementToShow;
                throw;
            }
        }

        private void ShowElementToShow(object sender, object eventArgs) {
            this.showStoryboard.Completed -= this.ShowElementToShow;

            this.elementToShow.Visibility = Visibility.Visible;
        }
    }
}