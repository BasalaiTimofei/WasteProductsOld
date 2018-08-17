using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Models.Products
{
    /// <summary>
    /// Model for entity Category.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Unique identifier of concrete Category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contains description of a specific Category
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of products that belong to a specific Ñategory
        /// </summary>
        public ICollection<Product> Products { get; set; }
    }
}
