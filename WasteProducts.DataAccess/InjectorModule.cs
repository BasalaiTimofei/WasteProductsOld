using AutoMapper;
using Ninject.Modules;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.DataAccess.Common.Repositories.Users;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.DataAccess.Repositories.Products;
using WasteProducts.DataAccess.Repositories.Groups;
using WasteProducts.DataAccess.Repositories.Users;
using WasteProducts.DataAccess.Common.Repositories.Diagnostic;
using WasteProducts.DataAccess.Repositories.Diagnostic;

namespace WasteProducts.DataAccess
{
    public class InjectorModule : NinjectModule
    {
        public override void Load()
        {
            if (Kernel is null)
                return;

            // context bindings
            Bind<WasteContext>().ToSelf().InTransientScope();
            Bind<IDatabase>().To<Database>().InTransientScope();

            // bind repositories below
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUserRoleRepository>().To<UserRoleRepository>();
            Bind<IDiagnosticRepository>().To<DiagnosticRepository>();

            Bind<IProductRepository>().To<ProductRepository>();
            Bind<ICategoryRepository>().To<CategoryRepository>();

            Bind<ISearchRepository>().To<LuceneSearchRepository>().InSingletonScope();

            Bind<IGroupRepository>().To<GroupRepository>();

            Bind<IMapper>().ToMethod(ctx =>
            {
                return new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserDB, UserDAL>().ReverseMap();
                }));
            }).WhenInjectedExactlyInto<UserRepository>();

            Bind<Bogus.Faker>().ToSelf();
        }

        /// <summary>
        /// Method not for use, just for preventing some bug made by .NET "optimization"
        /// </summary>
        public void FixEfProviderServicesProblem()
        {
            //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            //Make sure the provider assembly is available to the running application. 
            //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.
            
            // Try to fix a bug with StackOverflow copy-paste
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
