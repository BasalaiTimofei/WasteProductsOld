using System.Collections.Generic;
using WasteProducts.Logic.Common.Models.Product;

namespace WasteProducts.Logic.Common.Models.Caregory
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
        /// List of products that belong to a specific Сategory
        /// </summary>
        public List<Product> Products { get; set; }
    }
}
