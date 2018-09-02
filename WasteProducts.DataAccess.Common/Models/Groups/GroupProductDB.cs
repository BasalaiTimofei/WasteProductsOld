using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Products;

namespace WasteProducts.DataAccess.Common.Models
{
    public class GroupProductDB
    {
        /// <summary>
        /// Id - primary key
        /// </summary>
        public virtual ProductDB Product { get; set; }

        /// <summary>
        /// GroupBoardDBId - secondary key
        /// </summary>
        public virtual GroupBoardDB GroupBoardDB { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<ProductBoardComment> ProductDiscussion { get; set; }

        /// <summary>
        /// Information - additional information
        /// </summary>
        public string Information { get; set; }
    }
}
