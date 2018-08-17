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
        public List<GroupUserDB> GroupUserDBs { get; set; }
        /// <summary>
        /// GroupBordDBs - boards with products
        /// </summary>
        public List<GroupBoardDB> GroupBoardDBs { get; set; }
        /// <summary>
        /// Create - datatime when group created
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// Delete - datatime when group deleted
        /// </summary>
        public DateTime Delete { get; set; }
    }
}