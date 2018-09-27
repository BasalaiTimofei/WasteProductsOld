using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// Description of the specific product given by the specific user.
    /// </summary>
    public class UserProductDescription
    {
        /// <summary>
        /// User who set this description on the product.
        /// </summary>
        public User User { get; set; }

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
