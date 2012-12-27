using System;
using Microsoft.Practices.Prism.Events;
using StockTrader.Windows.Events;

namespace StockTrader.Windows.Trader.Views.Stocks {
    public class StocksPresenter {
        private readonly ColorRangeCollection colorRanges = new ColorRangeCollection();

        private readonly IStocksView view;
        private readonly IEventAggregator eventAggregator;

        public StocksPresenter(IStocksView view, IEventAggregator eventAggregator) {
            this.view = view;
            this.view.Presenter = this;

            var model = new StocksModel();
            model.StockItems.Add(new AddStockItemModel());
            this.view.Model = model;

            this.eventAggregator = eventAggregator;
            var stockPriceUpdatedEvent = this.eventAggregator.GetEvent<StockPriceUpdatedEvent>();
            stockPriceUpdatedEvent.Subscribe(this.OnStockPriceUpdated, ThreadOption.UIThread);

            //this.OnAddStock("Q", 5);
            //this.OnAddStock("W", 5);
            //this.OnAddStock("E", 5);
            //this.OnAddStock("R", 5);
            //this.OnAddStock("T", 5);
            //this.OnAddStock("Y", 5);
            //this.OnAddStock("U", 5);
            //this.OnAddStock("I", 5);
            //this.OnAddStock("O", 5);
            //this.OnAddStock("P", 5);
            //this.OnAddStock("A", 5);
            //this.OnAddStock("S", 5);
            //this.OnAddStock("D", 5);
            //this.OnAddStock("F", 5);
            //this.OnAddStock("G", 5);
            //this.OnAddStock("H", 5);
            //this.OnAddStock("J", 5);
            //this.OnAddStock("K", 5);
            //this.OnAddStock("L", 5);
            //this.OnAddStock("Z", 5);
            //this.OnAddStock("X", 5);
            //this.OnAddStock("C", 5);
            //this.OnAddStock("V", 5);
            //this.OnAddStock("B", 5);
            //this.OnAddStock("N", 5);
            //this.OnAddStock("M", 5);
        }

        public IStocksView View {
            get { return this.view; }
        }

        public void OnAddStock(string symbol, int quantity) {
            var item = new WatchedStockItemModel {
                Symbol = symbol,
                ColorRange = this.colorRanges.GetNext(),
                Quantity = quantity
            };

            int index = this.view.Model.StockItems.Count - 1;
            this.view.Model.StockItems.Insert(index, item);

            var subscribedToStockEvent = this.eventAggregator.GetEvent<SubscribedToStockEvent>();
            subscribedToStockEvent.Publish(new SubscribedToStockEventArgs(symbol));
        }

        public void OnBuyStock(string symbol, int quantity) {
            this.PublishStockActionRequest(StockAction.Buy, symbol, quantity);
        }

        public void OnSellStock(string symbol, int quantity) {
            this.PublishStockActionRequest(StockAction.Sell, symbol, quantity);
        }

        private void PublishStockActionRequest(StockAction action, string symbol, int quantity) {
            var stockActionRequestedEvent = this.eventAggregator.GetEvent<StockActionRequestedEvent>();
            stockActionRequestedEvent.Publish(new StockActionRequestedEventArgs(Guid.NewGuid(), action, symbol, quantity));
        }

        private void OnStockPriceUpdated(StockPriceUpdatedEventArgs eventArgs) {
            foreach (var stockItem in this.view.Model.StockItems) {
                var watchedItem = stockItem as WatchedStockItemModel;
                if ((null != watchedItem) && (string.Equals(watchedItem.Symbol, eventArgs.Symbol, StringComparison.OrdinalIgnoreCase))) {
                    decimal? currentPrice = watchedItem.CurrentPrice;

                    watchedItem.CurrentPrice = eventArgs.Price;
                    watchedItem.PreviousPrice = currentPrice;

                    bool shouldAnimate = true;

                    if (watchedItem.CurrentPrice > watchedItem.PreviousPrice) {
                        watchedItem.MovementIndicator = MovementIndicatorType.Increased;
                    } else if (watchedItem.CurrentPrice < watchedItem.PreviousPrice) {
                        watchedItem.MovementIndicator = MovementIndicatorType.Decreased;
                    } else {
                        watchedItem.MovementIndicator = MovementIndicatorType.NoActivity;
                        shouldAnimate = false;
                    }

                    if (shouldAnimate) {
                        this.view.AnimateStockItem(watchedItem);
                    }
                }
            }
        }
    }
}