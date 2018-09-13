using AutoMapper;
using FluentValidation;
using Ninject.Extensions.Factory;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Modules;
using System;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Common.Services.Groups;
using WasteProducts.Logic.Common.Services.MailService;
using WasteProducts.Logic.Common.Services.Products;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Interceptors;
using WasteProducts.Logic.Mappings;
using WasteProducts.Logic.Mappings.UserMappings;
using WasteProducts.Logic.Services;
using WasteProducts.Logic.Services.Groups;
using WasteProducts.Logic.Services.MailService;
using WasteProducts.Logic.Services.Products;
using WasteProducts.Logic.Services.UserService;
using WasteProducts.Logic.Mappings;
using WasteProducts.Logic.Mappings.Products;
using WasteProducts.Logic.Mappings.UserMappings;
using WasteProducts.Logic.Validators.Search;
using ProductProfile = WasteProducts.Logic.Mappings.ProductProfile;

namespace WasteProducts.Logic
{
    public class InjectorModule : NinjectModule
    {
        public override void Load()
        {
            if (Kernel is null)
            {
                return;
            }

            BindMappers();

            // bind services below
            Bind<IServiceFactory>().ToFactory(); //TODO: Если вы иньектируете дофига сервисов во что то, можно их прописать в интерфейс фабрики и запросить фабрику!

            // database services
            BindDatabaseServices();

            // user services
            BindUserServices();

            Bind<IValidator>().To<BoostedSearchQueryValidator>().WhenInjectedExactlyInto<SearchServiceInterceptor>();
            Bind<ISearchService>().To<LuceneSearchService>().Intercept().With<SearchServiceInterceptor>();

            Bind<IProductService>().To<ProductService>();
            Bind<ICategoryService>().To<CategoryService>();
        }

        private void BindDatabaseServices()
        {
            Bind<IDbService>().To<DbService>();
            Bind<IDbSeedService>().To<DbSeedService>();
            Bind<ITestModelsService>().To<TestModelsService>();
        }

        private void BindUserServices()
        {
            Bind<IMailService>().ToMethod(ctx => new MailService(null, "somevalidemail@mail.ru", null));

            Bind<IUserService>().To<UserService>();
            Bind<IUserRoleService>().To<UserRoleService>();
            Bind<ISearchService>().To<LuceneSearchService>();

            Bind<IGroupService>().To<GroupService>();
            Bind<IGroupBoardService>().To<GroupBoardService>();
            Bind<IGroupProductService>().To<GroupProductService>();
            Bind<IGroupUserService>().To<GroupUserService>();
            Bind<IGroupCommentService>().To<GroupCommentService>();
        }

        private void BindMappers()
        {
            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<UserProfile>();
                    cfg.AddProfile<UserClaimProfile>();
                    cfg.AddProfile<UserLoginProfile>();
                    cfg.AddProfile<Mappings.UserMappings.ProductProfile>();
                    cfg.AddProfile<UserProductDescriptionProfile>();
                }))).WhenInjectedExactlyInto<UserService>();

            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new UserProfile());
                    cfg.AddProfile(new UserClaimProfile());
                    cfg.AddProfile(new UserLoginProfile());
                }))).WhenInjectedExactlyInto<UserRoleService>();

            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Product, ProductDB>()
                        .ForMember(m => m.Created,
                            opt => opt.MapFrom(p => p.Name != null ? DateTime.UtcNow : default(DateTime)))
                        .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?) null))
                        .ForMember(m => m.Barcode, opt => opt.Ignore())
                        .ReverseMap();
                    cfg.AddProfile<CategoryProfile>();
                }))).WhenInjectedExactlyInto<ProductService>();

            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<ProductProfile>();
                    cfg.AddProfile<CategoryProfile>();
                }))).WhenInjectedExactlyInto<CategoryService>();
        }
    }
}
