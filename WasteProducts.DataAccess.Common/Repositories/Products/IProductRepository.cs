using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Models.Products;

namespace WasteProducts.DataAccess.Common.Repositories.Products
{
    /// <summary>
    /// Interface for the ProductRepository. Has an inheritance branch from IDisposable.
    /// </summary>
    public interface IProductRepository : IDisposable
    {
        /// <summary>
        /// Adding new product
        /// </summary>
        /// <param name="product">The specific product for adding</param>
        ///// <param name="barcode">Barcode of the specific product for adding</param>
        /// <returns>Id of the added product.</returns>
        Task<string> AddAsync(ProductDB product/*, BarcodeDB barcode*/);

        /// <summary>
        /// Deleting the specific product
        /// </summary>
        /// <param name="product">The specific product for deleting.</param>
        Task DeleteAsync(ProductDB product);

        /// <summary>
        /// Deleting the product by identifier
        /// </summary>
        /// <param name="id">Product's ID that needs to delete.</param>
        Task DeleteAsync(string id);

        /// <summary>
        /// Provides a listing of all products.
        /// </summary>
        /// <returns>List of all products in database.</returns>
        Task<IEnumerable<ProductDB>> SelectAllAsync();

        /// <summary>
        /// Provides a listing of products that satisfy the condition.
        /// </summary>
        /// <param name="predicate">The condition that list of products must satisfy</param>
        /// <returns>List of products satisfying the specified conditions.</returns>
        Task<IEnumerable<ProductDB>> SelectWhereAsync(Predicate<ProductDB> predicate);

        /// <summary>
        /// Provides a listing of products with a specific category.
        /// </summary>
        /// <param name="category">Category for select products</param>
        /// <returns>List of products belonging to the specified category.</returns>
        Task<IEnumerable<ProductDB>> SelectByCategoryAsync(CategoryDB category);

        /// <summary>
        /// Gets products by ID.
        /// </summary>
        /// <param name="id">The specific id of product that was sorted.</param>
        /// <returns>Product chosen by Id.</returns>
        Task<ProductDB> GetByIdAsync(string id);

        /// <summary>
        /// Gets product by name.
        /// </summary>
        /// <param name="name">Name of the required product.</param>
        /// <returns>Product with a chosen name.</returns>
        Task<ProductDB> GetByNameAsync(string name);

        /// <summary>
        /// Adds the product to the specific category.
        /// </summary>
        /// <param name="productId">Id of the specific product.</param>
        /// <param name="categoryId">Id of the specific category.</param>
        Task AddToCategoryAsync(string productId, string categoryId);

        /// <summary>
        /// Updating the specific product
        /// </summary>
        /// <param name="product">The specific product for updating</param>
        Task UpdateAsync(ProductDB product);
    }
}
