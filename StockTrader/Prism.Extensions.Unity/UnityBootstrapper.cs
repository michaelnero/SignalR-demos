using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Globalization;

namespace Prism.Extensions.Unity {
    /// <summary>
    /// Base class that provides a basic bootstrapping sequence that
    /// registers most of the Composite Application Library assets
    /// in a <see cref="IUnityContainer"/>.
    /// </summary>
    /// <remarks>
    /// This class must be overriden to provide application specific configuration.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public abstract class UnityBootstrapper : Bootstrapper {
        private bool useDefaultConfiguration = true;

        /// <summary>
        /// Gets the default <see cref="IUnityContainer"/> for the application.
        /// </summary>
        /// <value>The default <see cref="IUnityContainer"/> instance.</value>
        public IUnityContainer Container { get; protected set; }


        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true"/>, registers default Composite Application Library services in the container. This is the default behavior.</param>
        public override void Run(bool runWithDefaultConfiguration) {
            this.useDefaultConfiguration = runWithDefaultConfiguration;

            this.Logger = this.CreateLogger();
            if (this.Logger == null) {
                throw new InvalidOperationException(ResourceHelper.NullLoggerFacadeException);
            }

            this.Logger.Log(ResourceHelper.LoggerCreatedSuccessfully, Category.Debug, Priority.Low);

            this.Logger.Log(ResourceHelper.CreatingModuleCatalog, Category.Debug, Priority.Low);
            this.ModuleCatalog = this.CreateModuleCatalog();
            if (this.ModuleCatalog == null) {
                throw new InvalidOperationException(ResourceHelper.NullModuleCatalogException);
            }

            this.Logger.Log(ResourceHelper.ConfiguringModuleCatalog, Category.Debug, Priority.Low);
            this.ConfigureModuleCatalog();

            this.Logger.Log(ResourceHelper.CreatingUnityContainer, Category.Debug, Priority.Low);
            this.Container = this.CreateContainer();
            if (this.Container == null) {
                throw new InvalidOperationException(ResourceHelper.NullUnityContainerException);
            }

            this.Logger.Log(ResourceHelper.ConfiguringUnityContainer, Category.Debug, Priority.Low);
            this.ConfigureContainer();

            this.Logger.Log(ResourceHelper.ConfiguringServiceLocatorSingleton, Category.Debug, Priority.Low);
            this.ConfigureServiceLocator();

            this.Logger.Log(ResourceHelper.ConfiguringRegionAdapters, Category.Debug, Priority.Low);
            this.ConfigureRegionAdapterMappings();

            this.Logger.Log(ResourceHelper.ConfiguringDefaultRegionBehaviors, Category.Debug, Priority.Low);
            this.ConfigureDefaultRegionBehaviors();

            this.Logger.Log(ResourceHelper.RegisteringFrameworkExceptionTypes, Category.Debug, Priority.Low);
            this.RegisterFrameworkExceptionTypes();

            this.Logger.Log(ResourceHelper.CreatingShell, Category.Debug, Priority.Low);
            this.Shell = this.CreateShell();
            if (this.Shell != null) {
                this.Logger.Log(ResourceHelper.SettingTheRegionManager, Category.Debug, Priority.Low);
                RegionManager.SetRegionManager(this.Shell, this.Container.Resolve<IRegionManager>());

                this.Logger.Log(ResourceHelper.UpdatingRegions, Category.Debug, Priority.Low);
                RegionManager.UpdateRegions();

                this.Logger.Log(ResourceHelper.InitializingShell, Category.Debug, Priority.Low);
                this.InitializeShell();
            }

            if (this.Container.IsRegistered<IModuleManager>()) {
                this.Logger.Log(ResourceHelper.InitializingModules, Category.Debug, Priority.Low);
                this.InitializeModules();
            }

            this.Logger.Log(ResourceHelper.BootstrapperSequenceCompleted, Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="ServiceLocator" />.
        /// </summary>
        protected override void ConfigureServiceLocator() {
            ServiceLocator.SetLocatorProvider(() => this.Container.Resolve<IServiceLocator>());
        }

        /// <summary>
        /// Registers in the <see cref="IUnityContainer"/> the <see cref="Type"/> of the Exceptions
        /// that are not considered root exceptions by the <see cref="ExceptionExtensions"/>.
        /// </summary>
        protected override void RegisterFrameworkExceptionTypes() {
            base.RegisterFrameworkExceptionTypes();

            ExceptionExtensions.RegisterFrameworkExceptionType(typeof (ResolutionFailedException));
        }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer() {
            this.Logger.Log(ResourceHelper.AddingUnityBootstrapperExtensionToContainer, Category.Debug, Priority.Low);
            this.Container.AddNewExtension<UnityBootstrapperExtension>();

            Container.RegisterInstance(Logger);

            this.Container.RegisterInstance(this.ModuleCatalog);

            if (useDefaultConfiguration) {
                RegisterTypeIfMissing(typeof (IServiceLocator), typeof (UnityServiceLocatorAdapter), true);
                RegisterTypeIfMissing(typeof (IModuleInitializer), typeof (ModuleInitializer), true);
                RegisterTypeIfMissing(typeof (IModuleManager), typeof (ModuleManager), true);
                RegisterTypeIfMissing(typeof (RegionAdapterMappings), typeof (RegionAdapterMappings), true);
                RegisterTypeIfMissing(typeof (IRegionManager), typeof (RegionManager), true);
                RegisterTypeIfMissing(typeof (IEventAggregator), typeof (EventAggregator), true);
                RegisterTypeIfMissing(typeof (IRegionViewRegistry), typeof (RegionViewRegistry), true);
                RegisterTypeIfMissing(typeof (IRegionBehaviorFactory), typeof (RegionBehaviorFactory), true);
                RegisterTypeIfMissing(typeof (IRegionNavigationJournalEntry), typeof (RegionNavigationJournalEntry), false);
                RegisterTypeIfMissing(typeof (IRegionNavigationJournal), typeof (RegionNavigationJournal), false);
                RegisterTypeIfMissing(typeof (IRegionNavigationService), typeof (RegionNavigationService), false);
                RegisterTypeIfMissing(typeof (IRegionNavigationContentLoader), typeof (RegionNavigationContentLoader), true);
            }
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog
        /// </summary>
        protected override void InitializeModules() {
            IModuleManager manager;

            try {
                manager = this.Container.Resolve<IModuleManager>();
            }
            catch (ResolutionFailedException ex) {
                if (ex.Message.Contains("IModuleCatalog")) {
                    throw new InvalidOperationException(ResourceHelper.NullModuleCatalogException);
                }

                throw;
            }

            manager.Run();
        }

        /// <summary>
        /// Creates the <see cref="IUnityContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="IUnityContainer"/>.</returns>
        protected virtual IUnityContainer CreateContainer() {
            return new UnityContainer();
        }

        /// <summary>
        /// Registers a type in the container only if that type was not already registered.
        /// </summary>
        /// <param name="fromType">The interface type to register.</param>
        /// <param name="toType">The type implementing the interface.</param>
        /// <param name="registerAsSingleton">Registers the type as a singleton.</param>
        protected void RegisterTypeIfMissing(Type fromType, Type toType, bool registerAsSingleton) {
            if (fromType == null) {
                throw new ArgumentNullException("fromType");
            }
            if (toType == null) {
                throw new ArgumentNullException("toType");
            }
            if (Container.IsTypeRegistered(fromType)) {
                Logger.Log(String.Format(CultureInfo.CurrentCulture, ResourceHelper.TypeMappingAlreadyRegistered, fromType.Name), Category.Debug, Priority.Low);
            }
            else {
                if (registerAsSingleton) {
                    Container.RegisterType(fromType, toType, new ContainerControlledLifetimeManager());
                }
                else {
                    Container.RegisterType(fromType, toType);
                }
            }
        }
    }
}