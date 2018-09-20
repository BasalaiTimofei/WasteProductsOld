using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// Add new barcode in the repository.
        /// </summary>
        /// <param name="barcode">New barcode to add.</param>
        Task<string> AddAsync(BarcodeDB barcode);

        /// <summary>
        /// Update record of the barcode in the repository.
        /// </summary>
        /// <param name="barcode">New barcode to Update.</param>
        void Update(BarcodeDB barcode);

        /// <summary>
        /// Delete record of the barcode in the repository.
        /// </summary>
        /// <param name="id">ID of the barcode.</param>
        void DeleteById(string id);

        /// <summary>
        /// Delete record of the barcode in the repository.
        /// </summary>
        /// <param name="code">ID of the barcode.</param>
        void DeleteByCode(string code);
    }
}
