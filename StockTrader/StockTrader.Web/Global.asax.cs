using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using StockTrader.Web.Configuration;

namespace StockTrader.Web {
    public class MvcApplication : HttpApplication {
        protected void Application_Start() {
            var container = new UnityContainer();
            ContainerRegistrar.DoRegistration(typeof(MvcApplication).Assembly, container);

            GlobalHost.DependencyResolver = new UnityResolver(container);

            RouteTable.Routes.MapHubs();
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}