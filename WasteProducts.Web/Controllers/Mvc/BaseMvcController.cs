using System.Web.Mvc;
using NLog;

namespace WasteProducts.Web.Controllers.Mvc
{
    /// <summary>
    /// Abstract class-parent for all non-API controllers of WasteContext.Web
    /// </summary>
    public abstract class BaseMvcController : Controller
    {
        /// <summary>
        /// Constructor needed for logging purposes.
        /// </summary>
        /// <param name="logger"></param>
        protected BaseMvcController(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Logger of the controller.
        /// </summary>
        protected ILogger Logger { get; }
    }
}