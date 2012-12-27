using System.Linq;
using Microsoft.Practices.Prism.Events;
using StockTrader.Windows.Events;

namespace StockTrader.Windows.Trader.Views.History {
    public class HistoryPresenter {
        private readonly IHistoryView view;

        public HistoryPresenter(IHistoryView view, IEventAggregator eventAggregator) {
            this.view = view;
            this.view.Presenter = this;
            this.view.Model = new HistoryModel();

            var stockActionExecutedEvent = eventAggregator.GetEvent<StockActionExecutedEvent>();
            stockActionExecutedEvent.Subscribe(this.OnStockActionExecuted, ThreadOption.UIThread);
        }

        public IHistoryView View {
            get { return this.view; }
        }

        private void OnStockActionExecuted(StockActionExecutedEventArgs eventArgs) {
            var stockAction = this.view.Model.Actions.FirstOrDefault(a => a.RequestID == eventArgs.RequestID);
            
            if (null == stockAction) {
                stockAction = new StockActionModel();
                this.view.Model.Actions.Insert(0, stockAction);
            }

            stockAction.Action = eventArgs.Action;
            stockAction.Amount = eventArgs.Amount;
            stockAction.Quantity = eventArgs.Quantity;
            stockAction.RequestID = eventArgs.RequestID;
            stockAction.Status = eventArgs.Status;
            stockAction.Symbol = eventArgs.Symbol;
            stockAction.Price = eventArgs.Price;
        }
    }
}