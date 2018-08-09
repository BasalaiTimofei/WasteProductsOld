using System;

namespace WasteProducts.DataAccess.Common.Models.Product
{
    public class ProductDB
    {
        public string Name { get; set; }
        public string Id { get; }
        public DateTime Created { get; }
        public DateTime Updated { get; }
        public Category Category { get; set; }
        public Barcode Barcode { get; set; }
        public double? AvgRate { get; set; }
        public decimal Price { get; }
        public int RateCount { get; set; }
        /// <summary>
        /// Defines the product description
        /// </summary>
        public string Description { get; set; }
    }
}
