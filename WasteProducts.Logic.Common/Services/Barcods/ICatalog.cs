using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    public interface ICatalog
    {
        Barcode Get(string barcode);
    }
}
