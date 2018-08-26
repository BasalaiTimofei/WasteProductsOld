using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models
{
    public class GroupDB
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
        /// GroupUserDBs - users which as part of group
        /// </summary>
        public IList<GroupUserDB> GroupUserDBs { get; set; }
        /// <summary>
        /// GroupBordDBs - boards with products
        /// </summary>
        public IList<GroupBoardDB> GroupBoardDBs { get; set; }
        /// <summary>
        /// TimeCreate - datatime when group created
        /// </summary>
        public DateTime TimeCreate { get; set; }
        /// <summary>
        /// TimeDelete - datatime when group deleted
        /// </summary>
        public DateTime? TimeDelete { get; set; }
        /// <summary>
        /// Bool - group deleted/greated
        ///     true - group greated
        ///     false - group deleted
        /// </summary>
        public bool Bool { get; set; }
    }
}