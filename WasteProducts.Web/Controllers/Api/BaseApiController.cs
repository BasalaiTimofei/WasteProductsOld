using System.Web.Http;
using NLog;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Abstract class-parent for all API controllers of WasteContext.Web
    /// </summary>
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Constructor needed for logging purposes.
        /// </summary>
        /// <param name="logger"></param>
        protected BaseApiController(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Logger of the controller.
        /// </summary>
        protected ILogger Logger { get; }
    }
}