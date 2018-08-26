using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models
{
    public class GroupUserInviteTimeDB
    {
        /// <summary>
        /// Id - primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// GroupUserDBId secondary Id
        /// </summary>
        public int GroupUserDBId { get; set; }
        public GroupUserDB GroupUserDB { get; set; }
        /// <summary>
        /// Entry - data entry in the group
        /// </summary>
        public DateTime TimeEntry { get; set; }
        /// <summary>
        /// Exit - data left the group
        /// </summary>
        public DateTime TimeExit { get; set; }
        /// <summary>
        /// Invite - user action when administrator inviting
        ///     0 - invite send;
        ///     1 - invite user confirmed;
        ///     2 - invite user rejected;
        /// </summary>
        public int Invite { get; set; }
    }
}
