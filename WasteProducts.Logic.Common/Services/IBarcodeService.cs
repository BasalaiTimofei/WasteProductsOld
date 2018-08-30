using System.Drawing;
using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services
{
    public interface IBarcodeService
    {
        /// <summary>
        /// Resize a image of barcode
        /// </summary>
        /// <param name="img"> image of barcode photo</param>
        /// <param name="width"> width of barcode image result</param>
        /// <param name="height"> height of barcode image result</param>
        /// <returns>Resized image</returns>
        Image Resize(Image img, int width, int height);

        /// <summary>
        /// get a numerical barcode on the photo
        /// </summary>
        /// <param name="image"> image of barcode photo</param>
        /// <returns>string of a numerical barcode</returns>
        BarcodeInfo ScanByZxing(Image image);

        /// <summary>
        /// get product information
        /// </summary>
        /// <param name="barcodeInfo">a model of BarcodeInfo</param>
        /// <returns>product information in the format Barcode</returns>
        //Barcode GetBarcode(BarcodeInfo barcodeInfo);
    }
}
