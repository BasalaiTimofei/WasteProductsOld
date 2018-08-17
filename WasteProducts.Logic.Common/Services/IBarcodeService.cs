using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services
{
    public interface IBarcodeService
    {
        /// <summary>
        /// get a numerical barcode on the photo
        /// </summary>
        /// <param name="filePath"> path to barcode photo</param>
        /// <param name="userId"> User Id by User</param>
        /// <returns>a numerical barcode</returns>
        BarcodeInfo GetCode(string filePath, int userId);

        /// <summary>
        /// get product information
        /// </summary>
        /// <param name="barcodeInfo">a model of BarcodeInfo</param>
        /// <returns>product information in the format Barcode</returns>
        Barcode GetBarcode(BarcodeInfo barcodeInfo);
    }
}
