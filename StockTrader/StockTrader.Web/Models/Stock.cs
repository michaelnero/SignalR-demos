using System.Runtime.Serialization;

namespace StockTrader.Web.Models {
    [DataContract(Name = "stock", Namespace = "http://stocktrader")]
    public class Stock {
        [DataMember(Name = "symbol", IsRequired = true)]
        public string Symbol { get; set; }

        [DataMember(Name = "quantity", IsRequired = true)]
        public int Quantity { get; set; }

        public void UpdateQuantity(int quantity) {
            lock (this) {
                this.Quantity = quantity;
            }
        }
    }
}