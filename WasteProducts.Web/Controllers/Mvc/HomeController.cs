using System.Web.Mvc;

namespace WasteProducts.Web.Controllers.Mvc
{
    /// <summary>
    /// Mvc Home controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Index action
        /// </summary>
        /// <returns>index view page</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}