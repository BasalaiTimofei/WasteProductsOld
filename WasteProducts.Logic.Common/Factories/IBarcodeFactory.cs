using WasteProducts.DataAccess.Common.Repositories.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Common.Factories
{
    public interface IBarcodeFactory
    {
        /// <summary>
        /// Gets IBarcodeScanService from Ioc container 
        /// </summary>
        /// <returns>Implementation of IBarcodeScanService</returns>
        IBarcodeScanService CreateScanService();

        /// <summary>
        /// Gets IBarcodeCatalogSearchService from Ioc container 
        /// </summary>
        /// <returns>Implementation of IBarcodeCatalogSearchService</returns>
        IBarcodeCatalogSearchService CreateSearchService();

        /// <summary>
        /// Gets IBarcodeRepository from Ioc container 
        /// </summary>
        /// <returns>Implementation of IBarcodeRepository</returns>
        IBarcodeRepository CreateRepository();
    }
}
