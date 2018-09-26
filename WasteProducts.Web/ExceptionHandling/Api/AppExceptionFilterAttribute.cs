using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace WasteProducts.Web.ExceptionHandling.Api
{
    public class AppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is KeyNotFoundException keyNotFoundException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(keyNotFoundException.Message, Encoding.UTF8, "text/html"),
                };
            }

            if (context.Exception is UnauthorizedAccessException unauthorizedAccessException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(unauthorizedAccessException.Message, Encoding.UTF8, "text/html"),
                };
            }

            if (context.Exception is OperationCanceledException operationCanceledException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent(operationCanceledException.Message, Encoding.UTF8, "text/html"),
                };
            }
        }
        }

    }
