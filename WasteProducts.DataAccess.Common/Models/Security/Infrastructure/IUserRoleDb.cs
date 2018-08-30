namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the UserRoleDb.
    /// </summary>
    public interface IUserRoleDb
    {
        /// <summary>
        /// Navigation Role property
        /// </summary>
        IRoleDb Role { get; set; }

        /// <summary>
        /// Role Id
        /// </summary>
        int RoleId { get; set; }

        /// <summary>
        /// Navigation user property
        /// </summary>
        IUserDb User { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        int UserId { get; set; }
    }
}