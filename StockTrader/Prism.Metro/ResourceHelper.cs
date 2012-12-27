using Windows.ApplicationModel.Resources;

namespace Microsoft.Practices.Prism {
    public static class ResourceHelper {
        private static readonly ResourceLoader resourceLoader;

        static ResourceHelper() {
            resourceLoader = new ResourceLoader("Prism.Metro/Resources");
        }

        public static string InvalidDelegateRerefenceTypeException {
            get { return resourceLoader.GetString("InvalidDelegateRerefenceTypeException"); }
        }

        public static string DelegateCommandDelegatesCannotBeNull {
            get { return resourceLoader.GetString("DelegateCommandDelegatesCannotBeNull"); }
        }

        public static string DelegateCommandInvalidGenericPayloadType {
            get { return resourceLoader.GetString("DelegateCommandInvalidGenericPayloadType"); }
        }

        public static string PropertySupport_NotMemberAccessExpression_Exception {
            get { return resourceLoader.GetString("PropertySupport_NotMemberAccessExpression_Exception"); }
        }

        public static string PropertySupport_ExpressionNotProperty_Exception {
            get { return resourceLoader.GetString("PropertySupport_ExpressionNotProperty_Exception"); }
        }

        public static string PropertySupport_StaticExpression_Exception {
            get { return resourceLoader.GetString("PropertySupport_StaticExpression_Exception"); }
        }

        public static string ModuleDependenciesNotMetInGroup {
            get { return resourceLoader.GetString("ModuleDependenciesNotMetInGroup"); }
        }

        public static string DuplicatedModule {
            get { return resourceLoader.GetString("DuplicatedModule"); }
        }

        public static string StartupModuleDependsOnAnOnDemandModule {
            get { return resourceLoader.GetString("StartupModuleDependsOnAnOnDemandModule"); }
        }

        public static string StringCannotBeNullOrEmpty {
            get { return resourceLoader.GetString("StringCannotBeNullOrEmpty"); }
        }

        public static string DependencyForUnknownModule {
            get { return resourceLoader.GetString("DependencyForUnknownModule"); }
        }

        public static string CyclicDependencyFound {
            get { return resourceLoader.GetString("CyclicDependencyFound"); }
        }

        public static string DependencyOnMissingModule {
            get { return resourceLoader.GetString("DependencyOnMissingModule"); }
        }

        public static string ValueMustBeOfTypeModuleInfo {
            get { return resourceLoader.GetString("ValueMustBeOfTypeModuleInfo"); }
        }

        public static string FailedToLoadModule {
            get { return resourceLoader.GetString("FailedToLoadModule"); }
        }

        public static string FailedToLoadModuleNoAssemblyInfo {
            get { return resourceLoader.GetString("FailedToLoadModuleNoAssemblyInfo"); }
        }

        public static string FailedToGetType {
            get { return resourceLoader.GetString("FailedToGetType"); }
        }

        public static string ModuleNotFound {
            get { return resourceLoader.GetString("ModuleNotFound"); }
        }

        public static string NoRetrieverCanRetrieveModule {
            get { return resourceLoader.GetString("NoRetrieverCanRetrieveModule"); }
        }

        public static string FailedToRetrieveModule {
            get { return resourceLoader.GetString("FailedToRetrieveModule"); }
        }

        public static string InvalidArgumentAssemblyUri {
            get { return resourceLoader.GetString("InvalidArgumentAssemblyUri"); }
        }

        public static string UpdateRegionException {
            get { return resourceLoader.GetString("UpdateRegionException"); }
        }

        public static string RegionNotInRegionManagerException {
            get { return resourceLoader.GetString("RegionNotInRegionManagerException"); }
        }

        public static string RegionNameCannotBeEmptyException {
            get { return resourceLoader.GetString("RegionNameCannotBeEmptyException"); }
        }

        public static string RegionNameExistsException {
            get { return resourceLoader.GetString("RegionNameExistsException"); }
        }

        public static string HostControlCannotBeSetAfterAttach {
            get { return resourceLoader.GetString("HostControlCannotBeSetAfterAttach"); }
        }

        public static string DeactiveNotPossibleException {
            get { return resourceLoader.GetString("DeactiveNotPossibleException"); }
        }

        public static string ContentControlHasContentException {
            get { return resourceLoader.GetString("ContentControlHasContentException"); }
        }

        public static string ItemsControlHasItemsSourceException {
            get { return resourceLoader.GetString("ItemsControlHasItemsSourceException"); }
        }

        public static string CannotChangeRegionNameException {
            get { return resourceLoader.GetString("CannotChangeRegionNameException"); }
        }

        public static string RegionViewExistsException {
            get { return resourceLoader.GetString("RegionViewExistsException"); }
        }

        public static string RegionViewNameExistsException {
            get { return resourceLoader.GetString("RegionViewNameExistsException"); }
        }

        public static string ViewNotInRegionException {
            get { return resourceLoader.GetString("ViewNotInRegionException"); }
        }

        public static string AdapterInvalidTypeException {
            get { return resourceLoader.GetString("AdapterInvalidTypeException"); }
        }

        public static string MappingExistsException {
            get { return resourceLoader.GetString("MappingExistsException"); }
        }

        public static string NoRegionAdapterException {
            get { return resourceLoader.GetString("NoRegionAdapterException"); }
        }

        public static string RegionBehaviorRegionCannotBeSetAfterAttach {
            get { return resourceLoader.GetString("RegionBehaviorRegionCannotBeSetAfterAttach"); }
        }

        public static string RegionBehaviorAttachCannotBeCallWithNullRegion {
            get { return resourceLoader.GetString("RegionBehaviorAttachCannotBeCallWithNullRegion"); }
        }

        public static string TypeWithKeyNotRegistered {
            get { return resourceLoader.GetString("TypeWithKeyNotRegistered"); }
        }

        public static string CanOnlyAddTypesThatInheritIFromRegionBehavior {
            get { return resourceLoader.GetString("CanOnlyAddTypesThatInheritIFromRegionBehavior"); }
        }

        public static string RegionNotFound {
            get { return resourceLoader.GetString("RegionNotFound"); }
        }

        public static string RegionManagerWithDifferentNameException {
            get { return resourceLoader.GetString("RegionManagerWithDifferentNameException"); }
        }

        public static string CannotCreateNavigationTarget {
            get { return resourceLoader.GetString("CannotCreateNavigationTarget"); }
        }

        public static string NavigationServiceHasNoRegion {
            get { return resourceLoader.GetString("NavigationServiceHasNoRegion"); }
        }

        public static string OnViewRegisteredException {
            get { return resourceLoader.GetString("OnViewRegisteredException"); }
        }

        public static string DefaultTextLoggerPattern {
            get { return resourceLoader.GetString("DefaultTextLoggerPattern"); }
        }

        public static string RegionCreationException {
            get { return resourceLoader.GetString("RegionCreationException"); }
        }
    }
}