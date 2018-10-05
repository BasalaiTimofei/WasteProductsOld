using System;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Models.Groups
{
    public class GroupProduct
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string ProductId { get; set; }

        /// <summary>
        /// Unique name of concrete Product.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string GroupBoardId { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public virtual string Information { get; set; }
    }
}
