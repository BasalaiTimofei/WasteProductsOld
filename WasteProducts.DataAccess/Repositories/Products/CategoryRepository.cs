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

        /// <inheritdoc/>
        public CategoryRepository(WasteContext context) => _context = context;

        /// <inheritdoc/>
        public async Task<string> AddAsync(CategoryDB category)
        {
            category.Id = Guid.NewGuid().ToString();
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return category.Id;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> AddRangeAsync(IEnumerable<CategoryDB> categories)
        {
            var ids = new List<string>();
            categories.Select(c =>
            {
                c.Id = Guid.NewGuid().ToString();
                ids.Add(c.Id);
                return c;
            });

            (_context.Categories as DbSet).AddRange(categories);

            await _context.SaveChangesAsync();

            return ids;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(CategoryDB category)
        {
            if (!(await _context.Categories.ContainsAsync(category))) return;

            category.Marked = true;
            await UpdateAsync(category).ConfigureAwait(false);
            
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(string id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return;

            category.Marked = true;

            await UpdateAsync(category).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task <IEnumerable<CategoryDB>> SelectAllAsync()
        {
            var result = await Task.Run(() => _context.Categories.ToList()).ConfigureAwait(false);

            return result.Where(c => c.Marked == false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CategoryDB>> SelectWhereAsync(Predicate<CategoryDB> predicate)
        {
            var condition = new Func<CategoryDB, bool>(predicate);

            return await Task.Run(() => _context.Categories.Where(condition).ToList()).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task <CategoryDB> GetByIdAsync(string id)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);

            return (result.Marked == true) ? null : result;
        }

        /// <inheritdoc/>
        public async Task <CategoryDB> GetByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.Name == name).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(CategoryDB category)
        {
            var categoryInDb = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id).ConfigureAwait(false);

            var entry = _context.Entry(categoryInDb);
            entry.CurrentValues.SetValues(category);
            entry.Property(c => c.Id).IsModified = false;

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
