using System;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class GroupUserDB
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
        /// This group
        /// </summary>
        public virtual GroupDB Group { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// User which entered in group
        /// </summary>
        public virtual UserDB User { get; set; }

        /// <summary>
        /// true - can created boards;
        /// false - can't created boards
        /// </summary>
        public virtual bool RightToCreateBoards { get; set; }

        /// <summary>
        /// User action when administrator inviting
        ///     0 - invite send;
        ///     1 - invite user confirmed;
        ///     2 - invite user rejected;
        /// </summary>
        public virtual int? IsInvited { get; set; }

        /// <summary>
        /// Model modification time
        /// </summary>
        public virtual DateTime? Modified { get; set; }
    }
}
