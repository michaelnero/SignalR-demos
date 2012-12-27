using System;
using Microsoft.Practices.Prism.ViewModel;
using StockTrader.Windows.Events;

namespace StockTrader.Windows.Trader.Views.History {
    public class StockActionModel : NotificationObject {
        private StockActionStatus status;
        private StockAction action;
        private string symbol;
        private int quantity;
        private decimal? price;
        private decimal? amount;

        public Guid RequestID { get; set; }

        public StockActionStatus Status {
            get { return this.status; }
            set {
                if (this.status != value) {
                    this.status = value;
                    this.RaisePropertyChanged(() => this.Status);
                }
            }
        }

        public StockAction Action {
            get { return this.action; }
            set {
                if (this.action != value) {
                    this.action = value;
                    this.RaisePropertyChanged(() => this.Action);
                }
            }
        }

        public string Symbol {
            get { return this.symbol; }
            set {
                if (this.symbol != value) {
                    this.symbol = value;
                    this.RaisePropertyChanged(() => this.Symbol);
                }
            }
        }

        public int Quantity {
            get { return this.quantity; }
            set {
                if (this.quantity != value) {
                    this.quantity = value;
                    this.RaisePropertyChanged(() => this.Quantity);
                }
            }
        }

        public decimal? Price {
            get { return this.price; }
            set {
                if (this.price != value) {
                    this.price = value;
                    this.RaisePropertyChanged(() => this.Price);
                }
            }
        }

        public decimal? Amount {
            get { return this.amount; }
            set {
                if (this.amount != value) {
                    this.amount = value;
                    this.RaisePropertyChanged(() => this.Amount);
                }
            }
        }
    }
}