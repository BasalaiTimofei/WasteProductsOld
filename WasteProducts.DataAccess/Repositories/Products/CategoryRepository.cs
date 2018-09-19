using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.Products
{
    /// <summary>
    /// This class is a context class. A binder for the 'CategoyrDB' class with a data access.
    /// </summary>
    class CategoryRepository : ICategoryRepository
    {
        private readonly WasteContext _context;
        private bool _disposed;

        /// <summary>
        /// Using the context of the WasteContext class through the private field.
        /// </summary>
        /// <param name="context">The specific context of WasteContext</param>
        public CategoryRepository(WasteContext context) => _context = context;

        /// <summary>
        /// Adds a new category
        /// </summary>
        /// <param name="category">The specific category for adding</param>
        public async Task<string> AddAsync(CategoryDB category)
        {
            category.Id = Guid.NewGuid().ToString();
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category.Id;
        }

        /// <summary>
        /// Deletes the specific category
        /// </summary>
        /// <param name="category">The specific category for deleting</param>
        public async Task DeleteAsync(CategoryDB category)
        {
            if (!(await _context.Categories.ContainsAsync(category))) return;

            category.Marked = true;
            await UpdateAsync(category);
            
        }

        /// <summary>
        /// Deletes the specific category by id
        /// </summary>
        /// <param name="id">Represents a specific category id to delete</param>
        public async Task DeleteAsync(string id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return;

            category.Marked = true;

            await UpdateAsync(category);
        }

        /// <summary>
        /// Provides a listing of all categories.
        /// </summary>
        /// <returns>Returns list of categories.</returns>
        public async Task <IEnumerable<CategoryDB>> SelectAllAsync()
        {
            return await Task.Run(() => _context.Categories.ToList());
        }

        /// <summary>
        /// Provides a listing of categories that satisfy the condition.
        /// </summary>
        /// <param name="predicate">The condition that list of categories must satisfy</param>
        /// <returns>Returns list of categories.</returns>
        public async Task <IEnumerable<CategoryDB>> SelectWhereAsync(Predicate<CategoryDB> predicate)
        {
            var condition = new Func<CategoryDB, bool>(predicate);

            return await Task.Run(() => _context.Categories.Where(condition).ToList());
        }

        /// <summary>
        /// Gets category by ID
        /// </summary>
        /// <param name="id">The specific id of category that was sorted</param>
        /// <returns>Returns a category chosen by ID</returns>
        public async Task <CategoryDB> GetByIdAsync(string id)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Gets category by name
        /// </summary>
        /// <param name="name">The specific category for updating</param>
        public async Task <CategoryDB> GetByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.Name == name);
        }

        /// <summary>
        /// Updates the specific category
        /// </summary>
        /// <param name="category">The specific category for updating</param>
        public async Task UpdateAsync(CategoryDB category)
        {
            _context.Entry(category).State = EntityState.Modified;

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
            if (_disposed) return;
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}
