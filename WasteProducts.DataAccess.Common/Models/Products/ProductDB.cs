﻿using System;
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
        /// Users having this product in their lists.
        /// </summary>
        public virtual ICollection<UserDB> Users { get; set; }

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
                   Name == other.Name &&
                   AvgRating == other.AvgRating &&
                   Id == other.Id &&
                   Created == other.Created &&
                   Modified == other.Modified &&
                   Category == other.Category &&
                   Barcode == other.Barcode &&
                   Price == other.Price &&
                   RateCount == other.RateCount;
        }

        public override int GetHashCode()
        {
            var hashCode = Id.GetHashCode();
            return hashCode;
        }
    }
}
