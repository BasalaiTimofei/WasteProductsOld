using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Services.Products
{
    /// <summary>
    /// This interface provides product methods.
    /// </summary>
    public interface IProductService : IDisposable
    {
        /// <summary>
        /// Tries to add a new product by stream from input image with barcode.
        /// </summary>
        /// <param name="imageStream">Stream from input image.</param>
        /// <returns>Represents added products's id.</returns>
        Task<string> AddAsync(Stream imageStream);

        /// <summary>
        /// Tries to add a new product by name.
        /// </summary>
        /// <param name="name">Name of the product to be added.</param>
        /// <returns>Represents added products's id.</returns>
        Task<string> AddAsync(string name);

        /// <summary>
        /// Gets the product by its id.
        /// </summary>
        /// <param name="id">The id of the product.</param>
        /// <returns>The product with the specific id.</returns>
        Task<Product> GetByIdAsync(string id);

        /// <summary>
        /// Gets the product by its barcode.
        /// </summary>
        /// <param name="barcode">The barcode of the product.</param>
        /// <returns>The product with the specific barcode.</returns>
        Task<Product> GetByBarcodeAsync(Barcode barcode);

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>All products in the database.</returns>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// Gets product by its name.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <returns>Product with the specific name.</returns>
        Task<Product> GetByNameAsync(string name);

        /// <summary>
        /// Gets products by a category.
        /// </summary>
        /// <param name="category">Category of the product.</param>
        /// <returns>Products belonging to the specific category.</returns>
        Task<IEnumerable<Product>> GetByCategoryAsync(Category category);

        /// <summary>
        /// Deletes product from Database.
        /// </summary>
        /// <param name="productId">Id of the product that should be deleted.</param>
        Task DeleteAsync(string productId);

        /// <summary>
        /// Tries to add the product to specific category.
        /// </summary>
        /// <param name="product">The specific product to add category.</param>
        /// <param name="category">The specific category to be added.</param>
        Task AddToCategoryAsync(string productId, string categoryId);

        /// <summary>
        /// Updates product if been modyfied.
        /// </summary>
        /// <param name="product">The specific product for updating.</param>
        Task UpdateAsync(Product product);
    }
}
