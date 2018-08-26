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
        public IList<GroupProductDB> GroupProductDBs { get; set; }
        /// <summary>
        /// TimeCreate - datatime when board created
        /// </summary>
        public DateTime TimeCreate { get; set; }
        /// <summary>
        /// TimeDelete - datatime when board deleted
        /// </summary>
        public DateTime? TimeDelete { get; set; }
        /// <summary>
        /// Bool - board deleted/greated
        ///     true - board greated
        ///     false - board deleted
        /// </summary>
        public bool Bool { get; set; }
    }
}
