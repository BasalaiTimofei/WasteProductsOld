using System.Web.Http;
using Ninject.Extensions.Logging;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Abstract WebApi controller with Logger
    /// </summary>
    public abstract class BaseApiController : ApiController
    {
        protected ILogger Logger { get; }

        /// <summary>
        /// Initializes Logger property
        /// </summary>
        /// <param name="logger">abstract logger</param>
        protected BaseApiController(ILogger logger)
        {
            Logger = logger;
        }
    }
}
