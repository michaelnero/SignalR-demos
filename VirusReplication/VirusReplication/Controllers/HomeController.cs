using System.Web.Mvc;

namespace VirusReplication.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return this.View();
        }
    }
}