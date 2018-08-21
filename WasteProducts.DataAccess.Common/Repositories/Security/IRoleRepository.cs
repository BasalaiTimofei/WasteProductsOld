using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    public interface IRoleRepository
    {
        IRoleDb FindByName(string roleName);
        Task<IRoleDb> FindByNameAsync(CancellationToken cancellationToken, string roleName);
        Task<IRoleDb> FindByNameAsync(string name);
        Task<List<string>> GetRolesNameByUserId(int userId);
    }
}