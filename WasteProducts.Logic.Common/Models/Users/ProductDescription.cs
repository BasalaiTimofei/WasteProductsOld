using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// Description of the specific product.
    /// </summary>
    public class ProductDescription
    {
        /// <summary>
        /// Product of this description.
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Rating of this product pescription.
        /// </summary>
        public virtual int Rating { get; set; }

        /// <summary>
        /// Description contains opinion of the user about the product.
        /// </summary>
        public virtual string Description { get; set; }
    }
}
