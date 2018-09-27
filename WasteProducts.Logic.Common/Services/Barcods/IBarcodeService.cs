using System.IO;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    public interface IBarcodeService
    {
        /// <summary>
        /// Scan photo of barcode and return a model of Barcode.
        /// </summary>
        /// <param name="stream">Photo stream barcode.</param>
        /// <returns>Model of Barcode.</returns>
        Task<Barcode> GetBarcodeAsync(Stream stream);
    }
}
