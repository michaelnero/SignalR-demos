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
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Microsoft.Practices.Prism.Events {
    /// <summary>
    /// Wraps the Application Dispatcher.
    /// </summary>
    public class DefaultDispatcher : IDispatcherFacade {
        /// <summary>
        /// Forwards the BeginInvoke to the current application's <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="method">Method to be invoked.</param>
        /// <param name="arg">Arguments to pass to the invoked method.</param>
        public async void BeginInvoke(Delegate method, object arg) {
            if (CoreApplication.MainView.CoreWindow != null) {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => method.DynamicInvoke(arg));
            }
        }
    }
}