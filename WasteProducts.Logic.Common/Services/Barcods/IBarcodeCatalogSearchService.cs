using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    public interface IBarcodeCatalogSearchService
    {
        Barcode Get(string barcode);
    }
}
