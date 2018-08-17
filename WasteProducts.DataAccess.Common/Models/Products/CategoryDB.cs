using System.Collections.Generic;

namespace WasteProducts.DataAccess.Common.Models.Products
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
        /// Contains description of a specific category
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of products that belong to a specific Ñategory in database
        /// </summary>
        public ICollection<ProductDB> Products { get; set; }
    }
}
