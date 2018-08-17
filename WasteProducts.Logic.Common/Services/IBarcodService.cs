using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services
{
    public interface IBarcodService
    {
        /// <summary>
        /// get a numerical barcode on the photo
        /// </summary>
        /// <param name="path"> path to barcode photo</param>
        /// <param name="UserId"> User Id by User</param>
        /// <returns>a numerical barcode</returns>
        BarcodeInfo GetCode(string path, int UserId);

        /// <summary>
        /// get product information
        /// </summary>
        /// <param name="barcodeInfo">a model of BarcodeInfo</param>
        /// <returns>product information in the format Barcode</returns>
        Barcode GetBarcode(BarcodeInfo barcodeInfo);
    }
}
