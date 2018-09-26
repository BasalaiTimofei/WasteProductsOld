// <copyright file="AppBuilderExtensions.cs">
//    2017 - Johan Boström
// </copyright>

using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.EntityFramework;
using WasteProducts.IdentityServer.Config;
using WasteProducts.IdentityServer.Db;
using WasteProducts.IdentityServer.Managers;
using WasteProducts.IdentityServer.Services;
using WasteProducts.IdentityServer.Stores;
using WasteProducts.IdentityServer.Infrastructure;

using Owin;

namespace WasteProducts.IdentityServer.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder MapCore(this IAppBuilder app, X509Certificate2 signingCertificate)
        {
            app.Map(Constants.Routes.Core, coreApp =>
            {
                var efConfig = new EntityFrameworkServiceOptions
                {
                    ConnectionString = Constants.ConnectionStringName
                };

                var factory = new IdentityServerServiceFactory();

                factory.RegisterConfigurationServices(efConfig);
                factory.RegisterOperationalServices(efConfig);
                factory.RegisterClientStore(efConfig);
                factory.RegisterScopeStore(efConfig);

                factory.Register(new Registration<UserManager>());
                factory.Register(new Registration<UserStore>());
                factory.UseInMemoryClients(Clients.Get());
                factory.UseInMemoryScopes(Scopes.Get());
                factory.Register(new Registration<Context>(resolver => new Context(Constants.ConnectionStringName)));
                factory.UserService = new Registration<IUserService, UserService>();

                DefaultSetup.Configure(efConfig);

                coreApp.UseIdentityServer(new IdentityServerOptions
                {
                    Factory = factory,
                    SigningCertificate = signingCertificate,
                    SiteName = "WasteProducts.IdentityServer",
                    RequireSsl = true,
                    LoggingOptions = new LoggingOptions
                    {
                        EnableKatanaLogging = true
                    },
                    EventsOptions = new EventsOptions
                    {
                        RaiseFailureEvents = true,
                        RaiseInformationEvents = true,
                        RaiseSuccessEvents = true,
                        RaiseErrorEvents = true
                    }
                });
            });

            return app;
        }
    }
}