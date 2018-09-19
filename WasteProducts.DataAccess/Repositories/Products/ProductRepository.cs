using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.Products
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
        /// <param name="context">The specific context of WasteContext</param>
        public ProductRepository(WasteContext context) => _context = context;

        /// <summary>
        /// Allows you to add new product to the products colletion.
        /// </summary>
        /// <param name="product">The specific product for adding</param>
        public async Task<string> AddAsync(ProductDB product)
        {
            product.Id = Guid.NewGuid().ToString();
            product.Created = DateTime.UtcNow;
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product.Id;
        }

        /// <summary>
        /// Deleting the specific product
        /// </summary>
        /// <param name="product">The specific product for deleting.</param>
        public async Task DeleteAsync(ProductDB product)
        {
            if (! (await _context.Products.ContainsAsync(product))) return;
            product.Marked = true;

            await UpdateAsync(product);
            
        }

        /// <summary>
        /// The method that delets the product by ID.
        /// </summary>
        /// <param name="id">Product's ID that needs to be deleted.</param>
        public async Task DeleteAsync(string id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return;

            product.Marked = true;

            await UpdateAsync(product);
            
        }

        public async Task<ProductDB> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name); 
        }

        /// <summary>
        /// A method that provides a list of all products.
        /// </summary>
        /// <returns>List of products.</returns>
        public async Task<IEnumerable<ProductDB>> SelectAllAsync()
        {
            return await Task.Run(() =>
            {
               return _context.Products.ToList();
            });
        }

        /// <inheritdoc />
        /// <summary>
        /// Provides a listing of products that satisfy the condition.
        /// </summary>
        /// <param name="predicate">The condition that list of products must satisfy</param>
        /// <returns>Returns list of products.</returns>
        public async Task <IEnumerable<ProductDB>> SelectWhereAsync(Predicate<ProductDB> predicate)
        {
            var condition = new Func<ProductDB, bool>(predicate);

            return await Task.Run(() =>
            {
                return _context.Products.Where(condition).ToList();
            });
                
        }

        /// <summary>
        /// Provides a listing of products with a specific category.
        /// </summary>
        /// <param name="category">Category for select products</param>
        /// <returns>Returns list of products.</returns>
        public async Task <IEnumerable<ProductDB>> SelectByCategoryAsync(CategoryDB category)
        {
            return await Task.Run(() =>
            {
                return _context.Products.Where(p => p.Category.Id == category.Id).ToList();
            });
        }

        /// <summary>
        /// A method that selectively provides a product by product's ID.
        /// </summary>
        /// <param name="id">The specific id of product that was sorted.</param>
        /// <returns>Product by ID.</returns>
        public async Task<ProductDB> GetByIdAsync(string id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// This method allows you to modify some or all of the product values.
        /// </summary>
        /// <param name="product"></param>
        public async Task UpdateAsync(ProductDB product)
        {
            _context.Entry(product).State = EntityState.Modified;

            var productInDb = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            productInDb.Modified = DateTime.UtcNow;

            await _context.SaveChangesAsync();
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
