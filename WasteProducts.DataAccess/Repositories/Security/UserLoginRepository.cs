using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Repositories.Security
{
    class UserLoginRepository : RepositoryBase<IUserLoginDb>
    {
        public UserLoginRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public Task<List<IUserLoginDb>> GetByUserId(int userId)
        {
            return dbSet.Where(ul => ul.UserId == userId).ToListAsync();
        }

        public Task<IUserLoginDb> FindByLoginProviderAndProviderKey(string loginProvider, string providerKey)
        {
            return dbSet.FirstOrDefaultAsync(ul => ul.LoginProvider == loginProvider && ul.ProviderKey == providerKey);
        }

    }
}
