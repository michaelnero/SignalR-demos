using Microsoft.Practices.Prism.ViewModel;

namespace StockTrader.Windows.Trader.Views.Stocks {
    public class WatchedStockItemModel : NotificationObject, IStockItemModel {
        private decimal? currentPrice;
        private MovementIndicatorType movementIndicator;
        private decimal? previousPrice;

        public string Symbol { get; set; }

        public int Quantity { get; set; }

        public ColorRange ColorRange { get; set; }

        public decimal? CurrentPrice {
            get { return this.currentPrice; }
            set {
                if (this.currentPrice != value) {
                    this.currentPrice = value;
                    this.RaisePropertyChanged(() => this.CurrentPrice);
                }
            }
        }

        public MovementIndicatorType MovementIndicator {
            get { return this.movementIndicator; }
            set {
                if (this.movementIndicator != value) {
                    this.movementIndicator = value;
                    this.RaisePropertyChanged(() => this.MovementIndicator);
                }
            }
        }

        public decimal? PreviousPrice {
            get { return this.previousPrice; }
            set {
                if (this.previousPrice != value) {
                    this.previousPrice = value;
                    this.RaisePropertyChanged(() => this.PreviousPrice);
                }
            }
        }
    }
}