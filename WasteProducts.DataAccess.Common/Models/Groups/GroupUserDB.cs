using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class GroupUserDB
    {
        /// <summary>
        /// Id - primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id - secondary key
        /// </summary>
        public int GroupId { get; set; }
        public GroupDB GroupDB { get; set; }
        /// <summary>
        /// UserId - user which entered in group
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// UserId - user which entered in group
        /// </summary>
        public List<GroupUserInviteTimeDB> GroupUserInviteTimeDBs { get; set; }
    }
}
