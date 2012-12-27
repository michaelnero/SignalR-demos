using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.Practices.Prism.Events;
using StockTrader.Windows.Common.Configuration;
using StockTrader.Windows.Events;

namespace StockTrader.Windows.Broker.Services {
    [RegisterInContainer(typeof(IStockTraderHubGateway), RegistrationType.Singleton)]
    public class StockTraderHubGateway : IStockTraderHubGateway {
        private readonly IEventAggregator eventAggregator;
        private readonly HubConnection connection;
        private readonly IHubProxy proxy;
        private readonly HubEventsListener hubEventsListener;
        private readonly UserEventsListener userEventsListener;

        public StockTraderHubGateway(IEventAggregator eventAggregator) {
            this.eventAggregator = eventAggregator;
            this.connection = new HubConnection("http://localhost:14635");
            this.proxy = this.connection.CreateHubProxy("trader");
            this.hubEventsListener = new HubEventsListener(this.eventAggregator, this.proxy);
            this.userEventsListener = new UserEventsListener(this.eventAggregator, this.proxy);
        }

        public void Connect(string accountID) {
            this.connection.Stop();

            if (ConnectionState.Connected != this.connection.State) {
                this.proxy["AccountID"] = accountID;

                this.connection.Start()
                    .ContinueWith(task => {
                        var eventArgs = task.IsFaulted ? new ConnectionStateChangedEventArgs(false, true) : new ConnectionStateChangedEventArgs(true, false);

                        var onConnectionStateChangedEvent = this.eventAggregator.GetEvent<ConnectionStateChangedEvent>();
                        onConnectionStateChangedEvent.Publish(eventArgs);

                        var balanceRequestedEvent = this.eventAggregator.GetEvent<BalanceRequestedEvent>();
                        balanceRequestedEvent.Publish(new BalanceRequestedEventArgs());
                    });
            }
        }
    }
}