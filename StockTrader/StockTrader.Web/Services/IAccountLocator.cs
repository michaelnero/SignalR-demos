using StockTrader.Web.Models;

namespace StockTrader.Web.Services {
    public interface IAccountLocator {
        Account GetAccount(string id);
    }
}