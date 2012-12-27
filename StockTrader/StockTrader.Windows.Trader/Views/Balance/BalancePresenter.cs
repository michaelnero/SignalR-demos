using Microsoft.Practices.Prism.Events;
using StockTrader.Windows.Events;

namespace StockTrader.Windows.Trader.Views.Balance {
    public class BalancePresenter {
        private readonly IBalanceView view;

        public BalancePresenter(IBalanceView view, IEventAggregator eventAggregator) {
            this.view = view;
            this.view.Presenter = this;
            this.view.SetBalance(0m);

            var onBalanceUpdatedEvent = eventAggregator.GetEvent<BalanceUpdatedEvent>();
            onBalanceUpdatedEvent.Subscribe(this.OnBalanceUpdated, ThreadOption.UIThread);
        }

        public IBalanceView View {
            get { return this.view; }
        }

        private void OnBalanceUpdated(BalanceUpdatedEventArgs eventArgs) {
            this.view.SetAccountID(eventArgs.AccountID);
            this.view.SetBalance(eventArgs.Balance);
        }
    }
}