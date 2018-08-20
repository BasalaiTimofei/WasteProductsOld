using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Models.Products
{
    /// <summary>
    /// Model for entity Product used in database.
    /// </summary>
    public class ProductDB
    {
        /// <summary>
        /// Unique name of concrete Product in database.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Unique identifier of concrete Product in database.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Specifies the timestamp of creation of concrete Product in database.
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Specifies the timestamp of modifying of any property of the Product in database.
        /// </summary>
        public virtual DateTime? Modified { get; set; }

        /// <summary>
        /// Specifies the Product category.
        /// </summary>
        public virtual CategoryDB CategoryDB { get; set; }

        /// <summary>
        /// Defines the Product barcode.
        /// </summary>
        public virtual BarcodeDB Barcode { get; set; }

        /// <summary>
        /// Defines the average Product rating based on user ratings.
        /// </summary>
        public virtual double? AvgRating { get; set; }

        /// <summary>
        /// Defines the price of the Product.
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// Defines the number of users who have rated the Product. Is used to determine the property "AvgRating".
        /// </summary>
        public virtual int RateCount { get; set; }

        /// <summary>
        /// Users having this product in their lists
        /// </summary>
        public virtual ICollection<UserDB> Users { get; set; }

        /// <summary>
        /// Defines the product description
        /// </summary>
        public virtual string Description { get; set; }
    }
}
