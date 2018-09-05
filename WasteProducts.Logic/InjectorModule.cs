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
using WasteProducts.Logic.Mappings.UserMappings;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;

namespace WasteProducts.Logic
{
    public class InjectorModule : NinjectModule
    {
        public override void Load()
        {
            if(Kernel is null)
                return;

            Bind<IDbServiceFactory>().ToFactory();

            Bind<IDbSeedService>().To<DbSeedService>();

            Bind<IDbManagementService>().To<DbManagementService>();

            Bind<ISearchService>().To<LuceneSearchService>();

            BindIMailService();

            BindIUserService();

            BindIUserRoleService();

            BindIProductService();

            BindIMapper();

            void BindIMailService()
            {
                Bind<IMailService>().To<MailService>();
                Bind<IMailService>().ToMethod(ctx => new MailService(null, "somevalidemail@mail.ru", null)).Named("UserIntegrTest");
            }

            void BindIUserService()
            {
                Bind<IUserService>().To<UserService>();
                Bind<IUserService>().ToMethod(ctx =>
                {
                    var repo = ctx.Kernel.Get<IUserRepository>("UserIntegrTest");

                    var mapper = ctx.Kernel.Get<IMapper>("UserService");

                    var mailService = ctx.Kernel.Get<IMailService>("UserIntegrTest");

                    return new UserService(repo, mapper, mailService);
                })
                .Named("UserIntegrTest");
            }

            void BindIUserRoleService()
            {
                Bind<IUserRoleService>().To<UserRoleService>();
                Bind<IUserRoleService>().ToMethod(ctx =>
                {
                    var repo = ctx.Kernel.Get<IUserRoleRepository>("UserIntegrTest");
                    var mapper = ctx.Kernel.Get<IMapper>("UserRoleService");
                    return new UserRoleService(repo, mapper);
                })
                .Named("UserIntegrTest");
            }

            void BindIProductService()
            {
                Bind<IProductService>().To<ProductService>();
                Bind<IProductService>().ToMethod(ctx =>
                {
                    var repo = ctx.Kernel.Get<IProductRepository>("UserIntegrTest");

                    var mapper = ctx.Kernel.Get<IMapper>("ProductService");

                    return new ProductService(repo, mapper);
                })
                .Named("UserIntegrTest");
            }

            void BindIMapper()
            {
                Bind<IMapper>().ToMethod(ctx =>
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<UserProfile>();
                        cfg.AddProfile<UserClaimProfile>();
                        cfg.AddProfile<UserLoginProfile>();
                        cfg.AddProfile<Mappings.UserMappings.ProductProfile>();
                        cfg.AddProfile<UserProductDescriptionProfile>();
                    });

                    return new Mapper(config);
                })
                .Named("UserService");

                Bind<IMapper>().ToMethod(ctx =>
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile(new UserProfile());
                        cfg.AddProfile(new UserClaimProfile());
                        cfg.AddProfile(new UserLoginProfile());
                    });
                    return new Mapper(config);
                })
                .Named("UserRoleService");

                Bind<IMapper>().ToMethod(ctx =>
                {
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

                    return new Mapper(mapConfig);
                })
                .Named("ProductService");
            }
        }
    }
}
