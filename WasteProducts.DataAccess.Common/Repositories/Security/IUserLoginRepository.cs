using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    public interface IUserLoginRepository
    {
        Task<IUserLoginDb> FindByLoginProviderAndProviderKey(string loginProvider, string providerKey);
        Task<List<IUserLoginDb>> GetByUserId(int userId);
    }
}