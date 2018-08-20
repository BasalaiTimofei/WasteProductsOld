using System.Web.Mvc;
using NLog;

namespace WasteProducts.Web.Controllers
{
    public abstract class BaseMvcController : Controller
    {
        protected ILogger Logger { get; }

        protected BaseMvcController(ILogger logger)
        {
            Logger = logger;
        }
    }
}