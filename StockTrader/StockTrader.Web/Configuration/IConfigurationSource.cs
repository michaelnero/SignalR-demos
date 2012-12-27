using System.Configuration;
using StockTrader.Web.Properties;

namespace StockTrader.Web.Configuration {
    public interface IConfigurationSource {
        TSection GetSection<TSection>(string name, bool required = false) where TSection : ConfigurationSection;
    }

    [RegisterInContainer(typeof(IConfigurationSource))]
    public class DefaultConfigurationSource : IConfigurationSource {
        public TSection GetSection<TSection>(string name, bool required = true) where TSection : ConfigurationSection {
            var section = ConfigurationManager.GetSection(name) as TSection;

            if ((null == section) && required) {
                throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.DefaultConfigurationSource_ConfigSectionMissing, name));
            }

            return section;
        }
    }
}