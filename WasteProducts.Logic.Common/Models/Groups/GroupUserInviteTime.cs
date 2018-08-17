using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models
{
    class GroupUserInviteTime
    {
        /// <summary>
        /// Id - primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// GroupUserDBId secondary Id
        /// </summary>
        public int GroupUserId { get; set; }
        /// <summary>
        /// Entry - data entry in the group
        /// </summary>
        public DateTime Entry { get; set; }
        /// <summary>
        /// Exit - data left the group
        /// </summary>
        public DateTime Exit { get; set; }
        /// <summary>
        /// Invite - user action when administrator inviting
        ///     0 -invite send;
        ///     1- invite user confirmed;
        ///     2- invite user rejected;
        /// </summary>
        public int Invite { get; set; }
    }
}
