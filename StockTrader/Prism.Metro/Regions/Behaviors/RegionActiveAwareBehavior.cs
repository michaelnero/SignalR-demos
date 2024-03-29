//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;

namespace Microsoft.Practices.Prism.Regions.Behaviors {
    /// <summary>
    /// Behavior that monitors a <see cref="IRegion"/> object and 
    /// changes the value for the <see cref="IActiveAware.IsActive"/> property when
    /// an object that implements <see cref="IActiveAware"/> gets added or removed 
    /// from the collection.
    /// </summary>
    /// <remarks>
    /// This class can also sync the active state for any scoped regions directly on the view based on the <see cref="SyncActiveStateAttribute"/>.
    /// If you use the <see cref="Microsoft.Practices.Prism.Regions.Region.Add(object,string,bool)" /> method with the createRegionManagerScope option, the scoped manager will be attached to the view.
    /// </remarks>
    public class RegionActiveAwareBehavior : IRegionBehavior {
        /// <summary>
        /// Name that identifies the <see cref="RegionActiveAwareBehavior"/> behavior in a collection of <see cref="IRegionBehavior"/>.
        /// </summary>
        public const string BehaviorKey = "ActiveAware";

        /// <summary>
        /// The region that this behavior is extending
        /// </summary>
        public IRegion Region { get; set; }

        /// <summary>
        /// Attaches the behavior to the specified region
        /// </summary>
        public void Attach() {
            INotifyCollectionChanged collection = this.GetCollection();
            if (collection != null) {
                collection.CollectionChanged += OnCollectionChanged;
            }
        }

        /// <summary>
        /// Detaches the behavior from the <see cref="INotifyCollectionChanged"/>.
        /// </summary>
        public void Detach() {
            INotifyCollectionChanged collection = this.GetCollection();
            if (collection != null) {
                collection.CollectionChanged -= OnCollectionChanged;
            }
        }

        private static void InvokeOnActiveAwareElement(object item, Action<IActiveAware> invocation) {
            var activeAware = item as IActiveAware;
            if (activeAware != null) {
                invocation(activeAware);
            }

            var frameworkElement = item as FrameworkElement;
            if (frameworkElement != null) {
                var activeAwareDataContext = frameworkElement.DataContext as IActiveAware;
                if (activeAwareDataContext != null) {
                    invocation(activeAwareDataContext);
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                foreach (object item in e.NewItems) {
                    Action<IActiveAware> invocation = activeAware => activeAware.IsActive = true;

                    InvokeOnActiveAwareElement(item, invocation);
                    InvokeOnSynchronizedActiveAwareChildren(item, invocation);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove) {
                foreach (object item in e.OldItems) {
                    Action<IActiveAware> invocation = activeAware => activeAware.IsActive = false;

                    InvokeOnActiveAwareElement(item, invocation);
                    InvokeOnSynchronizedActiveAwareChildren(item, invocation);
                }
            }

            // May need to handle other action values (reset, replace). Currently the ViewsCollection class does not raise CollectionChanged with these values.
        }

        private void InvokeOnSynchronizedActiveAwareChildren(object item, Action<IActiveAware> invocation) {
            var dependencyObjectView = item as DependencyObject;

            if (dependencyObjectView != null) {
                // We are assuming that any scoped region managers are attached directly to the 
                // view.
                var regionManager = RegionManager.GetRegionManager(dependencyObjectView);

                // If the view's RegionManager attached property is different from the region's RegionManager,
                // then the view's region manager is a scoped region manager.
                if (regionManager == null || regionManager == this.Region.RegionManager) return;

                var activeViews = regionManager.Regions.SelectMany(e => e.ActiveViews);

                var syncActiveViews = activeViews.Where(ShouldSyncActiveState);

                foreach (var syncActiveView in syncActiveViews) {
                    InvokeOnActiveAwareElement(syncActiveView, invocation);
                }
            }
        }

        private bool ShouldSyncActiveState(object view) {


            if (view.GetType().GetTypeInfo().GetCustomAttribute<SyncActiveStateAttribute>() != null) {
                return true;
            }

            var viewAsFrameworkElement = view as FrameworkElement;

            if (viewAsFrameworkElement != null) {
                var viewModel = viewAsFrameworkElement.DataContext;

                return viewModel != null && (view.GetType().GetTypeInfo().GetCustomAttribute<SyncActiveStateAttribute>() != null);
            }

            return false;
        }

        private INotifyCollectionChanged GetCollection() {
            return this.Region.ActiveViews;
        }
    }
}