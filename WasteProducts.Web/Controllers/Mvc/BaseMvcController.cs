using System.Web.Mvc;
using NLog;

namespace WasteProducts.Web.Controllers.Mvc
{
    public abstract class BaseMvcController : Controller
    {
        protected BaseMvcController(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }
    }
}