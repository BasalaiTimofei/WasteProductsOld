using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models
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
        public int GroupDBId { get; set; }
        public virtual GroupDB GroupDB { get; set; }
        /// <summary>
        /// UserId - user which entered in group
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// UserId - user which entered in group
        /// </summary>
        public virtual IList<GroupUserInviteTimeDB> GroupUserInviteTimeDBs { get; set; }
        /// <summary>
        /// Bool - user in a group/not in a group
        ///     true - user in a group
        ///     false - user not in a group
        /// </summary>
        public bool Bool { get; set; }
    }
}
