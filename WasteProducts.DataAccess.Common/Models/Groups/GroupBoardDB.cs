using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models
{
    public class GroupBoardDB
    {
        /// <summary>
        /// Id - primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name - name board
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Information - additional information
        /// </summary>
        public string Information { get; set; }
        /// <summary>
        /// GroupId - secondary key
        /// </summary>
        public int GroupId { get; set; }
        public GroupDB GroupDB { get; set; }
        /// <summary>
        /// UserId - user which created board
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// GroupProductDBs - products which add at board
        /// </summary>
        public List<GroupProductDB> GroupProductDBs { get; set; }
        /// <summary>
        /// Create - datatime when board created
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// Delete - datatime when board deleted
        /// </summary>
        public DateTime Delete { get; set; }
    }
}
