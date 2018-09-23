using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    /// <inheritdoc />
    public class BarcodeCatalogSearchService: IBarcodeCatalogSearchService
    {
        IEnumerable<ICatalog> _catalogs;
        IHttpHelper _httpHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="catalogs">List catalogs</param>
        public BarcodeCatalogSearchService(IEnumerable<ICatalog> catalogs)
        {
            _catalogs = catalogs;
            _httpHelper = new HttpHelper();
        }

        /// <inheritdoc />
        public async Task<Barcode> GetAsync(string barcode)
        {
            foreach(var catalog in _catalogs)
            {
                var productInfo = await catalog.GetAsync(barcode);

                if(productInfo != null)
                {
                    return productInfo;
                }
            }

            return null;
        }
    }
}
