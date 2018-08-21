using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    public interface IUserClaimRepository
    {
        Task<List<IClaimDb>> GetByUserId(int userId);
    }
}