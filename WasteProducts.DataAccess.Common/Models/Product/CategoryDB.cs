using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Product;

namespace WasteProducts.DataAccess.Common.Models.Category
{
    /// <summary>
    /// Model for entity Category used in database.
    /// </summary>
    public class CategoryDB
    {
        /// <summary>
        /// Unique identifier of concrete Category in database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique name of concrete Category in database.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of products that belong to a specific Сategory in database
        /// </summary>
        public List<ProductDB> Product { get; set; }
    }
}
