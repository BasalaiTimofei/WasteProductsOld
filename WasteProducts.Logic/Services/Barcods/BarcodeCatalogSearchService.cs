using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    public class BarcodeCatalogSearchService: IBarcodeCatalogSearchService
    {
        IEnumerable<ICatalog> _catalogs;
        IHttpHelper _httpHelper;

        public BarcodeCatalogSearchService(IEnumerable<ICatalog> catalogs)
        {
            _catalogs = catalogs;
            _httpHelper = new HttpHelper();
        }

        public CatalogProductInfo Get(string barcode)
        {
            foreach(var catalog in _catalogs)
            {
                var productInfo = catalog.Get(barcode);

                if(productInfo != null)
                {
                    return productInfo;
                }
            }

            return null;
        }
    }
}
