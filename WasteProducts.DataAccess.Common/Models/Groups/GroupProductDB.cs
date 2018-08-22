using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models
{
    public class GroupProductDB
    {
        /// <summary>
        /// Id - primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// GroupBoardId - secondary key
        /// </summary>
        public int GroupBoardId { get; set; }
        public GroupBoardDB GroupBoardDB { get; set; }
        /// <summary>
        /// ProductId - product which add at board
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Information - additional information
        /// </summary>
        public string Information { get; set; }
    }
}
