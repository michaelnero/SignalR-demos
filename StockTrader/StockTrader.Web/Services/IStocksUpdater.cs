using System.Threading.Tasks;

namespace StockTrader.Web.Services {
    public interface IStocksUpdater {
        Task Subscribe(string symbol, string connectionID);
        Task Unsubsribe(string symbol, string connectionID);
    }
}