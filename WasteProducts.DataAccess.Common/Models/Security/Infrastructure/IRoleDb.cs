using Microsoft.AspNet.Identity;

namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    public interface IRoleDb : IRole<int>
    {
        System.Collections.Generic.ICollection<IUserDb> Users { get; set; }
    }
}