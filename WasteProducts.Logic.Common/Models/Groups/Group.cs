using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models
{
    public class Group
    {
        /// <summary>
        /// Id - primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name - name group
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Information - additional information
        /// </summary>
        public string Information { get; set; }
        /// <summary>
        /// Admin - user which created group
        /// </summary>
        public int Admin { get; set; }
        /// <summary>
        /// GroupUsers - users which as part of group
        /// </summary>
        public List<GroupUser> GroupUsers { get; set; }
        /// <summary>
        /// GroupBoards - boards with products
        /// </summary>
        public List<GroupBoard> GroupBoards { get; set; }
    }
}
