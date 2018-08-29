using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.Repositories.UserManagement;

namespace WasteProducts.DataAccess
{
    public class InjectorModule : NinjectModule
    {
        public override void Load()
        {
            if (Kernel is null)
                return;

            Kernel.Bind<WasteContext>().ToSelf().InTransientScope(); ; // TODO : replace with IDbContext in all repositories
            Kernel.Bind<IDbContext>().ToMethod(context => context.Kernel.Get<WasteContext>());
            
            Kernel.Bind<IUserRepository>().To<UserRepository>();
            Kernel.Bind<IUserRoleRepository>().To<UserRoleRepository>();
        }
    }
}
