using System;
using WasteProducts.DataAccess.Common.Models.Products;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class GroupProductDB
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
        /// This product
        /// </summary>
        public virtual ProductDB Product { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string GroupBoardId { get; set; }

        /// <summary>
        /// This board
        /// </summary>
        public virtual GroupBoardDB GroupBoard { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// Model modification time
        /// </summary>
        public virtual DateTime? Modified { get; set; }
    }
}
