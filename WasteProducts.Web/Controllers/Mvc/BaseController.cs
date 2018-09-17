using System.Web.Mvc;
using Ninject.Extensions.Logging;

namespace WasteProducts.Web.Controllers.Mvc
{
    /// <summary>
    /// Abstract Mvc controller with Logger
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Storage property for ILogger
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Initializes Logger property
        /// </summary>
        /// <param name="logger">abstract logger</param>
        protected BaseController(ILogger logger)
        {
            Logger = logger;
        }
    }
}