using System.Web.Http;
using System.Web.Mvc;
using Ninject;
using NLog;

namespace WasteProducts.Web.Controllers
{
    public class BaseMvcController : Controller
    {

        [Inject]
        protected ILogger Logger { get; set; }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            Logger.Error(filterContext.Exception);

            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Index", "ErrorHandler");
            // OR 
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/ErrorHandler/Index.cshtml"
            };
        }
    }

    
}