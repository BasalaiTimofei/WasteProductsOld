using System.Web.Http;
using Ninject;
using NLog;

namespace WasteProducts.Web.Controllers.Base
{
    public class BaseApiController : ApiController
    {
        [Inject]
        public ILogger Logger { protected get; set; }
    }
}