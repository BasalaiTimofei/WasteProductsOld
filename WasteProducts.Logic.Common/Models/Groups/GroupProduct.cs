using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Models.Groups
{
    public class GroupProduct
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// This product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// This board
        /// </summary>
        public virtual GroupBoard GroupBoard { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public string Information { get; set; }
    }
}
