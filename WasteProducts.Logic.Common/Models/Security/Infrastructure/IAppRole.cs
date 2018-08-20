using Microsoft.AspNet.Identity;

namespace WasteProducts.Logic.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the Role. Has an inheritance from Microsoft.AspNet.Identity.IRole.
    /// </summary>
    public interface IAppRole : IRole<int>
    {
    }
}
