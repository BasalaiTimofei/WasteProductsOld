using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;
using WasteProducts.DataAccess.Common.Repositories.Security;

namespace WasteProducts.DataAccess.Repositories.Security
{
    /// <summary>
    /// UserClaim repository class inheritance from RepositoryBase and IUserClaimRepository interface
    /// </summary>
    internal class UserClaimRepository : RepositoryBase<IClaimDb>, IUserClaimRepository
    {
        /// <summary>
        /// Initializes a new instance of UserClaimRepository
        /// </summary>
        /// <param name="dbFactory">Used to set DbFactory in base class RepositoryBase </param>
        public UserClaimRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Getting UserClaim List by User Id
        /// </summary>
        /// <param name="userId">search by id</param>
        /// <returns>Task List of UserClaim</returns>
        public Task<List<IClaimDb>> GetByUserId(int userId)
        {
            return _dbSet.Where(uc => uc.UserId == userId).ToListAsync();
        }

    }
}
