using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.Practices.Prism.Events;
using StockTrader.Windows.Events;

namespace StockTrader.Windows.Broker.Services {
    public class UserEventsListener {
        private readonly IEventAggregator eventAggregator;
        private readonly IHubProxy proxy;

        public UserEventsListener(IEventAggregator eventAggregator, IHubProxy proxy) {
            this.eventAggregator = eventAggregator;
            this.proxy = proxy;

            var requestStockActionEvent = this.eventAggregator.GetEvent<StockActionRequestedEvent>();
            requestStockActionEvent.Subscribe(this.OnRequestStockAction);

            var subscribeToStockEvent = this.eventAggregator.GetEvent<SubscribedToStockEvent>();
            subscribeToStockEvent.Subscribe(this.OnSubscribeToStock);

            var unsubscribeFromStockEvent = this.eventAggregator.GetEvent<UnsubscribedFromStockEvent>();
            unsubscribeFromStockEvent.Subscribe(this.OnUnsubscribeFromStock);

            var balanceRequestedEvent = this.eventAggregator.GetEvent<BalanceRequestedEvent>();
            balanceRequestedEvent.Subscribe(this.OnBalanceRequested);
        }

        private void OnRequestStockAction(StockActionRequestedEventArgs eventArgs) {
            this.proxy.Invoke("RequestStockAction", eventArgs.RequestID, eventArgs.Action, eventArgs.Symbol, eventArgs.Quantity);
        }

        private void OnSubscribeToStock(SubscribedToStockEventArgs eventArgs) {
            this.proxy.Invoke("SubscribeToStock", eventArgs.Symbol);
        }

        private void OnUnsubscribeFromStock(UnsubscribedFromStockEventArgs eventArgs) {
            this.proxy.Invoke("UnsubscribeFromStock", eventArgs.Symbol);
        }

        private void OnBalanceRequested(BalanceRequestedEventArgs obj) {
            this.proxy.Invoke("RequestAccountBalance");
        }
    }
}