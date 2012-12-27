using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Prism.Extensions.Unity {
    /// <summary>
    /// Defines a <seealso cref="IUnityContainer"/> adapter for the <see cref="IServiceLocator"/> interface to be used by the Composite Application Library.
    /// </summary>
    public sealed class UnityServiceLocatorAdapter : ServiceLocatorImplBase {
        private readonly IUnityContainer unityContainer;

        /// <summary>
        /// Initializes a new instance of <see cref="UnityServiceLocatorAdapter"/>.
        /// </summary>
        /// <param name="unityContainer">The <seealso cref="IUnityContainer"/> that will be used
        /// by the <see cref="DoGetInstance"/> and <see cref="DoGetAllInstances"/> methods.</param>
        public UnityServiceLocatorAdapter(IUnityContainer unityContainer) {
            this.unityContainer = unityContainer;
        }

        /// <summary>
        /// Resolves the instance of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>The requested service instance.</returns>
        protected override object DoGetInstance(Type serviceType, string key) {
            return this.unityContainer.Resolve(serviceType, key);
        }

        /// <summary>
        /// Resolves all the instances of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Sequence of service instance objects.</returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType) {
            return this.unityContainer.ResolveAll(serviceType);
        }
    }
}