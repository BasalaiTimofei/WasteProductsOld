using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    public interface IBarcodeCatalogSearchService
    {
        Task<Barcode> GetAsync(string barcode);
    }
}
