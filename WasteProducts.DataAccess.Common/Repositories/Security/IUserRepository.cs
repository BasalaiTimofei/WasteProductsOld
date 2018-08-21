using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    /// <summary>
    /// Interface IUserRepository for the UserRepository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Getting User by Email async
        /// </summary>
        /// <param name="email">User Email</param>
        /// <returns>User as Task</returns>
        Task<IUserDb> FindByEmailAsync(string email);

        /// <summary>
        /// Getting User by username async
        /// </summary>
        /// <param name="name">name of user</param>
        /// <returns>User as Task</returns>
        Task<IUserDb> FindByNameAsync(string name);
    }
}