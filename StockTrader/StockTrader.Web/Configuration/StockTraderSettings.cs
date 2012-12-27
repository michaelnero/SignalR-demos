using System.Configuration;

namespace StockTrader.Web.Configuration {
    public class StockTraderSettings : ConfigurationSection {
        public const string SectionName = "stockTrader";

        private const string dataPathProperty = "dataPath";

        [ConfigurationProperty(dataPathProperty, IsRequired = true)]
        public string DataPath {
            get { return (string) this[dataPathProperty]; }
            set { this[dataPathProperty] = value; }
        }
    }
}