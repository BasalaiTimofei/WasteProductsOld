using System.Web.Http;
using NLog;

namespace WasteProducts.Web.Controllers.Api
{
    public abstract class BaseApiController : ApiController
    {
        protected BaseApiController(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }
    }
}