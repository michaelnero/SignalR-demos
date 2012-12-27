using Microsoft.Practices.Prism.Events;
using StockTrader.Windows.Broker.Services;
using StockTrader.Windows.Events;

namespace StockTrader.Windows.StartUp.Views {
    public class StartUpPresenter {
        private readonly IStartUpView view;
        private readonly IStockTraderHubGateway hubGateway;

        private readonly SubscriptionToken onConnectionStateChangedEventSubscription;

        public StartUpPresenter(IStartUpView view, IStockTraderHubGateway hubGateway, IEventAggregator eventAggregator) {
            this.view = view;
            this.view.Presenter = this;
            this.view.TransitionToAccountEntryState();

            this.hubGateway = hubGateway;

            var onConnectionStateChangedEvent = eventAggregator.GetEvent<ConnectionStateChangedEvent>();
            this.onConnectionStateChangedEventSubscription = onConnectionStateChangedEvent.Subscribe(this.OnConnectionStateChanged, ThreadOption.UIThread);
        }

        public IStartUpView View {
            get { return this.view; }
        }

        public void OnContinue(string accountID) {
            this.view.TransitionToConnectingState();
            this.hubGateway.Connect(accountID);
        }

        private void OnConnectionStateChanged(ConnectionStateChangedEventArgs eventArgs) {
            if (eventArgs.HasFault) {
                this.view.TransitionToAccountEntryState();
                this.view.ShowError("There was an error connecting to the stock trading service. Please make sure that you are connected to the Internet or try again later.");
            } else if (eventArgs.IsConnected) {
                this.onConnectionStateChangedEventSubscription.Dispose();
            }
        }
    }
}