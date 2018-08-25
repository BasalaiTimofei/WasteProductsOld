using System.Web.Mvc;

namespace WasteProducts.Web.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Administration";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            #region ErrorControllerRoute

            context.MapRoute(
                "Administration.Error",
                "Administration/ErrorManagement/{resource}",
                new {controller = "ErrorManagement", action = "Index", resource = UrlParameter.Optional});

            context.MapRoute(
                "Administration.Error.Detail",
                "Administration/ErrorManagement/detail/{resource}",
                new {controller = "ErrorManagement", action = "Detail", resource = UrlParameter.Optional});

            #endregion

            context.MapRoute(
                "Administration_default",
                "Administration/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}