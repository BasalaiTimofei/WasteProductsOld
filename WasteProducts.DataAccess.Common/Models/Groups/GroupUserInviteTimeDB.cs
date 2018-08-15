using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class GroupUserInviteTimeDB
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// GroupUserDB Id
        /// </summary>
        public int GroupUserDBId { get; set; }
        public GroupUserDB GroupUserDB { get; set; }
        /// <summary>
        /// Entry - data entry in the group
        /// </summary>
        public DateTime Entry { get; set; }
        /// <summary>
        /// Exit - data left the group
        /// </summary>
        public DateTime Exit { get; set; }
        /// <summary>
        /// user action when inviting administrator
        /// 0 -invite send;
        /// 1- invite user confirmed;
        /// 2- invite user rejected;
        /// </summary>
        public int Invite { get; set; }
    }
}
