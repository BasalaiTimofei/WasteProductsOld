using Microsoft.AspNet.Identity;

namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the Role. Has an inheritance from Microsoft.AspNet.Identity.IRole.
    /// </summary>
    public interface IAppRole : IRole<int>
    {
    }
}
