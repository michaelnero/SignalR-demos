using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace StockTrader.Windows.Common.Configuration {
    public static class ContainerRegistrar {
        public static void DoRegistration(Assembly assembly, IUnityContainer container) {
            var types = GetTypesFrom(assembly);
            foreach (var type in types) {
                var attributes = type.GetTypeInfo().GetCustomAttributes<RegisterInContainerAttribute>(true);
                foreach (var attribute in attributes) {
                    container.RegisterType(attribute.InterfaceType, type, attribute.Name, GetLifetimeManager(attribute.RegistrationType));
                }
            }
        }

        private static IEnumerable<Type> GetTypesFrom(Assembly assembly) {
            IEnumerable<Type> types;

            try {
                types = assembly.ExportedTypes.ToList();
            }
            catch (ReflectionTypeLoadException exception) {
                types = exception.Types;
            }

            return types;
        }

        private static LifetimeManager GetLifetimeManager(RegistrationType registrationType) {
            if (RegistrationType.Singleton == registrationType) {
                return new ContainerControlledLifetimeManager();
            }

            return new TransientLifetimeManager();
        }
    }
}