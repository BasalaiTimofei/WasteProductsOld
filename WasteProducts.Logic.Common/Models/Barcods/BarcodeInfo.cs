namespace WasteProducts.Logic.Common.Models.Barcods
{
    public class BarcodeInfo
    {
        /// <summary>
        /// Barcode number.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Barcode type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// User Id of the sending photo.
        /// </summary>
        public int UserId { get; set; }      
    }
}
