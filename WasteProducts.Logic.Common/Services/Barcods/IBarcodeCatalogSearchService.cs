using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    public interface IBarcodeCatalogSearchService
    {
        CatalogProductInfo Get(string barcode);
    }
}
