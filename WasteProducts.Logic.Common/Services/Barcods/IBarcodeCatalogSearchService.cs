using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    /// <summary>
    /// This interface provides barcodes methods.
    /// </summary>
    public interface IBarcodeCatalogSearchService
    {
        /// <summary>
        /// Gets product info.
        /// </summary>
        /// <param name="barcode">String code.</param>
        /// <returns>Model of Barcode</returns>
        Task<Barcode> GetAsync(string barcode);
    }
}
