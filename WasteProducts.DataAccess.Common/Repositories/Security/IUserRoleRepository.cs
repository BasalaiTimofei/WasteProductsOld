using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    /// <summary>
    /// Interface IUserRoleRepository for the UserRoleRepository.
    /// </summary>
    public interface IUserRoleRepository
    {
        /// <summary>
        /// Checking if user with current id has role with current id role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns>bool value as task</returns>
        Task<bool> IsInRoleAsync(int userId, int roleId);
    }
}