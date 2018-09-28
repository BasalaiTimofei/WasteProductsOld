using AutoMapper;
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

        public BarcodeService(IBarcodeScanService scanner, IBarcodeRepository repository, IBarcodeCatalogSearchService catalog)
        {
            _scanner = scanner;
            _repository = repository;
            _catalog = catalog;
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
            {
                var map1 = Mapper.Map<Barcode>(barcodeDB);
                return map1;
            }

            //если ее нет - получить инфу из веб каталога
            var barcode = _catalog.GetAsync(code).Result;

            if (barcode == null)
                return null;

            //сохранить ее в репозиторий
            var map2 = Mapper.Map<BarcodeDB>(barcode);

            string res = _repository.AddAsync(map2).Result; //mapping Barcode -> BarcodeDB 

            //вернуть ее
            return barcode;
        }
    }
}
