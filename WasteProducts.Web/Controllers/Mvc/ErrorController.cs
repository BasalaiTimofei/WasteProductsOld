using System.Web.Mvc;
using WasteProducts.Web.Utils.ActionResults;

namespace WasteProducts.Web.Controllers.Mvc
{
    /// <summary>
    /// Mvc errors controller 
    /// </summary>
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        /// <summary>
        /// Action for Elmah errors list page
        /// </summary>
        /// <returns>ElmahResult</returns>
        public ActionResult Index(string resource)
        {
            return new ElmahResult();
        }

        /// <summary>
        /// Action for Elmah error details page
        /// </summary>
        /// <returns>ElmahResult</returns>
        public ActionResult Detail(string resource)
        {
            return new ElmahResult();
        }

        /// <summary>
        /// Action for InternalServerError 
        /// </summary>
        /// <returns>error page</returns>
        public ActionResult InternalServerError()
        {
            return View("500-InternalServerError");
        }

        /// <summary>
        /// Action for ForbiddenError 
        /// </summary>
        /// <returns>error page</returns>
        public ActionResult ForbiddenError(string id)
        {
            return View("403-ForbiddenError");
        }

        /// <summary>
        /// Action for NotFoundError 
        /// </summary>
        /// <returns>error page</returns>
        public ActionResult NotFoundError(string id)
        {
            return View("404-NotFoundError");
        }
    }
}