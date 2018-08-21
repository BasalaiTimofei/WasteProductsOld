using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    public interface IUserRoleRepository
    {
        Task<bool> IsInRoleAsync(int userId, int roleId);
    }
}