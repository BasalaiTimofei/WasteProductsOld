using System.Drawing;
using System.IO;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    /// <summary>
    /// This interface provides barcodes methods.
    /// </summary>
    public interface IBarcodeScanService
    {
        /// <summary>
        /// Resize a image of barcode
        /// </summary>
        /// <param name="img"> image of barcode photo</param>
        /// <param name="width"> width of barcode image result</param>
        /// <param name="height"> height of barcode image result</param>
        /// <returns>Resized image</returns>
        Bitmap Resize(Stream stream, int width, int height);

        /// <summary>
        /// get a numerical barcode on the photo
        /// </summary>
        /// <param name="stream"> image of barcode photo</param>
        /// <returns>string of a numerical barcode</returns>
        string ScanByZxing(Stream stream);

        /// <summary>
        /// get a numeric barcode from the photo
        /// </summary>
        /// <param name="stream"> image of barcode photo</param>
        /// <returns>string of a numerical barcode</returns>
        string ScanBySpire(Stream stream);
    }
}
