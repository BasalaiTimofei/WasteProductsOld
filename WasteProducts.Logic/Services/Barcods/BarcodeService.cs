using AutoMapper;
using Castle.Core.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Barcods;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    /// <inheritdoc />
    public class BarcodeService : IBarcodeService
    {
        private readonly IBarcodeScanService _scanner;
        private readonly IBarcodeCatalogSearchService _searcher;
        private readonly IBarcodeRepository _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public BarcodeService(IBarcodeScanService scanner, IBarcodeCatalogSearchService searcher, IBarcodeRepository repository, ILogger logger, IMapper mapper)
        {
            _scanner = scanner;
            _searcher = searcher;
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <inheritdoc />
        public Task<string> AddAsync(Barcode barcode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Barcode> GetBarcodeAsync(Stream stream)
        {
            string code = "";
            Barcode barcode = null;

            code = _scanner.ScanBySpire(stream);
            if (await GetByCodeAsync(code) != null)
            {
                barcode = await _searcher.GetAsync(code);
            }
            return barcode;
        }

        /// <inheritdoc />
        public async Task<Barcode> GetBarcodeByCodeAsync(string code)
        {
            Barcode barcode = null;

            if (await GetByCodeAsync(code) != null)
            {
                barcode = await _searcher.GetAsync(code);
            }
            return barcode;
        }

        /// <inheritdoc />
        public async Task<Barcode> GetByCodeAsync(string code)
           => _mapper.Map<Barcode>(await _repository.GetByCodeAsync(code));
    }
}
