using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Product;

namespace WasteProducts.DataAccess.Common.Models.Product
{
    /// <summary>
    /// Model for entity Product used in database.
    /// </summary>
    public class ProductDB
    {
        /// <summary>
        /// Unique name of concrete Product in database.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier of concrete Product in database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Specifies the timestamp of creation of concrete Product in database.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Specifies the timestamp of modifying of any property of the Product in database.
        /// </summary>
        public DateTime? Updated { get; set; }

        /// <summary>
        /// Specifies the Product category.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Defines the Product barcode.
        /// </summary>
        public Barcode Barcode { get; set; }

        /// <summary>
        /// Defines the average Product mark based on user ratings.
        /// </summary>
        public double? AvgRate { get; set; }

        /// <summary>
        /// Defines the price of the Product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Defines the number of users who have rated the Product. Is used to determine the property "AvgMark".
        /// </summary>
        public int RateCount { get; set; }

        /// <summary>
        /// Defines the product description
        /// </summary>
        public string Description { get; set; }
    }
}
