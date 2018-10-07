using AutoMapper;
using System;
using System.IO;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Repositories.Barcods;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    /// <inheritdoc />
    public class BarcodeService : IBarcodeService
    {
        private readonly IBarcodeScanService _scanner;
        private readonly IBarcodeCatalogSearchService _catalog;
        private readonly IBarcodeRepository _repository;
        private readonly IMapper _mapper;

        private bool _disposed;

        public BarcodeService(IServiceFactory serviceFactory, IBarcodeRepository repository, IMapper mapper)
        {
            _scanner = serviceFactory.CreateBarcodeScanService();
            _catalog = serviceFactory.CreateSearchBarcodeService();
            _repository = repository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public Task<string> AddAsync(Barcode barcode)
        {
            return _repository.AddAsync(_mapper.Map<BarcodeDB>(barcode));
        }

        /// <inheritdoc />
        public string ParseBarcodePhoto(Stream imageStream)
        {
            return _scanner.Scan(imageStream);
        }

        /// <inheritdoc />
        public Task<BarcodeDB> GetBarcodeFromDBAsync(string code)
        {
            return _repository.GetByCodeAsync(code);
        }

        /// <inheritdoc />
        public Task<Barcode> GetBarcodeFromCatalogAsync(string code)
        {
            return _catalog.GetAsync(code);
        }

        /// <inheritdoc />
        public Task<Barcode> GetBarcodeByCodeAsync(string code)
        {
            //если получили валидный код - найти информацию о товаре в репозитории
            var barcodeDB = _repository.GetByCodeAsync(code).Result;

            //если она есть - вернуть ее
            if (barcodeDB != null)
                return Task.FromResult(_mapper.Map<Barcode>(barcodeDB));

            //если ее нет - получить инфу из веб каталога
            var barcode = _catalog.GetAsync(code).Result;

            if (barcode == null)
                return null;

            //вернуть ее
            return Task.FromResult(barcode);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                this._repository.Dispose();
                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }

        ~BarcodeService()
        {
            this.Dispose();
        }
    }
}
