using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StockTrader.Web.Models {
    [DataContract(Name = "account", Namespace = "http://stocktrader")]
    public class Account {
        private List<Stock> stocks;

        [DataMember(Name = "id", IsRequired = true)]
        public string ID { get; set; }

        [DataMember(Name = "balance", IsRequired = true)]
        public decimal Balance { get; set; }

        [DataMember(Name = "stocks")]
        public List<Stock> Stocks {
            get {
                lock (this) {
                    return this.stocks ?? (this.stocks = new List<Stock>());
                }
            }
            set { this.stocks = value; }
        }

        public bool TryPurchaseStock(string symbol, int quantity, decimal price, out decimal newBalance) {
            lock (this) {
                decimal purchasePrice = price * quantity;

                decimal temp = this.Balance - purchasePrice;
                if (0 < temp) {
                    newBalance = this.Balance = temp;

                    var stock = this.Stocks.Find(s => s.Symbol == symbol);
                    if (null == stock) {
                        stock = new Stock {
                            Symbol = symbol
                        };

                        this.Stocks.Add(stock);
                    }

                    stock.Quantity += quantity;
                    return true;
                }

                newBalance = 0m;
                return false;
            }
        }

        public bool TrySellStock(string symbol, int quantity, decimal price, out decimal newBalance) {
            lock (this) {
                var stock = this.Stocks.Find(s => s.Symbol == symbol);
                if (null != stock) {
                    int remainingQuantity = stock.Quantity - quantity;
                    if (0 <= remainingQuantity) {
                        stock.Quantity = remainingQuantity;

                        newBalance = (this.Balance += price*quantity);
                        return true;
                    }
                }

                newBalance = 0m;
                return false;
            }
        }
    }
}