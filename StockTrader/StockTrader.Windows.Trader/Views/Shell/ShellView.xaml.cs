using Microsoft.Practices.Prism.Regions;
using StockTrader.Windows.Common;
using StockTrader.Windows.Common.Configuration;

namespace StockTrader.Windows.Trader.Views.Shell {
    [RegisterInContainer(typeof(IShellView))]
    public sealed partial class ShellView : IShellView {
        public ShellView(IRegionManager regionManager) {
            this.InitializeComponent();

            RegionManager.SetRegionManager(this.BalanceRegion, regionManager);
            RegionManager.SetRegionName(this.BalanceRegion, RegionNames.BalanceRegion);

            RegionManager.SetRegionManager(this.StocksRegion, regionManager);
            RegionManager.SetRegionName(this.StocksRegion, RegionNames.StocksRegion);

            RegionManager.SetRegionManager(this.HistoryRegion, regionManager);
            RegionManager.SetRegionName(this.HistoryRegion, RegionNames.HistoryRegion);
        }

        public ShellPresenter Presenter { get; set; }
    }
}
