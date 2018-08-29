using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Common.Services.MailService;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Services;
using WasteProducts.Logic.Services.MailService;
using WasteProducts.Logic.Services.UserService;

namespace WasteProducts.Logic
{
    public class InjectorModule : NinjectModule
    {
        public override void Load()
        {
            if(Kernel is null)
                return;

            Kernel.Bind<IDbServiceFactory>().ToFactory();

            Kernel.Bind<IDbSeedService>().To<DbSeedService>();
            Kernel.Bind<IDbManagementService>().To<DbManagementService>();

            Kernel.Bind<IUserService>().To<UserService>();
            Kernel.Bind<IUserRoleService>().To<UserRoleService>();
            Kernel.Bind<IMailService>().To<MailService>();

        }
    }
}
