namespace WasteProducts.Logic.Common.Models.Product
{
    public class Product
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public Barcode Barcode { get; set; }
        public double? AvgRate { get; set; }
        public decimal Price { get; set; }
        public int RateCount { get; set; }
        /// <summary>
        /// Defines the product description
        /// </summary>
        public string Description { get; set; }
    }
}
