using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;
using WasteProducts.DataAccess.Common.Repositories.Security;

namespace WasteProducts.DataAccess.Repositories.Security
{
    internal class UserRepository : RepositoryBase<IUserDb>, IUserRepository
    {
        public UserRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IUserDb> FindByNameAsync(string name)
        {
            return await dbSet.FirstOrDefaultAsync(u => u.UserName.ToUpper() == name.ToUpper());
        }

        public async Task<IUserDb> FindByEmailAsync(string email)
        {
            return await dbSet.FirstOrDefaultAsync(u => u.Email.ToUpper() == email.ToUpper());
        }

       
    }
}
