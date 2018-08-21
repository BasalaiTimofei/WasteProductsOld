using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    public interface IUserRepository
    {
        Task<IUserDb> FindByEmailAsync(string email);
        Task<IUserDb> FindByNameAsync(string name);
    }
}