using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Repositories.Barcods;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.Barcods
{
    /// <summary>
    ///This class is a context class. A binder for the 'BarrcodeDB' class with a data access.
    /// </summary>
    public class BarcodeRepository : IBarcodeRepository
    {
        private readonly WasteContext _wasteContext;
        private bool _disposed;

        /// <summary>
        /// Using the context of the WasteContext class through the private field.
        /// </summary>
        /// <param name="wasteContext">The specific context of WasteContext</param>
        public BarcodeRepository(WasteContext wasteContext)
        {
            _wasteContext = wasteContext;
        }
        /// <summary>
        /// Return the barcode by its numerical barcode.
        /// </summary>
        /// <param name="id">Id of the barcode.</param>
        /// <returns>Barcode with the specific ID.</returns>
        public async Task<BarcodeDB> GetByIdAsync(string id)
        {
            var barcode = await _wasteContext.Barcodes.FirstOrDefaultAsync(b => b.Id == id);
            return barcode;
        }

        /// <summary>
        /// Return the barcode by its numerical barcode.
        /// </summary>
        /// <param name="code">Code of the barcode.</param>
        /// <returns>Barcode with the specific code.</returns>
        public async Task<BarcodeDB> GetByCodeAsync(string code)
        {
            var barcode = await _wasteContext.Barcodes.SingleOrDefaultAsync(c => c.Code == code);
            return barcode;
        }

        /// <summary>
        /// Returns the entire list of records.
        /// </summary>
        /// <returns>A list of all barcodes.</returns>
        public async Task<IEnumerable<BarcodeDB>> SelectAllAsync()
        {
            return await Task.Run(() =>
            {
                return _wasteContext.Barcodes.ToList();
            });
        }

        /// <summary>
        /// Add new barcode in the repository.
        /// </summary>
        /// <param name="barcode">New barcode to add.</param>
        public async Task<string> AddAsync(BarcodeDB barcode)
        {
            barcode.Id = Guid.NewGuid().ToString();
            barcode.Created = DateTime.UtcNow;
            _wasteContext.Barcodes.Add(barcode);

            await _wasteContext.SaveChangesAsync();

            return barcode.Id;
        }

        /// <summary>
        /// Update record of the barcode in the repository.
        /// </summary>
        /// <param name="barcode">New barcode to Update.</param>
        public async Task UpdateAsync(BarcodeDB barcode)
        {
            _wasteContext.Entry(barcode).State = EntityState.Modified;
            var barcodeDB = await _wasteContext.Barcodes.FirstOrDefaultAsync(b => b.Id == barcode.Id);
            barcodeDB.Modified = DateTime.UtcNow;
            await _wasteContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete record of the barcode in the repository.
        /// </summary>
        /// <param name="id">ID of the barcode.</param>
        public void DeleteById(BarcodeDB barcode)
        {
            if (barcode != null)  _wasteContext.Barcodes.Remove(barcode);
            _wasteContext.SaveChanges();
        }

        /// <summary>
        /// This method calls if the data context means release or closing of connections.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _wasteContext.Dispose();
                }

                _disposed = true;
            }
        }
    }
}




