using System.Web.Http.ExceptionHandling;
using Ninject.Modules;
using Ninject.Web.Mvc.FilterBindingSyntax;
using Ninject.Web.WebApi.FilterBindingSyntax;
using WasteProducts.Web.ExceptionHandling.Api;
using WasteProducts.Web.ExceptionHandling.Mvc;


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
            Kernel.Bind<IExceptionLogger>().To<ApiUnhandledExceptionExceptionLogger>();
            Kernel.BindHttpFilter<ApiValidationExceptionFilterAttribute>(System.Web.Http.Filters.FilterScope.Action);
        }
    }
}