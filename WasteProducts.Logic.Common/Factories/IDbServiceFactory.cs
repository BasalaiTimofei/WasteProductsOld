using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Services.UserService;

namespace WasteProducts.Logic.Common.Factories
{

    /// <summary>
    /// Proxy Ninject factory interface for services
    /// </summary>
    public interface IDbServiceFactory
    {
        /// <summary>
        /// Gets ISearchService from Ioc container 
        /// </summary>
        /// <returns>Implementation of ISearchService</returns>
        ISearchService CreateSearchService();

        /// <summary>
        /// Gets IUserService from Ioc container 
        /// </summary>
        /// <returns>Implementation of IUserService</returns>
        IUserService CreateUserService();

        /// <summary>
        /// Gets IUserRoleService from Ioc container 
        /// </summary>
        /// <returns>Implementation of IUserRoleService</returns>
        IUserRoleService CreateRoleService();

        /// <summary>
        /// Gets IProductService from Ioc container 
        /// </summary>
        /// <returns>Implementation of IProductService</returns>
        IProductService CreateProductService();

        /// <summary>
        /// Gets IBarcodeService from Ioc container 
        /// </summary>
        /// <returns>Implementation of IBarcodeService</returns>
        IBarcodeService CreateBarcodeService();

        /// <summary>
        /// Gets IDonationService from Ioc container 
        /// </summary>
        /// <returns>Implementation of IDonationService</returns>
        IDonationService CreateDonationService();
    }
}