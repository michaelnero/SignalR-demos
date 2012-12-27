using Microsoft.Practices.Prism.Regions;
using StockTrader.Windows.Common;
using StockTrader.Windows.Common.Configuration;

namespace StockTrader.Windows.Views.Shell {
    [RegisterInContainer(typeof(IShellView))]
    public sealed partial class ShellView : IShellView {
        public ShellView(IRegionManager regionManager) {
            this.InitializeComponent();

            RegionManager.SetRegionManager(this.MainRegion, regionManager);
            RegionManager.SetRegionName(this.MainRegion, RegionNames.MainRegion);
        }

        public ShellPresenter Presenter { get; set; }
    }
}