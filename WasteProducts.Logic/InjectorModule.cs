using Ninject.Extensions.Factory;
using Ninject.Modules;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Common.Services.Groups;
using WasteProducts.Logic.Common.Services.MailService;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Services;
using WasteProducts.Logic.Services.Groups;
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

            Bind<IDbServiceFactory>().ToFactory();

            Bind<IDbSeedService>().To<DbSeedService>();
            Bind<IDbManagementService>().To<DbManagementService>();

            Bind<IUserService>().To<UserService>();
            Bind<IUserRoleService>().To<UserRoleService>();
            Bind<IMailService>().To<MailService>();
            Bind<ISearchService>().To<LuceneSearchService>();

            Bind<IGroupService>().To<GroupService>();
            Bind<IGroupBoardService>().To<GroupBoardService>();
            Bind<IGroupProductService>().To<GroupProductService>();
            Bind<IGroupUserService>().To<GroupUserService>();
            Bind<IGroupCommentService>().To<GroupCommentService>();
        }
    }
}
