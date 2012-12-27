using System;
using System.Linq;
using StockTrader.Windows.Common;
using StockTrader.Windows.Common.Configuration;
using WinRTXamlToolkit.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace StockTrader.Windows.Trader.Views.Stocks {
    [RegisterInContainer(typeof(IStocksView))]
    public sealed partial class StocksView : IStocksView {
        public StocksView() {
            this.InitializeComponent();
        }

        public StocksPresenter Presenter { get; set; }

        public StocksModel Model {
            get { return (StocksModel) this.DataContext; }
            set { this.DataContext = value; }
        }

        public void AnimateStockItem(WatchedStockItemModel stockItem) {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => this.AnimateStockItemCore(stockItem));
        }

        private void AnimateStockItemCore(WatchedStockItemModel stockItem) {
            var animators = from wrapGrid in this.StockItems.Descendants<WrapGrid>()
                            from presenter in wrapGrid.Descendants<ContentPresenter>()
                            where presenter.Content == stockItem
                            from button in presenter.Descendants<Button>()
                            select new ButtonColorRangeAnimator(button, stockItem.ColorRange);

            foreach (var animator in animators) {
                animator.Begin();
            }
        }

        private async void AddStockButton_OnTapped(object sender, TappedRoutedEventArgs e) {
            var dialog = new InputDialog {
                AcceptButton = "Continue",
                CancelButton = "Cancel",
                IsLightDismissEnabled = true
            };

            string result = await dialog.ShowAsync("Subscribe to a stock symbol", "Please enter a stock symbol and a purchasing quantity. Example: MSFT or MSFT,1000", "Continue");

            if ((null != result) && !string.IsNullOrWhiteSpace(dialog.InputText)) {
                string[] parts = dialog.InputText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string symbolPart = parts.ElementAtOrDefault(0);
                if (!string.IsNullOrWhiteSpace(symbolPart)) {
                    symbolPart = symbolPart.Trim();

                    string quantityPart = parts.ElementAtOrDefault(1);
                    if (!string.IsNullOrWhiteSpace(quantityPart)) {
                        quantityPart = quantityPart.Trim();
                    }

                    int quantity;
                    if (!int.TryParse(quantityPart, out quantity)) {
                        quantity = 50;
                    }

                    this.Presenter.OnAddStock(symbolPart.ToUpperInvariant(), quantity);
                }
            }
        }

        private void WatchedStockButton_OnTapped(object sender, TappedRoutedEventArgs e) {
            e.Handled = true;

            this.AnimateToActionPanel(sender);
        }

        private void BuyButton_OnTapped(object sender, TappedRoutedEventArgs e) {
            e.Handled = true;

            var watchedStockItem = this.GetWatchedStockFrom(sender);
            if (null != watchedStockItem) {
                this.Presenter.OnBuyStock(watchedStockItem.Symbol, watchedStockItem.Quantity);
            }

            this.AnimateFromActionPanel(sender);
        }

        private void SellButton_OnTapped(object sender, TappedRoutedEventArgs e) {
            e.Handled = true;

            var watchedStockItem = this.GetWatchedStockFrom(sender);
            if (null != watchedStockItem) {
                this.Presenter.OnSellStock(watchedStockItem.Symbol, watchedStockItem.Quantity);
            }

            this.AnimateFromActionPanel(sender);
        }

        private void CancelButton_OnTapped(object sender, TappedRoutedEventArgs e) {
            e.Handled = true;

            this.AnimateFromActionPanel(sender);
        }

        private WatchedStockItemModel GetWatchedStockFrom(object sender) {
            var button = (Button)sender;

            var watchedStockItem = button.DataContext as WatchedStockItemModel;
            return watchedStockItem;
        }

        private void AnimateToActionPanel(object sender) {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => {
                var button = (Button) sender;

                var stackPanel = (StackPanel) button.FindName("RequestStockAction");
                var grid = (Grid) button.FindName("StockTicker");

                var flipAnimator = new FlipAnimator(grid, stackPanel);
                flipAnimator.Start();
            });
        }

        private void AnimateFromActionPanel(object sender) {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => {
                var button = (Button)sender;
                var watchedStockButton = button.Ancestors<Button>().First(b => b.Name == "WatchedStockButton");
                var stackPanel = (StackPanel)watchedStockButton.FindName("RequestStockAction");
                var grid = (Grid)watchedStockButton.FindName("StockTicker");

                var flipAnimator = new FlipAnimator(stackPanel, grid);
                flipAnimator.Start();
            });
        }
    }
}
