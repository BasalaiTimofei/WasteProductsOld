namespace WasteProducts.Logic.Common.Models.Groups
{
    /// <summary>
    /// Model representing many-to-many relationship between User and Group entities.
    /// </summary>
    public class GroupUser
    {
        /// <summary>
        /// ID of the group.
        /// </summary>
        public virtual string GroupId { get; set; }

        /// <summary>
        /// ID of the user.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Unique username
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// True if user can create boards;
        /// false - user can't create boards.
        /// </summary>
        public virtual bool RightToCreateBoards { get; set; }
    }
}
