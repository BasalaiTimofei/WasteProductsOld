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
                name: "Administration.Error",
                url: "Administration/ErrorManagement/{resource}",
                defaults: new { controller = "ErrorManagement", action = "Index", resource = UrlParameter.Optional });

            context.MapRoute(
                name: "Administration.Error.Detail",
                url: "Administration/ErrorManagement/detail/{resource}",
                defaults: new { controller = "ErrorManagement", action = "Detail", resource = UrlParameter.Optional });

            #endregion

            context.MapRoute(
                name: "Administration_default",
                url: "Administration/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}