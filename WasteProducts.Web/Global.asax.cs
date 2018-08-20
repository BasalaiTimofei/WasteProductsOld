using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Elmah;

namespace WasteProducts.Web
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void ErrorLog_Logged(object sender, ErrorLoggedEventArgs args)
        {
            if (!Context.IsCustomErrorEnabled)
                return;

            int statusCode = args.Entry.Error.StatusCode;
            string errorId = args.Entry.Id;
            string redirectUrl = GetRedirectUrl(statusCode);

            if (string.IsNullOrWhiteSpace(redirectUrl))
                return;

            redirectUrl += $"?id={errorId}";

            Context.Response.Clear();
            Context.Response.StatusCode = statusCode;
            Context.Response.TrySkipIisCustomErrors = true;
            Context.ClearError();

            //Response.Redirect(redirectUrl, true);
            Server.TransferRequest(redirectUrl);
        }

        private string GetRedirectUrl(int statusCode)
        {
            string redirectUrl = null;
            if (WebConfigurationManager.GetSection("system.web/customErrors") is CustomErrorsSection errorsSection)
            {
                redirectUrl = errorsSection.DefaultRedirect;

                if (errorsSection.Errors.Count > 0)
                {
                    var item = errorsSection.Errors[statusCode.ToString()];
                    if (item != null && !string.IsNullOrWhiteSpace(item.Redirect))
                        redirectUrl = item.Redirect;
                }
            }
            return redirectUrl;
        }
    }
}