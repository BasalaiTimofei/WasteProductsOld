using AutoMapper;
using System.IO;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Repositories.Barcods;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    /// <inheritdoc />
    public class BarcodeService : IBarcodeService
    {
        private Barcode _barcode;
        IBarcodeScanService _scanner;
        IBarcodeCatalogSearchService _catalog;
        IBarcodeRepository _repository;
        IMapper _mapper;

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
            return Task.FromResult(_repository.AddAsync(_mapper.Map<BarcodeDB>(barcode)).Result); //mapping Barcode -> BarcodeDB 
        }

        /// <inheritdoc />
        public Task<Barcode> GetBarcodeByStreamAsync(Stream imageStream)
        {
            //получить цифровой код баркода
            var code = _scanner.Scan(imageStream);

            if (code == null)
                return Task.FromResult(_barcode);

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
                return Task.FromResult(_barcode);

            //вернуть ее
            return Task.FromResult(barcode);
        }
    }
}
