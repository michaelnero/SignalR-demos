using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Prism.Extensions.Unity
{
    public static class ResourceHelper
    {
        private static ResourceLoader _resourceLoader;
        static ResourceHelper()
        {
            _resourceLoader = new ResourceLoader("Prism.Extensions.Unity/Resources");
        }

        public static string NullLoggerFacadeException
        {
            get
            {
                return _resourceLoader.GetString("NullLoggerFacadeException");
            }
        }

        public static string LoggerCreatedSuccessfully
        {
            get
            {
                return _resourceLoader.GetString("LoggerCreatedSuccessfully");
            }
        }

        public static string CreatingModuleCatalog
        {
            get
            {
                return _resourceLoader.GetString("CreatingModuleCatalog");
            }
        }

        public static string NullModuleCatalogException
        {
            get
            {
                return _resourceLoader.GetString("NullModuleCatalogException");
            }
        }

        public static string ConfiguringModuleCatalog
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringModuleCatalog");
            }
        }

        public static string CreatingUnityContainer
        {
            get
            {
                return _resourceLoader.GetString("CreatingUnityContainer");
            }
        }

        public static string NullUnityContainerException
        {
            get
            {
                return _resourceLoader.GetString("NullUnityContainerException");
            }
        }
        public static string ConfiguringUnityContainer
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringUnityContainer");
            }
        }
        public static string ConfiguringServiceLocatorSingleton
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringServiceLocatorSingleton");
            }
        }

        public static string ConfiguringRegionAdapters
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringRegionAdapters");
            }
        }
        public static string ConfiguringDefaultRegionBehaviors
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringDefaultRegionBehaviors");
            }
        }
        public static string RegisteringFrameworkExceptionTypes
        {
            get
            {
                return _resourceLoader.GetString("RegisteringFrameworkExceptionTypes");
            }
        }

        public static string AddingUnityBootstrapperExtensionToContainer
        {
            get
            {
                return _resourceLoader.GetString("AddingUnityBootstrapperExtensionToContainer");
            }
        }

        public static string CreatingShell
        {
            get
            {
                return _resourceLoader.GetString("CreatingShell");
            }
        }

        public static string SettingTheRegionManager
        {
            get
            {
                return _resourceLoader.GetString("SettingTheRegionManager");
            }
        }

        public static string UpdatingRegions
        {
            get
            {
                return _resourceLoader.GetString("UpdatingRegions");
            }
        }

        public static string InitializingShell
        {
            get
            {
                return _resourceLoader.GetString("InitializingShell");
            }
        }

        public static string InitializingModules
        {
            get
            {
                return _resourceLoader.GetString("InitializingModules");
            }
        }

        public static string BootstrapperSequenceCompleted
        {
            get
            {
                return _resourceLoader.GetString("BootstrapperSequenceCompleted");
            }
        }


        
        internal static string TypeMappingAlreadyRegistered
        {
            get
            {
                return _resourceLoader.GetString("TypeMappingAlreadyRegistered");
            }
        }
    }
}
