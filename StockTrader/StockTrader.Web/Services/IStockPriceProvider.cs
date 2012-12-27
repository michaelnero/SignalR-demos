using System.Collections.Generic;

namespace StockTrader.Web.Services {
    public interface IStockPriceProvider {
        IEnumerable<SymbolWithPrice> GetPricesFor(IEnumerable<string> symbols); 
    }
}