using AutoMapper;
using Ninject.Modules;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.DataAccess.Repositories.Products;
using WasteProducts.DataAccess.Repositories.Groups;
using WasteProducts.DataAccess.Repositories.UserManagement;
using WasteProducts.DataAccess.Common.Repositories.Donations;
using WasteProducts.DataAccess.Repositories.Donations;

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

            Bind<IProductRepository>().To<ProductRepository>();
            Bind<ICategoryRepository>().To<CategoryRepository>();

            Bind<ISearchRepository>().To<LuceneSearchRepository>().InSingletonScope();

            Bind<IGroupRepository>().To<GroupRepository>();

            Bind<IDonationRepository>().To<DonationRepository>();

            Bind<IMapper>().ToMethod(ctx =>
            {
                return new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserDB, UserDAL>().ReverseMap();
                }));
            }).WhenInjectedExactlyInto<UserRepository>();
        }
    }
}