using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models
{
    class GroupProductBoard
    {
        /// <summary>
        /// Id - primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// GroupBordId - secondary key
        /// </summary>
        public int GroupBordId { get; set; }
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
