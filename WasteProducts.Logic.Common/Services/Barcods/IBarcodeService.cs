using System.IO;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    /// <summary>
    /// This interface provides barcodes methods.
    /// </summary>
    public interface IBarcodeService
    {
        /// <summary>
        /// Return the barcode by its numerical barcode.
        /// </summary>
        /// <param name="code">Code of the barcode.</param>
        /// <returns>Barcode with the specific code.</returns>
        Task<Barcode> GetByCodeAsync(string code);

        /// <summary>
        /// Add new barcode in the repository.
        /// </summary>
        /// <param name="barcode">New barcode to add.</param>
        /// <returns>string Id</returns>
        Task<string> AddAsync(Barcode barcode);

        /// <summary>
        /// Scan photo of barcode and return a model of Barcode.
        /// </summary>
        /// <param name="stream">Photo stream barcode.</param>
        /// <returns>Model of Barcode.</returns>
        Task<Barcode> GetBarcodeAsync(Stream stream);

        /// <summary>
        /// Scan photo of barcode and return a model of Barcode.
        /// </summary>
        /// <param name="stream">Photo stream barcode.</param>
        /// <returns>Model of Barcode.</returns>
        Task<Barcode> GetBarcodeByCodeAsync(string code);
    }
}
