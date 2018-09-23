using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Barcods;

namespace WasteProducts.DataAccess.Common.Repositories.Barcods
{
    /// <summary>
    /// This interface provides CRUD methods for barcode repository
    /// </summary>
    public interface IBarcodeRepository : IDisposable
    {
        /// <summary>
        /// Return the barcode by its numerical barcode.
        /// </summary>
        /// <param name="id">Id of the barcode.</param>
        /// <returns>Barcode with the specific ID.</returns>
        Task<BarcodeDB> GetByIdAsync(string id);

        /// <summary>
        /// Return the barcode by its numerical barcode.
        /// </summary>
        /// <param name="code">Code of the barcode.</param>
        /// <returns>Barcode with the specific code.</returns>
        Task<BarcodeDB> GetByCodeAsync(string code);

        /// <summary>
        /// Returns the entire list of records.
        /// </summary>
        /// <returns>A list of all barcodes.</returns>
        Task<IEnumerable<BarcodeDB>> SelectAllAsync();

        /// <summary>
        /// Add new barcode in the repository.
        /// </summary>
        /// <param name="barcode">New barcode to add.</param>
        /// <returns>string Id</returns>
        Task<string> AddAsync(BarcodeDB barcode);

        /// <summary>
        /// Add list barcode in the repository.
        /// </summary>
        /// <param name="barcodes">List barcodes to add.</param>
        /// <returns>List Barcode Id.</returns>
        Task<IEnumerable<string>> AddRangeAsync(IEnumerable<BarcodeDB> barcodes);

        /// <summary>
        /// Update record of the barcode in the repository.
        /// </summary>
        /// <param name="barcode">New barcode to Update.</param>
        Task UpdateAsync(BarcodeDB barcode);

        /// <summary>
        /// Delete record of the barcode in the repository.
        /// </summary>
        /// <param name="barcode">Barcode for delete.</param>
        void DeleteById(BarcodeDB barcode);
    }
}