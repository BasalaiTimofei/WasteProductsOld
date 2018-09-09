using WasteProducts.Logic.Common.Attributes;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Services.Diagnostic
{
    /// <summary>
    /// Generates test models for seeding
    /// </summary>
    [Trace]
    public interface ITestModelsService
    {
        /// <summary>
        /// Generate User model
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="userName">username</param>
        /// <returns>User</returns>
        User GenerateUser(string email = null, string userName = null);

        /// <summary>
        /// Generate Product model
        /// </summary>
        /// <param name="barcode">product barcode</param>
        /// <param name="category">product category</param>
        /// <returns>Product</returns>
        Product GenerateProduct(Barcode barcode, Category category);

        /// <summary>
        /// Generate Category model
        /// </summary>
        /// <returns>Category</returns>
        Category GenerateCategory();

        /// <summary>
        /// Generate Barcode model
        /// </summary>
        /// <returns>Barcode</returns>
        Barcode GenerateBarcode();
    }
}