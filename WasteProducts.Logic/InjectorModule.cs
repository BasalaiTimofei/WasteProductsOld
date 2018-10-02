using AutoMapper;
using FluentValidation;
using Moq;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Search;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Common.Services.Donations;
using WasteProducts.Logic.Common.Services.Groups;
using WasteProducts.Logic.Common.Services.Mail;
using WasteProducts.Logic.Common.Services.Notifications;
using WasteProducts.Logic.Common.Services.Products;
using WasteProducts.Logic.Common.Services.Users;
using WasteProducts.Logic.Extensions;
using WasteProducts.Logic.Interceptors;
using WasteProducts.Logic.Mappings.Donations;
using WasteProducts.Logic.Mappings.Barcods;
using WasteProducts.Logic.Mappings.Groups;
using WasteProducts.Logic.Mappings.Products;
using WasteProducts.Logic.Mappings.Users;
using WasteProducts.Logic.Services;
using WasteProducts.Logic.Services.Barcods;
using WasteProducts.Logic.Services.Donations;
using WasteProducts.Logic.Services.Groups;
using WasteProducts.Logic.Services.Mail;
using WasteProducts.Logic.Services.Notifications;
using WasteProducts.Logic.Services.Products;
using WasteProducts.Logic.Services.Users;
using ProductProfile = WasteProducts.Logic.Mappings.Products.ProductProfile;

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

            BindInterceptors();
            BindValidators();
            BindMappers();

            Bind<IServiceFactory>().ToFactory(); //TODO: Если вы иньектируете дофига сервисов во что то, можно их прописать в интерфейс фабрики и запросить фабрику!

            // bind services below
            BindDatabaseServices();
            BindUserServices();
            BindGroupServices();
            BindProductServices();
            BindBarcodeServices();
            BindDonationServices();

            Bind<ISearchService>().To<LuceneSearchService>().ValidateArguments(typeof(BoostedSearchQuery));

            Bind<INotificationService>().To<NotificationService>();
        }

        private void BindInterceptors()
        {
            Bind<TraceInterceptor>().ToSelf();
            Bind<ArgumentValidationInterceptor>().ToSelf();
        }

        private void BindValidators()
        {
            AssemblyScanner
                .FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                .ForEach(result => Kernel.Bind(result.InterfaceType).To(result.ValidatorType).InTransientScope());
        }

        private void BindDatabaseServices()
        {
            Bind<IDbService>().To<DbService>();
            Bind<ITestModelsService>().To<TestModelsService>();
        }

        private void BindUserServices()
        {
            Bind<IMailService>().ToMethod(ctx =>
            {
                var appSettingsReader = new AppSettingsReader();

                string ourEmail = (string)appSettingsReader.GetValue("OurMailAddress", typeof(string));
                string host = (string)appSettingsReader.GetValue("Host", typeof(string));
                int port = (int)appSettingsReader.GetValue("Port", typeof(int));
                SmtpDeliveryMethod method = (SmtpDeliveryMethod)appSettingsReader.GetValue("SMTPDeliveryMethod", typeof(int));
                string ourMailPassword = (string)appSettingsReader.GetValue("OurMailPassword", typeof(string));
                bool enableSsl = (bool)appSettingsReader.GetValue("EnableSSL", typeof(bool));

                var client = new SmtpClient(host, port)
                {
                    DeliveryMethod = method,
                    Credentials = new NetworkCredential(ourEmail, ourMailPassword),
                    EnableSsl = enableSsl
                };

                return new MailService(client, ourEmail);
            });

            Bind<IUserService>().To<UserService>();
            Bind<IUserRoleService>().To<UserRoleService>();
        }

        private void BindGroupServices()
        {
            Bind<IGroupService>().To<GroupService>().ValidateArguments(typeof(Group));
            Bind<IGroupBoardService>().To<GroupBoardService>().ValidateArguments(typeof(GroupBoard));
            Bind<IGroupProductService>().To<GroupProductService>().ValidateArguments(typeof(GroupProduct));
            Bind<IGroupUserService>().To<GroupUserService>().ValidateArguments(typeof(GroupUser));
            Bind<IGroupCommentService>().To<GroupCommentService>().ValidateArguments(typeof(GroupComment));
        }

        private void BindProductServices()
        {
            Bind<IProductService>().To<ProductService>().ValidateArguments(typeof(Product), typeof(Category));
            Bind<ICategoryService>().To<CategoryService>().ValidateArguments(typeof(Category));
        }

        private void BindBarcodeServices()
        {
            Bind<IBarcodeService>().To<BarcodeService>();
            Bind<IBarcodeScanService>().To<BarcodeScanService>();
            Bind<IBarcodeCatalogSearchService>().To<BarcodeCatalogSearchService>();
            Bind<ICatalog>().To<EDostavkaCatalog>();
            Bind<ICatalog>().To<PriceGuardCatalog>();
            Bind<IHttpHelper>().To<HttpHelper>();
        }

        private void BindDonationServices()
        {
            Bind<IVerificationService>().To<PayPalVerificationService>();
            Bind<IDonationService>().To<PayPalService>();
        }

        private void BindMappers()
        {
            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<UserProfile>();
                    cfg.AddProfile<ProductProfile>();
                    cfg.AddProfile<UserProductDescriptionProfile>();
                    cfg.AddProfile<FriendProfile>();
                    cfg.AddProfile<UserProductProfile>();
                    cfg.AddProfile<GroupOfUserProfile>();
                })))
                .WhenInjectedExactlyInto<UserService>();

            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new UserProfile());
                })))
                .WhenInjectedExactlyInto<UserRoleService>();

            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Product, ProductDB>()
                        .ForMember(m => m.Created,
                            opt => opt.MapFrom(p => p.Name != null ? DateTime.UtcNow : default(DateTime)))
                        .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?)null))
                        .ForMember(m => m.Barcode, opt => opt.Ignore())
                        .ReverseMap();
                    cfg.AddProfile<CategoryProfile>();
                })))
                .WhenInjectedExactlyInto<ProductService>();

            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<ProductProfile>();
                    cfg.AddProfile<CategoryProfile>();
                })))
                .WhenInjectedExactlyInto<CategoryService>();

            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new BarcodeProfile());
                })))
                .WhenInjectedExactlyInto<BarcodeService>();

            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<GroupBoardProfile>();
                    cfg.AddProfile<GroupCommentProfile>();
                    cfg.AddProfile<GroupProductProfile>();
                    cfg.AddProfile<GroupProfile>();
                    cfg.AddProfile<GroupUserProfile>();
                })))
            .WhenInjectedExactlyInto<GroupService>();

            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<GroupProfile>();
                    cfg.AddProfile<GroupUserProfile>();
                })))
            .WhenInjectedExactlyInto<GroupUserService>();

            Bind<IMapper>().ToMethod(ctx =>
            new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AddressProfile>();
                cfg.AddProfile<DonorProfile>();
                cfg.AddProfile<DonationProfile>();
            })))
            .WhenInjectedExactlyInto<PayPalService>();
        }
    }
}