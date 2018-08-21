using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;
using WasteProducts.DataAccess.Common.Repositories.Security;

namespace WasteProducts.DataAccess.Repositories.Security
{
    /// <summary>
    /// UserLoginRepository repository class inheritance from RepositoryBase and IUserLoginRepository interface
    /// </summary>
    internal class UserLoginRepository : RepositoryBase<IUserLoginDb>, IUserLoginRepository
    {
        /// <summary>
        /// Initializes a new instance of UserLoginRepository
        /// </summary>
        /// <param name="dbFactory">Used to set DbFactory in base class RepositoryBase </param>
        public UserLoginRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Getting UserLogin List by User Id
        /// </summary>
        /// <param name="userId">user id number</param>
        /// <returns>Task List of UserLogin</returns>
        public Task<List<IUserLoginDb>> GetByUserId(int userId)
        {
            return _dbSet.Where(ul => ul.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Getting UserLogin by loginProvider and providerKey
        /// </summary>
        /// <param name="loginProvider">login provider</param>
        /// <param name="providerKey">provider key</param>
        /// <returns>Task UserLogin</returns>
        public Task<IUserLoginDb> FindByLoginProviderAndProviderKey(string loginProvider, string providerKey)
        {
            return _dbSet.FirstOrDefaultAsync(ul => ul.LoginProvider == loginProvider && ul.ProviderKey == providerKey);
        }

    }
}
