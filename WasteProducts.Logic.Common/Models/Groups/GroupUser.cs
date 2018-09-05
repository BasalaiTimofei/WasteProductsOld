using System;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Models.Groups
{
    public class GroupUser
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// This group
        /// </summary>
        public virtual Group Group { get; set; }

        /// <summary>
        /// User which entered in group
        /// </summary>
        public virtual User User { get; set; }

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
