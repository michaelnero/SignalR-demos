using StockTrader.Web.Models;

namespace StockTrader.Web.Services {
    public interface IAccountPersister {
        void SaveAccount(Account account);
    }
}