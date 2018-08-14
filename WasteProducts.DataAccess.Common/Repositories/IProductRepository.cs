using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Product;

namespace WasteProducts.DataAccess.Common.Repositories
{
    /// <summary>
    /// Interface for the ProductRepository. Has an inheritance branch from IDisposable.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Provides a listing of all products.
        /// </summary>
        /// <returns>Returns list of products.</returns>
        IEnumerable<ProductDB> GetAll();

        /// <summary>
        /// Getinng students by ID.
        /// </summary>
        /// <param name="id">The specific id of product that was sorted.</param>
        /// <returns>Returns a product chosen by ID.</returns>
        ProductDB GetById(string id);

        /// <summary>
        /// Adding new product
        /// </summary>
        /// <param name="product">The specific product for adding</param>
        void Add(ProductDB product);

        /// <summary>
        /// Updating the specific product
        /// </summary>
        /// <param name="product">The specific product for updating</param>
        void Update(ProductDB product);

        /// <summary>
        /// Deleting the product by identifier
        /// </summary>
        /// <param name="id">Produxt's ID that needs to delete.</param>
        void DeleteById(string id);
    }
}
