using System.Web;
using System.Web.Http;
using Elmah;
using NLog;

namespace WasteProducts.Web.Controllers.Api
{
    public class ErrorController : BaseApiController
    {
        public ErrorController(ILogger logger) : base(logger)
        {
        }

        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        [HttpHead]
        [HttpOptions]
        public IHttpActionResult NotFound(string path)
        {
            var exception = new HttpException(404, "404 Not Found: /" + path);

            //log Nlog
            Logger.Error(exception, "Route don't exist");

            // log error to ELMAH
            ErrorSignal.FromCurrentContext().Raise(exception);

            // return 404
            return NotFound();
        }
    }
}