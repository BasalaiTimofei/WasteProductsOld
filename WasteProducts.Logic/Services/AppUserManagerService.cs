using Microsoft.AspNet.Identity;
using WasteProducts.Logic.Common.Models.Security.Infrastructure;

namespace WasteProducts.Logic.Services
{
    /// <summary>
    /// App User Manager Service has an inheritance from Microsoft.AspNet.Identity.UserManager. Security model class
    /// </summary>
    public class AppUserManagerService : UserManager<IAppUser, int>
    {
        /// <summary>
        /// Initializes a new instance of AppUserManagerService
        /// </summary>
        /// <param name="store">Used to set store into UserManager</param>
        public AppUserManagerService(IAppUserStore store) : base(store)
        {

        }
    }
}
