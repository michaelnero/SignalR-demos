using System;

namespace StockTrader.Web.Configuration {
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class RegisterInContainerAttribute : Attribute {
        public RegisterInContainerAttribute(Type interfaceType)
            : this(interfaceType, RegistrationType.Transient) {
        }

        public RegisterInContainerAttribute(Type interfaceType, RegistrationType registrationType)
            : this(interfaceType, null, registrationType) {
        }

        public RegisterInContainerAttribute(Type interfaceType, string name, RegistrationType registrationType) {
            if (interfaceType == null) {
                throw new ArgumentNullException("interfaceType");
            }

            this.InterfaceType = interfaceType;
            this.Name = name;
            this.RegistrationType = registrationType;
        }

        public Type InterfaceType { get; private set; }
        
        public string Name { get; private set; }
        
        public RegistrationType RegistrationType { get; private set; }
    }
}