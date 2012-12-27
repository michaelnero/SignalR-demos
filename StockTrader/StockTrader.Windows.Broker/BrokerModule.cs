using System.Reflection;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using StockTrader.Windows.Common.Configuration;

namespace StockTrader.Windows.Broker {
    [Module(ModuleName = "BrokerModule")]
    public class BrokerModule : IModule {
        private readonly IUnityContainer container;

        public BrokerModule(IUnityContainer container) {
            this.container = container;
        }

        public void Initialize() {
            ContainerRegistrar.DoRegistration(typeof (BrokerModule).GetTypeInfo().Assembly, this.container);
        }
    }
}