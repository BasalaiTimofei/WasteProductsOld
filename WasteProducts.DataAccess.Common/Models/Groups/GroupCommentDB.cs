using System;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class GroupCommentDB
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
        /// This board
        /// </summary>
        public virtual GroupBoardDB GroupBoard { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string CommentatorId { get; set; }

        /// <summary>
        /// User who send message
        /// </summary>
        public virtual UserDB Commentator { get; set; }

        /// <summary>
        /// This comment
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Model modification time
        /// </summary>
        public virtual DateTime? Modified { get; set; }
    }
}
