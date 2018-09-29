using AutoMapper;
using Castle.Core.Logging;
using System.IO;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Repositories.Barcods;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    public class BarcodeService : IBarcodeService
    {
        IBarcodeScanService _scanner;
        IBarcodeCatalogSearchService _catalog;
        IBarcodeRepository _repository;
        ILogger _logger;
        IMapper _mapper;

        public BarcodeService(IServiceFactory serviceFactory, IBarcodeRepository repository, /*ILogger logger, */IMapper mapper)
        {
            _scanner = serviceFactory.CreateBarcodeScanService();
            _catalog = serviceFactory.CreateSearchBarcodeService();
            _repository = repository;
            //_logger = logger;
            _mapper = mapper;
        }

        public Task<string> AddAsync(Barcode barcode)
        {
            return Task.FromResult(_repository.AddAsync(Mapper.Map<BarcodeDB>(barcode)).Result); //mapping Barcode -> BarcodeDB 
        }

        public Task<Barcode> GetBarcodeByStreamAsync(Stream imageStream)
        {
            //получить цифровой код баркода
            var code = _scanner.Scan(imageStream);

            if (code == null)
                return null;

            //если получили валидный код - найти информацию о товаре в репозитории
            var barcodeDB = _repository.GetByCodeAsync(code).Result;

            //если она есть - вернуть ее
            if (barcodeDB != null)
                return Task.FromResult(_mapper.Map<Barcode>(barcodeDB));

            //если ее нет - получить инфу из веб каталога
            var barcode = _catalog.GetAsync(code).Result;

            if (barcode == null)
                return null;

            //сохранить ее в репозиторий
            //string res = _repository.AddAsync(Mapper.Map<BarcodeDB>(barcode)).Result; //mapping Barcode -> BarcodeDB 

            //вернуть ее
            return Task.FromResult(barcode);
        }

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

            //сохранить ее в репозиторий
            string res = _repository.AddAsync(Mapper.Map<BarcodeDB>(barcode)).Result; //mapping Barcode -> BarcodeDB 

            //вернуть ее
            return Task.FromResult(barcode);
        }
    }
}
