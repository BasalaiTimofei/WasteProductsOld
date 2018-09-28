using AutoMapper;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Repositories.Barcods;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    public class BarcodeService : IBarcodeService
    {
        IBarcodeScanService _scanner;
        IBarcodeRepository _repository;
        IBarcodeCatalogSearchService _catalog;
        ILogger _logger;
        IMapper _mapper;

        public BarcodeService(IBarcodeScanService scanner, IBarcodeRepository repository, IBarcodeCatalogSearchService catalog, ILogger logger, IMapper mapper)
        {
            _scanner = scanner;
            _repository = repository;
            _catalog = catalog;
            _logger = logger;
            _mapper = mapper;
        }

        public Barcode Get(Stream imageStream)
        {
            //получить цифровой код баркода
            var code = _scanner.Scan(imageStream);

            if (code == null)
                return null;

            //если получили валидный код - найти информацию о товаре в репозитории
            var barcodeDB = _repository.GetByCodeAsync(code).Result;

            //если она есть - вернуть ее
            if (barcodeDB != null)
                return Mapper.Map<Barcode>(barcodeDB);

            //если ее нет - получить инфу из веб каталога
            var barcode = _catalog.GetAsync(code).Result;

            if (barcode == null)
                return null;

            //сохранить ее в репозиторий
            string res = _repository.AddAsync(Mapper.Map<BarcodeDB>(barcode)).Result; //mapping Barcode -> BarcodeDB 

            //вернуть ее
            return barcode;
        }
    }
}
