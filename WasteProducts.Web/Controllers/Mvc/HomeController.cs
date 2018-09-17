using System.Web.Mvc;

namespace WasteProducts.Web.Controllers.Mvc
{
    public class HomeController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return View();
        }
    }
}