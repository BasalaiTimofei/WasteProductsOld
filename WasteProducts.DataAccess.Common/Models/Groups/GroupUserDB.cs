using System;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    /// <summary>
    /// Entity representing many-to-many relationship between User and Group entities.
    /// </summary>
    public class GroupUserDB
    {
        /// <summary>
        /// ID of the group.
        /// </summary>
        public virtual string GroupId { get; set; }

        /// <summary>
        /// Group entity.
        /// </summary>
        public virtual GroupDB Group { get; set; }

        /// <summary>
        /// ID of the user.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// User entity.
        /// </summary>
        public virtual UserDB User { get; set; }

        /// <summary>
        /// True if user can create boards;
        /// false - user can't create boards.
        /// </summary>
        public virtual bool RightToCreateBoards { get; set; }

        /// <summary>
        /// True if user have seen and confirmed inviting to the group.
        /// False if user didn't see the invite.
        /// </summary>
        public virtual bool IsConfirmed { get; set; }

        /// <summary>
        /// Specifies timestamp of inviting the user to the group.
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Specifies timestamp of modifying of any property of the entity in the database.
        /// </summary>
        public virtual DateTime? Modified { get; set; }
    }
}
