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

        /// <summary>
        /// true - can created boards;
        /// false - can't created boards
        /// </summary>
        public virtual bool RigtToCreateBoards { get; set; }

        /// <summary>
        /// User action when administrator inviting
        ///     0 - invite send;
        ///     1 - invite user confirmed;
        ///     2 - invite user rejected;
        /// </summary>
        public virtual int? IsInvited { get; set; }
    }
}
