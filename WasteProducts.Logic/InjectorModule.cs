using Ninject.Extensions.Factory;
using Ninject.Modules;
using Ninject;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Common.Services.MailService;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Services;
using WasteProducts.Logic.Services.MailService;
using WasteProducts.Logic.Services.UserService;
using WasteProducts.DataAccess.Common.Repositories;
using AutoMapper;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Products;
using System;
using WasteProducts.Logic.Mappings;

namespace WasteProducts.Logic
{
    public class InjectorModule : NinjectModule
    {
        public override void Load()
        {
            if(Kernel is null)
                return;

            Bind<IServiceFactory>().ToFactory();

            Bind<IDbSeedService>().To<DbSeedService>();

            Bind<IDbService>().To<DbService>();

            Bind<ITestModelsService>().To<TestModelsService>();

            Bind<IUserService>().To<UserService>();

            Bind<IUserRoleService>().To<UserRoleService>();

            Bind<IMailService>().To<MailService>();

            Bind<IProductService>().ToMethod(ctx =>
            {
                var repo = ctx.Kernel.Get<IProductRepository>("UserIntegrTest");

                var mapConfig = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Product, ProductDB>()
                        .ForMember(m => m.Created,
                            opt => opt.MapFrom(p => p.Name != null ? DateTime.UtcNow : default(DateTime)))
                        .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?)null))
                        .ForMember(m => m.Barcode, opt => opt.Ignore())
                        .ReverseMap();
                    cfg.AddProfile<CategoryProfile>();
                });

                var mapper = new Mapper(mapConfig);

                return new ProductService(repo, mapper);
            })
            .Named("UserIntegrTest");

            Bind<ISearchService>().To<LuceneSearchService>();
        }
    }
}
