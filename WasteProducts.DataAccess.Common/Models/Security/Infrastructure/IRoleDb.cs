using Microsoft.AspNet.Identity;

namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// IRoleDb Interface for the Role. Has an inheritance from Microsoft.AspNet.Identity.IRole.
    /// </summary>
    public interface IRoleDb : IRole<int>
    {
        /// <summary>
        /// Navigation user collection property 
        /// </summary>
        System.Collections.Generic.ICollection<IUserDb> Users { get; set; }
    }
}