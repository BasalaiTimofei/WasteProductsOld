using System.Net;
using System.Web.Mvc;

namespace WasteProducts.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult InternalServerError(string id) => GenericError("500-InternalServerError", HttpStatusCode.InternalServerError, id);

        public ActionResult BadGateway(string id) => GenericError("502-BadGateway", HttpStatusCode.BadGateway, id);

        public ActionResult ServiceUnavailable(string id) => GenericError("503-ServiceUnavailable", HttpStatusCode.ServiceUnavailable, id);

        public ActionResult GatewayTimeout(string id) => GenericError("504-GatewayTimeout", HttpStatusCode.GatewayTimeout, id);

        public ActionResult Forbidden(string id) => GenericError("403-Forbidden", HttpStatusCode.Forbidden, id);

        public ActionResult NotFound(string id) => GenericError("404-NotFound", HttpStatusCode.NotFound, id);

        private ActionResult GenericError(string viewName, HttpStatusCode statusCode, string errorId)
        {
            if (User.IsInRole("Admin") || HttpContext.Request.IsLocal || HttpContext.IsDebuggingEnabled)
                ViewData.Model = errorId;

            Response.StatusCode = (int)statusCode;
            return View(viewName);
        }
    }
}