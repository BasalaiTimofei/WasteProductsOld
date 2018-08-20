using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Repositories.Security
{
    public class UserClaimRepository : RepositoryBase<IClaimDb>
    {
        public UserClaimRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public Task<List<IClaimDb>> GetByUserId(int userId)
        {
            return dbSet.Where(uc => uc.UserId == userId).ToListAsync();
        }

    }
}
