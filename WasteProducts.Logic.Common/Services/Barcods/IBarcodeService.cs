using System.IO;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    /// <summary>
    /// This interface provides barcodes methods.
    /// </summary>
    public interface IBarcodeService
    {
        /// <summary>
        /// Add new barcode in the repository.
        /// </summary>
        /// <param name="barcode">New barcode to add.</param>
        /// <returns>string Id</returns>
        Task<string> AddAsync(Barcode barcode);

        /// <summary>
        /// Parses photo of barcode and returns its digit representation.
        /// </summary>
        /// <param name="imageStream">Stream of the photo.</param>
        /// <returns>Code of the barcode.</returns>
        string ParseBarcodePhoto(Stream imageStream);

        /// <summary>
        /// Gets BarcodeDB by its code.
        /// </summary>
        /// <param name="code">Code of product.</param>
        /// <returns>BarcodeDB entity.</returns>
        Task<BarcodeDB> GetBarcodeFromDBAsync(string code);

        /// <summary>
        /// Gets barcode and information about product from product catalog by code.
        /// </summary>
        /// <param name="code">Code of product.</param>
        /// <returns>Barcode entity.</returns>
        Task<Barcode> GetBarcodeFromCatalogAsync(string code);

        /// <summary>
        ///  Return a model of Barcode by code.
        /// </summary>
        /// <param name="stream">Photo stream barcode.</param>
        /// <returns>Model of Barcode.</returns>
        Task<Barcode> GetBarcodeByCodeAsync(string code);
    }
}
