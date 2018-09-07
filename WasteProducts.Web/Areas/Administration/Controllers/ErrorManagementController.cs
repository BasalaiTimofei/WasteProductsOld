using System.Web.Mvc;
using WasteProducts.Web.Utils.ActionResults;

namespace WasteProducts.Web.Areas.Administration.Controllers
{
    /// <summary>
    /// Contoller for Elmah errors page
    /// </summary>
    public class ErrorManagementController : Controller
    {
        /// <summary>
        /// List page
        /// </summary>
        /// <returns>ElmahResult</returns>
        public ActionResult Index(string resource)
        {
            return new ElmahResult();
        }

        /// <summary>
        /// Error details page
        /// </summary>
        /// <returns>ElmahResult</returns>
        public ActionResult Detail(string resource)
        {
            return new ElmahResult();
        }
    }
}