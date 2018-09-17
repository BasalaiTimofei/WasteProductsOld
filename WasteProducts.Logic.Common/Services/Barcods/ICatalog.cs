using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    public interface ICatalog
    {
        CatalogProductInfo Get(string barcode);
    }
}
