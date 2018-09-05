using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Products;

namespace WasteProducts.DataAccess.Common.Repositories
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
        void Add(ProductDB product);

        /// <summary>
        /// Deleting the specific product
        /// </summary>
        /// <param name="product">The specific product for deleting.</param>
        void Delete(ProductDB product);

        /// <summary>
        /// Deleting the product by identifier
        /// </summary>
        /// <param name="id">Product's ID that needs to delete.</param>
        void DeleteById(string id);

        /// <summary>
        /// Provides a listing of all products.
        /// </summary>
        /// <returns>Returns list of products.</returns>
        IEnumerable<ProductDB> SelectAll();

        /// <summary>
        /// Provides a listing of products that satisfy the condition.
        /// </summary>
        /// <param name="predicate">The condition that list of products must satisfy</param>
        /// <returns>Returns list of products.</returns>
        IEnumerable<ProductDB> SelectWhere(Predicate<ProductDB> predicate);

        /// <summary>
        /// Provides a listing of products with a specific category.
        /// </summary>
        /// <param name="category">Category for select products</param>
        /// <returns>Returns list of products.</returns>
        IEnumerable<ProductDB> SelectByCategory(CategoryDB category);

        /// <summary>
        /// Gets products by ID.
        /// </summary>
        /// <param name="id">The specific id of product that was sorted.</param>
        /// <returns>Returns a product chosen by ID.</returns>
        ProductDB GetById(string id);

        /// <summary>
        /// Gets product by name.
        /// </summary>
        /// <param name="name">Name of the required product.</param>
        /// <returns>Product with a chosen name.</returns>
        Task<ProductDB> GetByNameAsync(string name);

        /// <summary>
        /// Updating the specific product
        /// </summary>
        /// <param name="product">The specific product for updating</param>
        void Update(ProductDB product);
    }
}
