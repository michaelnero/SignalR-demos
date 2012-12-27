using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;

namespace StockTrader.Web.Configuration {
    public class UnityResolver : DefaultDependencyResolver {
        private readonly IUnityContainer container;

        public UnityResolver(IUnityContainer container) {
            this.container = container;
        }

        public override object GetService(Type serviceType) {
            if (this.container.IsRegistered(serviceType)) {
                return this.container.Resolve(serviceType);
            }

            return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType) {
            if (this.container.IsRegistered(serviceType)) {
                return this.container.ResolveAll(serviceType);
            }

            return base.GetServices(serviceType);
        }
    }
}