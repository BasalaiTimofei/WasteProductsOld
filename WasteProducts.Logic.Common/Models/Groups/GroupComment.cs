using System;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Models.Groups
{
    public class GroupComment
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual Guid GroupBoardId { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string CommentatorId { get; set; }

        /// <summary>
        /// This comment
        /// </summary>
        public virtual string Comment { get; set; }
    }
}
