using System.Reflection;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using StockTrader.Windows.Common;
using StockTrader.Windows.Common.Configuration;
using StockTrader.Windows.StartUp.Views;

namespace StockTrader.Windows.StartUp {
    [Module(ModuleName = "StartUpModule")]
    [ModuleDependency("BrokerModule")]
    public class StartUpModule : IModule {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;

        public StartUpModule(IRegionManager regionManager, IUnityContainer container) {
            this.regionManager = regionManager;
            this.container = container;
        }

        public void Initialize() {
            ContainerRegistrar.DoRegistration(typeof(StartUpModule).GetTypeInfo().Assembly, this.container);

            var startUpPresenter = this.container.Resolve<StartUpPresenter>();
            this.regionManager.AddToRegion(RegionNames.MainRegion, startUpPresenter.View);
        }
    }
}