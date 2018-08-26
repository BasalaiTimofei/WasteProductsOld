using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    /// <summary>
    /// Interface IUserLoginRepository for the UserLoginRepository. 
    /// </summary>
    public interface IUserLoginRepository
    {
        /// <summary>
        /// Getting UserLogin by loginProvider and providerKey
        /// </summary>
        /// <param name="loginProvider">login provider</param>
        /// <param name="providerKey">provider key</param>
        /// <returns>UserLogin as Task</returns>
        Task<IUserLoginDb> FindByLoginProviderAndProviderKey(string loginProvider, string providerKey);

        /// <summary>
        /// Getting UserLogin List by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of UserLogin as Task</returns>
        Task<List<IUserLoginDb>> GetByUserId(int userId);
    }
}