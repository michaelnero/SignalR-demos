using System.Reflection;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using StockTrader.Windows.Common.Configuration;

namespace StockTrader.Windows.Trader {
    [Module(ModuleName = "TraderModule")]
    [ModuleDependency("BrokerModule")]
    public class TraderModule : IModule {
        private readonly IUnityContainer container;
        
        public TraderModule(IUnityContainer container) {
            this.container = container;
        }

        public void Initialize() {
            ContainerRegistrar.DoRegistration(typeof(TraderModule).GetTypeInfo().Assembly, this.container);
            this.container.RegisterType<IDispatcherFacade, DefaultDispatcher>();

            var controller = this.container.Resolve<TraderController>();
            controller.Run();
        }
    }
}