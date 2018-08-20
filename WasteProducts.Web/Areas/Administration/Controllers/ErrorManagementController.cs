using System.Net;
using System.Web.Mvc;
using NLog;
using WasteProducts.Web.Controllers;
using WasteProducts.Web.Utils.ActionResults;

namespace WasteProducts.Web.Areas.Administration.Controllers
{

    public class ErrorManagementController : Controller
    {
        public ActionResult Index(string resource)
        {
            return new ElmahResult();
        }

        public ActionResult Detail(string resource)
        {
            return new ElmahResult();
        }
    }
}