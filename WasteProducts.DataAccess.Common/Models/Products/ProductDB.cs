using System;
using WasteProducts.DataAccess.Common.Models.Barcods;

namespace WasteProducts.DataAccess.Common.Models.Products
{
    /// <summary>
    /// Model for entity Product used in database.
    /// </summary>
    public class ProductDB
    {
        /// <summary>
        /// Unique identifier of concrete Product in database.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Unique name of concrete Product in database.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the timestamp of creation of concrete Product in database.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Specifies the timestamp of modifying of any property of the Product in database.
        /// </summary>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// Specifies the Product category.
        /// </summary>
        public virtual CategoryDB Category { get; set; }

        /// <summary>
        /// Defines the Product barcode.
        /// </summary>
        public virtual BarcodeDB Barcode { get; set; }

        /// <summary>
        /// Defines the average Product rating based on user ratings.
        /// </summary>
        public double? AvgRating { get; set; }

        /// <summary>
        /// Defines the price of the Product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Defines the number of users who have rated the Product. Is used to determine the property "AvgRating".
        /// </summary>
        public int RateCount { get; set; }

        /// <summary>
        /// Defines the Product description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Defines whether the Product is in the "hidden" state
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Defines whether the Product is marked for deletion
        /// </summary>
        public bool Marked { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ProductDB other &&
                   this.Name == other.Name &&
                   this.AvgRating == other.AvgRating &&
                   this.Id == other.Id &&
                   this.Created == other.Created &&
                   this.Modified == other.Modified &&
                   this.Category == other.Category &&
                   this.Barcode == other.Barcode &&
                   this.Price == other.Price &&
                   this.RateCount == other.RateCount;
        }
    }
}
