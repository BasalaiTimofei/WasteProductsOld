using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.IdentityServer.Config;
using WasteProducts.IdentityServer.Services;

namespace WasteProducts.IdentityServer.Extensions
{
    public static class IdentityServerServiceFactoryExtensions
    {
        public static IdentityServerServiceFactory Configure(this IdentityServerServiceFactory factory)
        {
            factory
                .UseInMemoryClients(Clients.Load())
                .UseInMemoryScopes(Scopes.Load());

            factory.Register(new Registration<WasteContext>(resolver => new WasteContext(null)));
            factory.Register(new Registration<UserStore>());
            factory.Register(new Registration<UserManager>());
            factory.UserService = new Registration<IUserService, IdentityUserService>();

            return factory;
        }
    }
}