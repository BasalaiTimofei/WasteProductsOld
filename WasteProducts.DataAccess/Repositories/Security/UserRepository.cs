using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;
using WasteProducts.DataAccess.Common.Repositories.Security;

namespace WasteProducts.DataAccess.Repositories.Security
{
    /// <summary>
    /// User repository class inheritance from RepositoryBase T and IUserRepository interface
    /// </summary>
    internal class UserRepository : RepositoryBase<IUserDb>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of UserRepository
        /// </summary>
        /// <param name="dbFactory">Used to set DbFactory in base class RepositoryBase </param>
        public UserRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Getting User by username
        /// </summary>
        /// <param name="name">name of user</param>
        /// <returns>Task User</returns>
        public async Task<IUserDb> FindByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName.ToUpper() == name.ToUpper());
        }

        /// <summary>
        /// Getting User by Email
        /// </summary>
        /// <param name="email">User Email</param>
        /// <returns>Task User</returns>
        public async Task<IUserDb> FindByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.ToUpper() == email.ToUpper());
        }

       
    }
}
