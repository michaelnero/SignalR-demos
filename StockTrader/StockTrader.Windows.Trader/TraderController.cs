using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using StockTrader.Windows.Common;
using StockTrader.Windows.Events;
using StockTrader.Windows.Trader.Views.Balance;
using StockTrader.Windows.Trader.Views.History;
using StockTrader.Windows.Trader.Views.Shell;
using StockTrader.Windows.Trader.Views.Stocks;

namespace StockTrader.Windows.Trader {
    public class TraderController {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;

        private SubscriptionToken onConnectionStateChangedEventSubscription;

        public TraderController(IEventAggregator eventAggregator, IRegionManager regionManager, IUnityContainer container) {
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            this.container = container;
        }

        public void Run() {
            var onConnectionStateChangedEvent = this.eventAggregator.GetEvent<ConnectionStateChangedEvent>();
            this.onConnectionStateChangedEventSubscription = onConnectionStateChangedEvent.Subscribe(this.OnConnectionStateChanged, ThreadOption.UIThread, true);
        }

        private void OnConnectionStateChanged(ConnectionStateChangedEventArgs eventArgs) {
            if (eventArgs.IsConnected) {
                this.onConnectionStateChangedEventSubscription.Dispose();
                this.onConnectionStateChangedEventSubscription = null;

                var shellPresenter = this.container.Resolve<ShellPresenter>();
                this.regionManager.AddToRegion(RegionNames.MainRegion, shellPresenter.View);

                var balancePresenter = this.container.Resolve<BalancePresenter>();
                this.regionManager.AddToRegion(RegionNames.BalanceRegion, balancePresenter.View);

                var stocksPresenter = this.container.Resolve<StocksPresenter>();
                this.regionManager.AddToRegion(RegionNames.StocksRegion, stocksPresenter.View);

                var historyPresenter = this.container.Resolve<HistoryPresenter>();
                this.regionManager.AddToRegion(RegionNames.HistoryRegion, historyPresenter.View);
            }
        }
    }
}