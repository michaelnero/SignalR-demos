using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace StockTrader.Windows.Balance {
    [Module(ModuleName = "BalanceModule")]
    public class BalanceModule : IModule {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public BalanceModule(IUnityContainer container, IRegionManager regionManager) {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize() {
        }
    }
}