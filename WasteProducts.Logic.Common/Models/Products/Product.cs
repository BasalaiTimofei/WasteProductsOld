using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Models.Products
{
    /// <summary>
    /// Model for entity Product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique identifier of concrete Product.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Unique name of concrete Product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines the Product category.
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Defines the Product barcode.
        /// </summary>
        public virtual Barcode Barcode { get; set; }

        /// <summary>
        /// Defines the Average Rating of the product.
        /// </summary>
        public decimal? AvgRating { get; }

        /// <summary>
        /// Defines the product description
        /// </summary>
        public string Composition { get; set; }
    }
}
