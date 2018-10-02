using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using Ninject.Modules;
using Ninject.Web.Mvc.FilterBindingSyntax;
using Ninject.Web.WebApi.FilterBindingSyntax;
using WasteProducts.Logic.Common.Services.Notifications;
using WasteProducts.Web.ExceptionHandling.Api;
using WasteProducts.Web.ExceptionHandling.Mvc;
using WasteProducts.Web.Utils.Hubs;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;


namespace WasteProducts.Web
{
    /// <inheritdoc />
    public class InjectionModule: NinjectModule
    {
        /// <inheritdoc />
        public override void Load()
        {
            // mvc filtres
            Kernel.BindFilter<MvUnhandledExceptionFilterAttribute>(System.Web.Mvc.FilterScope.Global, 99);
            // api filtres
            //Kernel.BindHttpFilter<AuthorizeAttribute>(System.Web.Http.Filters.FilterScope.Controller);
            Kernel.BindHttpFilter<ApiValidationExceptionFilterAttribute>(System.Web.Http.Filters.FilterScope.Action);
            Kernel.Bind<IExceptionLogger>().To<ApiUnhandledExceptionLogger>();

            Kernel.Bind<INotificationProvider>().To<SignalRNotifiactionProvider>();
        }
    }
}