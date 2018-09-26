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
        /// Tries to add a new product by barcode.
        /// </summary>
        /// <param name="barcode">Barcode of the product to be added.</param>
        /// <returns>Represents added products's id.</returns>
        Task<string> Add(Stream imageStream);

        /// <summary>
        /// Gets the product by its id.
        /// </summary>
        /// <param name="id">The id of the product.</param>
        /// <returns>The product with the specific id.</returns>
        Task<Product> GetById(string id);

        /// <summary>
        /// Gets the product by its barcode.
        /// </summary>
        /// <param name="barcode">The barcode of the product.</param>
        /// <returns>The product with the specific barcode.</returns>
        Task<Product> GetByBarcode(Barcode barcode);

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>All products in the application.</returns>
        Task<IEnumerable<Product>> GetAll();

        /// <summary>
        /// Gets product by its name.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <returns>Product with the specific name.</returns>
        Task<Product> GetByName(string name);

        /// <summary>
        /// Gets products by a category.
        /// </summary>
        /// <param name="category">Category of the product.</param>
        /// <returns>Products belonging to the specific category.</returns>
        Task<IEnumerable<Product>> GetByCategory(Category category);

        /// <summary>
        /// Deletes product from Database.
        /// </summary>
        /// <param name="productId">Id of the product that should be deleted.</param>
        Task Delete(string productId);

        /// <summary>
        /// Tries to add the product to specific category.
        /// </summary>
        /// <param name="product">The specific product to add category.</param>
        /// <param name="category">The specific category to be added.</param>
        Task AddToCategory(string productId, string categoryId);

        /// <summary>
        /// Updates product if been modyfied.
        /// </summary>
        /// <param name="product">The specific product for updating.</param>
        Task Update(Product product);
    }
}
