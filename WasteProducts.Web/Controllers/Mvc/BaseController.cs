using System.Web.Mvc;

namespace WasteProducts.Web.Controllers.Mvc
{
    internal abstract class BaseController : Controller
    {
        // GET: Base
        public ActionResult Index()
        {
            return View();
        }
    }
}