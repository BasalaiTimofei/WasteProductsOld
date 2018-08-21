using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    /// <summary>
    /// Interface IUserClaimRepository for the UserClaimRepository.
    /// </summary>
    public interface IUserClaimRepository
    {
        /// <summary>
        /// Getting UserClaim List by User Id
        /// </summary>
        /// <param name="userId">search by id</param>
        /// <returns>List of UserClaim list</returns>
        Task<List<IClaimDb>> GetByUserId(int userId);
    }
}