using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using FluentValidation;

namespace WasteProducts.Web.ExceptionHandling.Api
{
    /// <summary>
    /// WebApi validation exception filter attribute
    /// </summary>
    public class ApiValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ValidationException exception)
            {
                var modelState = actionExecutedContext.ActionContext.ModelState;

                foreach (var validationFailure in exception.Errors)
                {
                    modelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
                }

                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
                actionExecutedContext.Response.Content = new StringContent(exception.Message, Encoding.UTF8, "text/html");
                    
                    //ReasonPhrase = exception.Message;
            }
        }
    }
}