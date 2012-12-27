using System;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.Practices.Prism.Events;
using StockTrader.Windows.Events;

namespace StockTrader.Windows.Broker.Services {
    public class HubEventsListener {
        private readonly IEventAggregator eventAggregator;
        private readonly IHubProxy proxy;

        public HubEventsListener(IEventAggregator eventAggregator, IHubProxy proxy) {
            this.eventAggregator = eventAggregator;
            this.proxy = proxy;

            this.proxy.On<string, decimal>("StockPriceUpdated", this.OnStockPriceUpdated);
            this.proxy.On<Guid, StockActionStatus, StockAction, string, int, decimal?, decimal?>("StockActionExecuted", this.OnStockActionExecuted);
            this.proxy.On<string, decimal>("BalanceUpdated", this.OnBalanceUpdated);
        }

        private void OnStockPriceUpdated(string symbol, decimal price) {
            var stockPriceUpdatedEvent = this.eventAggregator.GetEvent<StockPriceUpdatedEvent>();
            stockPriceUpdatedEvent.Publish(new StockPriceUpdatedEventArgs(symbol, price));
        }

        private void OnStockActionExecuted(Guid requestID, StockActionStatus status, StockAction action, string symbol, int quantity, decimal? price, decimal? amount) {
            var stockActionExecutedEvent = this.eventAggregator.GetEvent<StockActionExecutedEvent>();
            stockActionExecutedEvent.Publish(new StockActionExecutedEventArgs(requestID, status, action, symbol, quantity, price, amount));
        }

        private void OnBalanceUpdated(string accountID, decimal balance) {
            var balanceUpdatedEvent = this.eventAggregator.GetEvent<BalanceUpdatedEvent>();
            balanceUpdatedEvent.Publish(new BalanceUpdatedEventArgs(accountID, balance));
        }
    }
}