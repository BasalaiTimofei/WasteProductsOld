using System;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Models.Groups
{
    public class GroupUser
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual Guid GroupId { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string UserId { get; set; }
    }
}
