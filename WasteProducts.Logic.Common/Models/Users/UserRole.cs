namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// BLL level model that represents a user belonging to a role.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// UserId for the user that is in the role.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// RoleId for the role.
        /// </summary>
        public string RoleId { get; set; }
    }
}
