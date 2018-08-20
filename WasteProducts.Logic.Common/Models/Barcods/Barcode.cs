namespace WasteProducts.Logic.Common.Models.Barcods
{
    public class Barcode
    {
        /// <summary>
        /// Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Barcode number.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Barcode type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Product brend.
        /// </summary>
        public string Brend { get; set; }

        /// <summary>
        /// Product country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Product weight.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// User Id of the sending photo.
        /// </summary>
        public int UserId { get; set; }
    }
}
