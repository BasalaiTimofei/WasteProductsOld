namespace WasteProducts.Logic.Common.Models.Product
{
    /// <summary>
    /// Model for entity Product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique name of concrete Product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines the Product category.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Defines the Product barcode.
        /// </summary>
        public Barcode Barcode { get; set; }

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
        /// Defines the product description
        /// </summary>
        public string Description { get; set; }
    }
}
