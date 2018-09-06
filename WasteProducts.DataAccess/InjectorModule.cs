using Ninject;
using Ninject.Modules;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.DataAccess.Repositories.UserManagement;

namespace WasteProducts.DataAccess
{
    public class InjectorModule : NinjectModule
    {
        public override void Load()
        {
            if (Kernel is null)
                return;

            Bind<WasteContext>().ToSelf().InTransientScope();
            Bind<WasteContext>().ToMethod(ctx => new WasteContext("name=UserIntegrTest")).Named("UserIntegrTest");

            Bind<IDatabase>().To<Database>().InTransientScope();

            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUserRepository>().ToMethod(ctx =>
            {
                var context = ctx.Kernel.Get<WasteContext>("UserIntegrTest");
                return new UserRepository(context);
            })
            .Named("UserIntegrTest");

            Bind<IUserRoleRepository>().To<UserRoleRepository>();
            Bind<IUserRoleRepository>().ToMethod(ctx =>
            {
                var context = ctx.Kernel.Get<WasteContext>("UserIntegrTest");
                return new UserRoleRepository(context);
            })
            .Named("UserIntegrTest");

            Bind<IProductRepository>().To<ProductRepository>();
            Bind<IProductRepository>().ToMethod(ctx =>
            {
                var context = ctx.Kernel.Get<WasteContext>("UserIntegrTest");
                return new ProductRepository(context);
            })
            .Named("UserIntegrTest");

            Bind<ISearchRepository>().To<LuceneSearchRepository>().InSingletonScope();
        }
    }
}
