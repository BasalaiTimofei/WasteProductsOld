using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models
{
    public class GroupBoard
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
        /// <summary>
        /// UserId - user which created board
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// GroupProducts - products which add at board
        /// </summary>
        public IList<GroupProduct> GroupProducts { get; set; }
    }
}
