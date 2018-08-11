using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Product;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories
{/// <summary>
///This class is a context class. A binder for the 'ProductDB' class with a data access.
/// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly WasteContext _context;

        /// <summary>
        /// Using the context of the WasteContext class through the private field.
        /// </summary>
        /// <param name="context"></param>
        public ProductRepository(WasteContext context) => _context = context;

        /// <summary>
        /// A method that provides a list of all products.
        /// </summary>
        /// <returns>List of products.</returns>
        public IQueryable<ProductDB> GetAll() => _context.Products;

        /// <summary>
        /// A method that selectively provides a product by product's ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product by ID.</returns>
        public ProductDB GetById(string id) => _context.Products.Find(id);

        /// <summary>
        /// The method that delets the product by ID.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteById(string id)
        {
            var product = _context.Products.Find(id);
            if (product != null) _context.Products.Remove(product);
        }

        /// <summary>
        /// Saves all changes made with the product.
        ///Recommend to call this method every time you make any manipulations with the product.
        /// </summary>
        public void Save() => _context.SaveChanges();

        /// <summary>
        /// This method allows you to modify some or all of the product values.
        /// </summary>
        /// <param name="product"></param>
        public void Update(ProductDB product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        /// <summary>
        /// Allows you to add new product to the products colletion.
        /// </summary>
        /// <param name="product"></param>
        public void Add(ProductDB product)
        {
            _context.Products.Add(product);
        }

        /// <summary>
        /// This method calls if the data context means release or closing of connections.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
