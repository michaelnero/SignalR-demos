﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.ServiceLocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Practices.Prism {
    /// <summary>
    /// Base class that provides a basic bootstrapping sequence and hooks
    /// that specific implementations can override
    /// </summary>
    /// <remarks>
    /// This class must be overriden to provide application specific configuration.
    /// </remarks>
    public abstract class Bootstrapper {
        /// <summary>
        /// Gets the <see cref="ILoggerFacade"/> for the application.
        /// </summary>
        /// <value>A <see cref="ILoggerFacade"/> instance.</value>
        protected ILoggerFacade Logger { get; set; }

        /// <summary>
        /// Gets the default <see cref="IModuleCatalog"/> for the application.
        /// </summary>
        /// <value>The default <see cref="IModuleCatalog"/> instance.</value>
        protected IModuleCatalog ModuleCatalog { get; set; }

        /// <summary>
        /// Gets the shell user interface
        /// </summary>
        /// <value>The shell user interface.</value>
        protected DependencyObject Shell { get; set; }

        /// <summary>
        /// Create the <see cref="ILoggerFacade" /> used by the bootstrapper.
        /// </summary>
        /// <remarks>
        /// The base implementation returns a new TextLogger.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "The Logger is added to the container which will dispose it when the container goes out of scope.")]
        protected virtual ILoggerFacade CreateLogger() {
            return new TextLogger();
        }

        /// <summary>
        /// Runs the bootstrapper process.
        /// </summary>
        public void Run() {
            this.Run(true);
        }

        /// <summary>
        /// Creates the <see cref="IModuleCatalog"/> used by Prism.
        /// </summary>
        ///  <remarks>
        /// The base implementation returns a new ModuleCatalog.
        /// </remarks>
        protected virtual IModuleCatalog CreateModuleCatalog() {
            return new ModuleCatalog();
        }

        /// <summary>
        /// Configures the <see cref="IModuleCatalog"/> used by Prism.
        /// </summary>
        protected virtual void ConfigureModuleCatalog() {
        }

        /// <summary>
        /// Registers the <see cref="Type"/>s of the Exceptions that are not considered 
        /// root exceptions by the <see cref="ExceptionExtensions"/>.
        /// </summary>
        protected virtual void RegisterFrameworkExceptionTypes() {
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof (ActivationException));
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog
        /// </summary>
        protected virtual void InitializeModules() {
            var manager = ServiceLocator.Current.GetInstance<IModuleManager>();
            manager.Run();
        }

        /// <summary>
        /// Configures the default region adapter mappings to use in the application, in order
        /// to adapt UI controls defined in XAML to use a region and register it automatically.
        /// May be overwritten in a derived class to add specific mappings required by the application.
        /// </summary>
        /// <returns>The <see cref="RegionAdapterMappings"/> instance containing all the mappings.</returns>
        protected virtual RegionAdapterMappings ConfigureRegionAdapterMappings() {
            var regionAdapterMappings = ServiceLocator.Current.GetInstance<RegionAdapterMappings>();
            if (regionAdapterMappings != null) {
                regionAdapterMappings.RegisterMapping(typeof (Selector), ServiceLocator.Current.GetInstance<SelectorRegionAdapter>());
                regionAdapterMappings.RegisterMapping(typeof (ItemsControl), ServiceLocator.Current.GetInstance<ItemsControlRegionAdapter>());
                regionAdapterMappings.RegisterMapping(typeof (ContentControl), ServiceLocator.Current.GetInstance<ContentControlRegionAdapter>());
            }

            return regionAdapterMappings;
        }

        /// <summary>
        /// Configures the <see cref="IRegionBehaviorFactory"/>. 
        /// This will be the list of default behaviors that will be added to a region. 
        /// </summary>
        protected virtual IRegionBehaviorFactory ConfigureDefaultRegionBehaviors() {
            var defaultRegionBehaviorTypesDictionary = ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>();

            if (defaultRegionBehaviorTypesDictionary != null) {
                defaultRegionBehaviorTypesDictionary.AddIfMissing(AutoPopulateRegionBehavior.BehaviorKey, typeof (AutoPopulateRegionBehavior));
                defaultRegionBehaviorTypesDictionary.AddIfMissing(BindRegionContextToDependencyObjectBehavior.BehaviorKey, typeof (BindRegionContextToDependencyObjectBehavior));
                defaultRegionBehaviorTypesDictionary.AddIfMissing(RegionActiveAwareBehavior.BehaviorKey, typeof (RegionActiveAwareBehavior));
                defaultRegionBehaviorTypesDictionary.AddIfMissing(SyncRegionContextWithHostBehavior.BehaviorKey, typeof (SyncRegionContextWithHostBehavior));
                defaultRegionBehaviorTypesDictionary.AddIfMissing(RegionManagerRegistrationBehavior.BehaviorKey, typeof (RegionManagerRegistrationBehavior));
                defaultRegionBehaviorTypesDictionary.AddIfMissing(RegionMemberLifetimeBehavior.BehaviorKey, typeof (RegionMemberLifetimeBehavior));
                defaultRegionBehaviorTypesDictionary.AddIfMissing(ClearChildViewsRegionBehavior.BehaviorKey, typeof (ClearChildViewsRegionBehavior));
            }

            return defaultRegionBehaviorTypesDictionary;
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        protected virtual void InitializeShell() {
        }

        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true"/>, registers default 
        /// Composite Application Library services in the container. This is the default behavior.</param>
        public abstract void Run(bool runWithDefaultConfiguration);

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        /// <remarks>
        /// If the returned instance is a <see cref="DependencyObject"/>, the
        /// <see cref="Bootstrapper"/> will attach the default <seealso cref="IRegionManager"/> of
        /// the application in its <see cref="RegionManager.RegionManagerProperty"/> attached property
        /// in order to be able to add regions by using the <seealso cref="RegionManager.RegionNameProperty"/>
        /// attached property from XAML.
        /// </remarks>
        protected abstract DependencyObject CreateShell();

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="Microsoft.Practices.ServiceLocation.ServiceLocator" />.
        /// </summary>
        protected abstract void ConfigureServiceLocator();
    }
}