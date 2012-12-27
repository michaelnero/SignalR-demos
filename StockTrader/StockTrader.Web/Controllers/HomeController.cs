using System.Web.Mvc;

namespace StockTrader.Web.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}