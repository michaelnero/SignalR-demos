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
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Microsoft.Practices.Prism.Commands {
    /// <summary>
    /// Handles management and dispatching of EventHandlers in a weak way.
    /// </summary>
    internal static class WeakEventHandlerManager {
        ///<summary>
        /// Invokes the handlers 
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="handlers"></param>
        public static void CallWeakReferenceHandlers(object sender, List<WeakReference> handlers) {
            if (handlers != null) {
                // Take a snapshot of the handlers before we call out to them since the handlers
                // could cause the array to me modified while we are reading it.
                var callees = new EventHandler[handlers.Count];
                int count = 0;

                //Clean up handlers
                count = CleanupOldHandlers(handlers, callees, count);

                // Call the handlers that we snapshotted
                for (int i = 0; i < count; i++) {
                    CallHandler(sender, callees[i]);
                }
            }
        }

        private static void CallHandler(object sender, EventHandler eventHandler) {
            DispatcherProxy dispatcher = DispatcherProxy.CreateDispatcher();

            if (eventHandler != null) {
                if (dispatcher != null && !dispatcher.CheckAccess()) {
                    dispatcher.Invoke((Action<object, EventHandler>) CallHandler, sender, eventHandler);
                }
                else {
                    eventHandler(sender, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Hides the dispatcher mis-match between Silverlight and .Net, largely so code reads a bit easier
        /// </summary>
        private class DispatcherProxy {
            private readonly CoreDispatcher innerDispatcher;

            private DispatcherProxy(CoreDispatcher dispatcher) {

                innerDispatcher = dispatcher;
            }

            public static DispatcherProxy CreateDispatcher() {
                if (CoreApplication.MainView.CoreWindow == null)
                    return null;

                var proxy = new DispatcherProxy(CoreApplication.MainView.CoreWindow.Dispatcher);
                return proxy;
            }

            public bool CheckAccess() {
                return innerDispatcher.HasThreadAccess;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Threading.Dispatcher.#BeginInvoke(System.Delegate,System.Windows.Threading.DispatcherPriority,System.Object[])")]
            public async void Invoke(Delegate method, params Object[] args) {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => method.DynamicInvoke(args));
            }
        }

        private static int CleanupOldHandlers(List<WeakReference> handlers, EventHandler[] callees, int count) {
            for (int i = handlers.Count - 1; i >= 0; i--) {
                WeakReference reference = handlers[i];
                var handler = reference.Target as EventHandler;
                if (handler == null) {
                    // Clean up old handlers that have been collected
                    handlers.RemoveAt(i);
                }
                else {
                    callees[count] = handler;
                    count++;
                }
            }
            return count;
        }

        ///<summary>
        /// Adds a handler to the supplied list in a weak way.
        ///</summary>
        ///<param name="handlers">Existing handler list.  It will be created if null.</param>
        ///<param name="handler">Handler to add.</param>
        ///<param name="defaultListSize">Default list size.</param>
        public static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize) {
            if (handlers == null) {
                handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
            }

            handlers.Add(new WeakReference(handler));
        }

        ///<summary>
        /// Removes an event handler from the reference list.
        ///</summary>
        ///<param name="handlers">Handler list to remove reference from.</param>
        ///<param name="handler">Handler to remove.</param>
        public static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler) {
            if (handlers != null) {
                for (int i = handlers.Count - 1; i >= 0; i--) {
                    WeakReference reference = handlers[i];
                    var existingHandler = reference.Target as EventHandler;
                    if ((existingHandler == null) || (existingHandler == handler)) {
                        // Clean up old handlers that have been collected
                        // in addition to the handler that is to be removed.
                        handlers.RemoveAt(i);
                    }
                }
            }
        }
    }
}