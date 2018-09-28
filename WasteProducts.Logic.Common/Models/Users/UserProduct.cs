using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// Description of the specific product.
    /// </summary>
    public class UserProduct
    {
        /// <summary>
        /// Product of this description.
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Rating of this product pescription.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Description contains opinion of the user about the product.
        /// </summary>
        public string Description { get; set; }
    }
}
