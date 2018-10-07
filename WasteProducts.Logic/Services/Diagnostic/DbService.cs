using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Ninject.Extensions.Logging;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.DataAccess.Common.Repositories.Diagnostic;
using WasteProducts.Logic.Common.Models.Diagnostic;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Common.Services.Products;
using WasteProducts.Logic.Properties;
using WasteProducts.Logic.Resources;

namespace WasteProducts.Logic.Services
{
    /// <inheritdoc />
    public class DbService : IDbService
    {
        private readonly IDiagnosticRepository _diagRepo;
        private readonly IDatabase _database;
        private readonly IProductService _prodService;
        private readonly ILogger _logger;

        private bool _disposed = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbSeedService">IDbSeedService implementation that seeds into database</param>
        /// <param name="database">IDatabase implementation, for operations with database</param>
        /// <param name="prodService">IProductService implementation, for seeding DB with real product data.</param>
        /// <param name="logger">NLog logger</param>
        public DbService(IDiagnosticRepository diagRepo, IDatabase database, IProductService prodService, ILogger logger)
        {
            _diagRepo = diagRepo;
            _database = database;
            _prodService = prodService;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<DatabaseState> GetStateAsync()
        {
            bool isExist = await Task.FromResult(_database.IsExists).ConfigureAwait(false);
            bool isCompatibleWithModel = isExist && await Task.FromResult(_database.IsCompatibleWithModel).ConfigureAwait(false);

            if (isExist && !isCompatibleWithModel)
            {
                _logger.Warn(DbServiceResources.GetStatusAsync_WarnMsg);
            }

            return new DatabaseState(isExist, isCompatibleWithModel);
        }
        
        /// <inheritdoc />
        public Task DeleteAsync()
        {
            return Task.Run(()=> _database.Delete());
        }

        /// <inheritdoc />
        public Task RecreateAsync()
        {
            return _diagRepo.RecreateAsync();
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            foreach (var bitmap in GetListOfPhotos())
            {
                using (Stream stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Bmp);
                    await _prodService.AddAsync(stream);
                }
            }
            
            await _diagRepo.SeedAsync();
        }

        /// <inheritdoc />
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
                    _database.Dispose();
                    _diagRepo.Dispose();
                }

                _disposed = true;
            }
        }

        private Bitmap[] GetListOfPhotos()
        {
            return new Bitmap[]
            {
                BarcodePhotos.Barcode01,
                BarcodePhotos.Barcode02,
                BarcodePhotos.Barcode03,
                BarcodePhotos.Barcode04,
                BarcodePhotos.Barcode05,
                BarcodePhotos.Barcode06,
                BarcodePhotos.Barcode07,
                BarcodePhotos.Barcode08,
                BarcodePhotos.Barcode09,
            };
        }

        ~DbService()
        {
            Dispose(false);
        }
    }
}