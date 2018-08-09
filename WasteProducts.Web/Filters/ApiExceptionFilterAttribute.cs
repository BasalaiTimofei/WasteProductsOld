using System;

using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using NLog;


namespace WasteProducts.Web.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ApiExceptionFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;

            //Todo: handle api exceptions 4xx-5xx

            _logger?.Error(exception, "Unhandled exception in Api controller", context.ActionContext.ActionDescriptor);

            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            response.Content = new ObjectContent<object>(new {Msg = $"Ooops, We have a exception!", Exception= exception }, new JsonMediaTypeFormatter());
            response.RequestMessage = context.Request;

            context.Response = response;
        }
    }
}