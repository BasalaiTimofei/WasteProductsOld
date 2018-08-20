
namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the IUserRole.
    /// </summary>
    public interface IUserRole
    {
        /// <summary>
        /// User Id
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// Navigation user property
        /// </summary>
        IAppUser User { get; set; }

        /// <summary>
        /// Role Id
        /// </summary>
        int RoleId { get; set; }

        /// <summary>
        /// Navigation Role property
        /// </summary>
        IAppRole Role { get; set; }
    }
}
