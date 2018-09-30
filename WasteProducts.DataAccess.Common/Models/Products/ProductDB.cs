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
        /// Unique identifier of concrete Product in database.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Unique name of concrete Product in database.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product brend.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Product country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Product weight.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Defines the Product description
        /// </summary>
        public string Composition { get; set; }

        /// <summary>
        /// Product picture path.
        /// </summary>
        public string PicturePath { get; set; }

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
        /// User descriptions of this product.
        /// </summary>
        public virtual ICollection<UserProductDescriptionDB> UserDescriptions { get; set; }

        /// <summary>
        /// Defines whether the Product is marked for deletion
        /// </summary>
        public bool Marked { get; set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns>Returns true if the specified object is equal to the current object; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            return obj is ProductDB other &&
                   this.Name == other.Name &&
                   this.Id == other.Id &&
                   this.Created == other.Created &&
                   this.Modified == other.Modified &&
                   this.Category == other.Category &&
                   this.Barcode == other.Barcode;
        }

        /// <summary>
        /// The hash code for this ProductDB
        /// </summary>
        /// <returns>A hash code for the current object</returns>
        public override int GetHashCode()
        {
            var hashCode = -1413941165;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Created.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTime?>.Default.GetHashCode(Modified);
            hashCode = hashCode * -1521134295 + EqualityComparer<CategoryDB>.Default.GetHashCode(Category);
            hashCode = hashCode * -1521134295 + EqualityComparer<BarcodeDB>.Default.GetHashCode(Barcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<ICollection<UserProductDescriptionDB>>.Default.GetHashCode(UserDescriptions);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Composition);
            hashCode = hashCode * -1521134295 + Marked.GetHashCode();
            return hashCode;
        }
    }
}
