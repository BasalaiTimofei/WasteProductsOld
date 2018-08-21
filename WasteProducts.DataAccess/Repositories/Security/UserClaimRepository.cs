using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;
using WasteProducts.DataAccess.Common.Repositories.Security;

namespace WasteProducts.DataAccess.Repositories.Security
{
    internal class UserClaimRepository : RepositoryBase<IClaimDb>, IUserClaimRepository
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
