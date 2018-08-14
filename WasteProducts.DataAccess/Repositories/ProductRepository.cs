using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Category;
using WasteProducts.DataAccess.Common.Models.Product;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories
{   /// <summary>
    ///This class is a context class. A binder for the 'ProductDB' class with a data access.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly WasteContext _context;
        private bool _disposed;

        /// <summary>
        /// Using the context of the WasteContext class through the private field.
        /// </summary>
        /// <param name="context"></param>
        public ProductRepository(WasteContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Allows you to add new product to the products colletion.
        /// </summary>
        /// <param name="product">The specific product for adding</param>
        public void Add(ProductDB product)
        {
            product.Created = DateTime.UtcNow;
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deleting the specific product
        /// </summary>
        /// <param name="product">The specific product for deleting.</param>
        public void Delete(ProductDB product)
        {
            if (product != null)
                if (_context.Products.Contains(product))
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                }
        }

        /// <summary>
        /// The method that delets the product by ID.
        /// </summary>
        /// <param name="id">Product's ID that needs to be deleted.</param>
        public void DeleteById(string id)
        {
            var product = _context.Products.Find(id);
            if (product != null) _context.Products.Remove(product);
            _context.SaveChanges();
        }

        /// <summary>
        /// A method that provides a list of all products.
        /// </summary>
        /// <returns>List of products.</returns>
        public IEnumerable<ProductDB> SelectAll() => _context.Products.ToList();

        /// <summary>
        /// Provides a listing of products that satisfy the condition.
        /// </summary>
        /// <param name="predicate">The condition that list of products must satisfy</param>
        /// <returns>Returns list of products.</returns>
        public IEnumerable<ProductDB> SelectWhere(Predicate<ProductDB> predicate)
        {
            Func<ProductDB, bool> condition = new Func<ProductDB, bool>(predicate);
            return _context.Products.Where(condition).ToList();
        }

        /// <summary>
        /// Provides a listing of products with a specific category.
        /// </summary>
        /// <param name="category">Category for select products</param>
        /// <returns>Returns list of products.</returns>
        public IEnumerable<ProductDB> SelectByCategory(CategoryDB category)
        {
            return _context.Products.Where(p => p.CategoryDB.Id == category.Id).ToList();
        }

        /// <summary>
        /// A method that selectively provides a product by product's ID.
        /// </summary>
        /// <param name="id">The specific id of product that was sorted.</param>
        /// <returns>Product by ID.</returns>
        public ProductDB GetById(string id) => _context.Products.Find(id);

        /// <summary>
        /// This method allows you to modify some or all of the product values.
        /// </summary>
        /// <param name="product"></param>
        public void Update(ProductDB product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.Products.First(p => p.Id == product.Id).Modified = DateTime.UtcNow;
            _context.SaveChanges();
        }

        /// <summary>
        /// This method calls if the data context means release or closing of connections.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
