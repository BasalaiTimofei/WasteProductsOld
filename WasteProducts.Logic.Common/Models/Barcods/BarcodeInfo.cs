namespace WasteProducts.Logic.Common.Services
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
        /// User ID of the sending photo.
        /// </summary>
        public int UserID { get; set; }      
    }
}
