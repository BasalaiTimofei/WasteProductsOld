using System.Web.Mvc;
using Ninject;
using NLog;

namespace WasteProducts.Web.Controllers.Mvc
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