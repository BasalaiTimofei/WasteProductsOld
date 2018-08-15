using System.Web.Http;
using Ninject;
using NLog;

namespace WasteProducts.Web.Controllers.Api
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