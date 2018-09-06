using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Bind<IDatabase>().To<Database>().InTransientScope();

            Bind<IUserRepository>().To<UserRepository>();

            Bind<IProductRepository>().ToMethod(c =>
            {
                var db = new WasteContext("name=ConStrByServer");
                return new ProductRepository(db);
            })
            .Named("UserIntegrTest");

            Bind<IUserRoleRepository>().To<UserRoleRepository>();

            Bind<ISearchRepository>().To<LuceneSearchRepository>().InSingletonScope();
        }
    }
}
