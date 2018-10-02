using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WasteProducts.Web.Controllers.Mvc;
using OperationCanceledException = System.OperationCanceledException;

namespace WasteProducts.Web
{
    /// <inheritdoc />
    public class Global : HttpApplication
    {

        /// <summary>
        /// That code runs on application error
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            if(exception is OperationCanceledException)
                return;

            var httpContext = ((HttpApplication)sender).Context;
            var currentController = string.Empty;
            var currentAction = string.Empty;
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            

            if (currentRouteData != null)
            {
                if (!string.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (!string.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var controller = new ErrorController();
            var routeData = new RouteData();
            var action = "InternalServerError"; // default error action
            var statusCode = 500; // default status code

            if (exception is HttpException httpException)
            {
                statusCode = httpException.GetHttpCode();

                switch (statusCode)
                {
                    case 403:
                        action = "ForbiddenError";
                        break;
                    case 404:
                        action = "NotFoundError";
                        break;
                }
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.TrySkipIisCustomErrors = true;

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            if (statusCode >= 500)
            {
                controller.ViewData.Model = new HandleErrorInfo(exception, currentController, currentAction);
            }

            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}