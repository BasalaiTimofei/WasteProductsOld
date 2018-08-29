using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Services;

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
        }
    }
}
