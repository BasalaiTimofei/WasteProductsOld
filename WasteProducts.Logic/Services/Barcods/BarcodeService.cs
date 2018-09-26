using AutoMapper;
using Castle.Core.Logging;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Barcods;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    /// <inheritdoc />
    public class BarcodeService : IBarcodeService
    {
        private Bitmap _image;
        private Stream _stream;
        private readonly IBarcodeScanService _scanner;
        private readonly IBarcodeCatalogSearchService _searcher;
        private readonly IBarcodeRepository _repository;
        private readonly IBarcodeFactory _barcodeFactory;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="barcodeFactory">IBarcodeFactory implementation IBarcodeScanService,
        /// IBarcodeCatalogSearchService, IBarcodeRepository</param>
        /// <param name="logger">NLog logger</param>
        /// <param name="mapper">AutoMapperr</param>
        public BarcodeService(IBarcodeFactory barcodeFactory, ILogger logger, IMapper mapper)
        {
            _barcodeFactory = barcodeFactory;
            _scanner = _barcodeFactory.CreateScanService();
            _searcher = _barcodeFactory.CreateSearchService();
            _repository = _barcodeFactory.CreateRepository();
            _logger = logger;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public Task<string> AddAsync(Barcode barcode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Barcode> GetBarcodeByStreamAsync(Stream stream)
        {
            string code = null;
            Barcode barcode = null;

            _image = _scanner.Resize(stream, 400, 400);
            using (_stream = new MemoryStream())
            {
                _image.Save(_stream, ImageFormat.Bmp);
                try
                {
                    code = _scanner.ScanByZxing(_stream);
                }
                catch
                {
                    code = _scanner.ScanBySpire(_stream);
                }
            }
            if (code.Length == 8 || code.Length == 4)
            {
                if (await GetByCodeAsync(code) != null)
                {
                    barcode = await _searcher.GetAsync(code);
                }
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
