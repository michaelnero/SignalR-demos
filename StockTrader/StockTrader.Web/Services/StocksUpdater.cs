using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using StockTrader.Web.Configuration;
using StockTrader.Web.Hubs;

namespace StockTrader.Web.Services {
    [RegisterInContainer(typeof(IStocksUpdater), RegistrationType.Singleton)]
    [RegisterInContainer(typeof(IStockPriceProvider), RegistrationType.Singleton)]
    public class StocksUpdater : IStockPriceProvider, IStocksUpdater {
        private readonly IHubContext context;
        private readonly Dictionary<string, Stock> subscribedStocks;
        private readonly Timer subscriptionTimer;

        public StocksUpdater() {
            this.context = GlobalHost.ConnectionManager.GetHubContext<StockTraderHub>();
            this.subscribedStocks = new Dictionary<string, Stock>();
            this.subscriptionTimer = new Timer(this.UpdateSubscriptions, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        public Task Subscribe(string symbol, string connectionID) {
            lock (this.subscribedStocks) {
                if (!this.subscribedStocks.ContainsKey(symbol)) {
                    this.subscribedStocks[symbol] = CreateNewStock(symbol);
                }
            }

            return this.context.Groups.Add(connectionID, symbol);
        }

        public Task Unsubsribe(string symbol, string connectionID) {
            return this.context.Groups.Remove(connectionID, symbol);
        }

        public IEnumerable<SymbolWithPrice> GetPricesFor(IEnumerable<string> symbols) {
            var prices = new List<SymbolWithPrice>();

            lock (this.subscribedStocks) {
                foreach (var symbol in symbols) {
                    Stock stock;
                    if (!this.subscribedStocks.TryGetValue(symbol, out stock)) {
                        this.subscribedStocks[symbol] = stock = CreateNewStock(symbol);
                    }

                    prices.Add(new SymbolWithPrice(symbol, stock.CurrentPrice));
                }
            }

            return prices;
        }

        private void UpdateSubscriptions(object ignore) {
            List<Stock> stocksToUpdate;

            lock (this.subscribedStocks) {
                DateTime now = DateTime.Now;

                stocksToUpdate = (from pair in this.subscribedStocks
                                  where UpdatedStock(pair.Value, now)
                                  select new Stock(pair.Value)).ToList();
            }

            foreach (var stock in stocksToUpdate) {
                var group = this.context.Clients.Group(stock.Symbol);
                group.StockPriceUpdated(stock.Symbol, stock.CurrentPrice);
            }
        }

        private static Stock CreateNewStock(string symbol) {
            decimal decimalToAdd = RandomNumber.GetRandomDecimal();
            int intToAdd = RandomNumber.GetRandomInt(300);

            return new Stock(symbol, GetNextUpdate(), GetNextPrice(intToAdd + decimalToAdd));
        }

        private static bool UpdatedStock(Stock stock, DateTime now) {
            if (stock.NextUpdate < now) {
                stock.NextUpdate = GetNextUpdate();
                stock.CurrentPrice = GetNextPrice(stock.CurrentPrice);

                return true;
            }

            return false;
        }

        private static decimal GetNextPrice(decimal lastPrice) {
            decimal nextPrice = lastPrice;

            decimal decimalToAdd = RandomNumber.GetRandomDecimal();
            int intToAdd = RandomNumber.GetRandomInt(3);
            if (RandomNumber.GetRandomDouble() < 0.5d) {
                decimalToAdd = -decimalToAdd;
                intToAdd = -intToAdd;
            }

            nextPrice += decimalToAdd;
            nextPrice += intToAdd;

            if (0 > nextPrice) {
                nextPrice = 0;
            }

            return nextPrice;
        }

        private static DateTime GetNextUpdate() {
            int secondsInFuture = RandomNumber.GetRandomInt(20);

            DateTime nextUpdate = DateTime.Now.AddSeconds(secondsInFuture);
            return nextUpdate;
        }

        private class Stock {
            public readonly string Symbol;

            public DateTime NextUpdate;

            public decimal CurrentPrice;

            public Stock(string symbol, DateTime nextUpdate, decimal currentPrice) {
                this.Symbol = symbol;
                this.NextUpdate = nextUpdate;
                this.CurrentPrice = currentPrice;
            }

            public Stock(Stock other) {
                this.Symbol = other.Symbol;
                this.NextUpdate = other.NextUpdate;
                this.CurrentPrice = other.CurrentPrice;
            }
        }
    }
}