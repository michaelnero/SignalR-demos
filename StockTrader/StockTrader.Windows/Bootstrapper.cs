using Microsoft.Practices.Prism.Modularity;
using Prism.Extensions.Unity;
using StockTrader.Windows.Broker;
using StockTrader.Windows.Common.Configuration;
using StockTrader.Windows.StartUp;
using StockTrader.Windows.Trader;
using StockTrader.Windows.Views.Shell;
using Windows.UI.Xaml;
using Microsoft.Practices.Unity;
using System.Reflection;

namespace StockTrader.Windows {
    internal class Bootstrapper : UnityBootstrapper {
        protected override DependencyObject CreateShell() {
            var shell = this.Container.Resolve<ShellPresenter>();

            Window.Current.Content = (UIElement) shell.View;
            Window.Current.Activate();

            return (DependencyObject) shell.View;
        }

        protected override void ConfigureContainer() {
            base.ConfigureContainer();

            var assembly = typeof (Bootstrapper).GetTypeInfo().Assembly;
            ContainerRegistrar.DoRegistration(assembly, this.Container);
        }

        protected override void ConfigureModuleCatalog() {
            base.ConfigureModuleCatalog();

            var brokerModule = typeof (BrokerModule);
            this.ModuleCatalog.AddModule(new ModuleInfo(brokerModule.Name, brokerModule.AssemblyQualifiedName));

            var startUpModule = typeof (StartUpModule);
            this.ModuleCatalog.AddModule(new ModuleInfo(startUpModule.Name, startUpModule.AssemblyQualifiedName));

            var traderModule = typeof (TraderModule);
            this.ModuleCatalog.AddModule(new ModuleInfo(traderModule.Name, traderModule.AssemblyQualifiedName));
        }
    }
}