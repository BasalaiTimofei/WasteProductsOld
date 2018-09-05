using System;
using WasteProducts.DataAccess.Common.Models.Products;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    /// <summary>
    /// Description of the specific product by the specific user.
    /// </summary>
    public class UserProductDescriptionDB
    {
        /// <summary>
        /// Id of User who set this description on the product.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// User who set this description on the product.
        /// </summary>
        public virtual UserDB User { get; set; }

        /// <summary>
        /// Id of Product of this description.
        /// </summary>
        public virtual string ProductId { get; set; }

        /// <summary>
        /// Product of this description.
        /// </summary>
        public virtual ProductDB Product { get; set; }

        /// <summary>
        /// Rating of this product pescription.
        /// </summary>
        public virtual int Rating { get; set; }

        /// <summary>
        /// Description contains opinion of the user about the product.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Specifies timestamp of creation of this entity in Database.
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Specifies timestamp of modifying of this entity in Database.
        /// </summary>
        public virtual DateTime? Modified { get; set; }
    }
}
