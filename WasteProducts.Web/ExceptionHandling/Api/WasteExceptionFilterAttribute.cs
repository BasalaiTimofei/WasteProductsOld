using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using WasteProducts.Web.ExceptionHandling.Exceptions;

namespace WasteProducts.Web.ExceptionHandling.Api
{
    public class WasteExceptionFilterAttribute : ExceptionFilterAttribute
    {
        
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is WasteException)
            {
                var wasteException = (WasteException)context.Exception;

                context.Response = new HttpResponseMessage(wasteException.Code)
                {
                    Content = new StringContent(wasteException.Message, Encoding.UTF8, "text/html"),
                };
            }
        }
    }
}