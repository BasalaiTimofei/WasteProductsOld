using Microsoft.AspNet.Identity;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security.Strores
{
    /// <summary>
    /// Interface for the RoleStore. Has an inheritance from Microsoft.AspNet.Identity.IAppUserStore,IUserClaimStore,IUserRoleStore,IUserPasswordStore,IUserSecurityStampStore,IUserEmailStore,IUserPhoneNumberStore,IUserTwoFactorStore,IUserLockoutStore.
    /// </summary>
    public interface IAppUserStore : IUserLoginStore<IAppUser, int>,
      IUserClaimStore<IAppUser, int>,
      IUserRoleStore<IAppUser, int>,
      IUserPasswordStore<IAppUser, int>,
      IUserSecurityStampStore<IAppUser, int>,
      IUserEmailStore<IAppUser, int>,
      IUserPhoneNumberStore<IAppUser, int>,
      IUserTwoFactorStore<IAppUser, int>,
      IUserLockoutStore<IAppUser, int>
    { }
}
