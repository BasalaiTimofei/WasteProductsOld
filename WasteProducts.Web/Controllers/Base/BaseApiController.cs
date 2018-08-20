using System.Web.Http;
using NLog;

namespace WasteProducts.Web.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected ILogger Logger { get; }

        protected BaseApiController(ILogger logger)
        {
            Logger = logger;
        }
    }
}