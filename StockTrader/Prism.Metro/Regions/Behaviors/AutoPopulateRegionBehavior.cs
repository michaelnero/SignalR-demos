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
using System.Collections.Generic;

namespace Microsoft.Practices.Prism.Regions.Behaviors {
    /// <summary>
    /// Populates the target region with the views registered to it in the <see cref="IRegionViewRegistry"/>.
    /// </summary>
    public class AutoPopulateRegionBehavior : RegionBehavior {
        /// <summary>
        /// The key of this behavior.
        /// </summary>
        public const string BehaviorKey = "AutoPopulate";

        private readonly IRegionViewRegistry regionViewRegistry;

        /// <summary>
        /// Creates a new instance of the AutoPopulateRegionBehavior 
        /// associated with the <see cref="IRegionViewRegistry"/> received.
        /// </summary>
        /// <param name="regionViewRegistry"><see cref="IRegionViewRegistry"/> that the behavior will monitor for views to populate the region.</param>
        public AutoPopulateRegionBehavior(IRegionViewRegistry regionViewRegistry) {
            this.regionViewRegistry = regionViewRegistry;
        }

        /// <summary>
        /// Attaches the AutoPopulateRegionBehavior to the Region.
        /// </summary>
        protected override void OnAttach() {
            if (string.IsNullOrEmpty(this.Region.Name)) {
                this.Region.PropertyChanged += this.Region_PropertyChanged;
            }
            else {
                this.StartPopulatingContent();
            }
        }

        private void StartPopulatingContent() {
            foreach (object view in this.CreateViewsToAutoPopulate()) {
                AddViewIntoRegion(view);
            }

            this.regionViewRegistry.ContentRegistered += this.OnViewRegistered;
        }

        /// <summary>
        /// Returns a collection of views that will be added to the
        /// View collection. 
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<object> CreateViewsToAutoPopulate() {
            return this.regionViewRegistry.GetContents(this.Region.Name);
        }

        /// <summary>
        /// Adds a view into the views collection of this region. 
        /// </summary>
        /// <param name="viewToAdd"></param>
        protected virtual void AddViewIntoRegion(object viewToAdd) {
            this.Region.Add(viewToAdd);
        }

        private void Region_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "Name" && !string.IsNullOrEmpty(this.Region.Name)) {
                this.Region.PropertyChanged -= this.Region_PropertyChanged;
                this.StartPopulatingContent();
            }
        }

        /// <summary>
        /// Handler of the event that fires when a new viewtype is registered to the registry. 
        /// </summary>
        /// <remarks>Although this is a public method to support Weak Delegates in Silverlight, it should not be called by the user.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification = "This has to be public in order to work with weak references in partial trust or Silverlight environments.")]
        public virtual void OnViewRegistered(object sender, ViewRegisteredEventArgs e) {
            if (e == null) throw new System.ArgumentNullException("e");

            if (e.RegionName == this.Region.Name) {
                AddViewIntoRegion(e.GetView());
            }
        }
    }
}