using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories
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
        public CategoryRepository(WasteContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new category
        /// </summary>
        /// <param name="category">The specific category for adding</param>
        public void Add(CategoryDB category)
        {
            if (category != null) _context.Categories.Add(category);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes the specific category
        /// </summary>
        /// <param name="category">The specific category for deleting</param>
        public void Delete(CategoryDB category)
        {
            if (category != null && _context.Categories.Contains(category))
            {
                category.Marked = true;
                Update(category);
            }
        }

        /// <summary>
        /// Deletes the specific category by id
        /// </summary>
        /// <param name="id">Represents a specific category id to delete</param>
        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            Delete(category);
        }

        /// <summary>
        /// Provides a listing of all categories.
        /// </summary>
        /// <returns>Returns list of categories.</returns>
        public IEnumerable<CategoryDB> SelectAll() => _context.Categories.ToList();

        /// <summary>
        /// Provides a listing of categories that satisfy the condition.
        /// </summary>
        /// <param name="predicate">The condition that list of categories must satisfy</param>
        /// <returns>Returns list of categories.</returns>
        public IEnumerable<CategoryDB> SelectWhere(Predicate<CategoryDB> predicate)
        {
            var condition = new Func<CategoryDB, bool>(predicate);
            return _context.Categories.Where(condition);
        }

        /// <summary>
        /// Gets category by ID
        /// </summary>
        /// <param name="id">The specific id of category that was sorted</param>
        /// <returns>Returns a category chosen by ID</returns>
        public CategoryDB GetById(int id) => _context.Categories.Find(id);

        /// <summary>
        /// Gets category by name
        /// </summary>
        /// <param name="name">The specific category for updating</param>
        public CategoryDB GetByName(string name) => _context.Categories.Find(name);

        /// <summary>
        /// Updates the specific category
        /// </summary>
        /// <param name="category">The specific category for updating</param>
        public void Update(CategoryDB category)
        {
            _context.Entry(category).State = EntityState.Modified;
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
            if (_disposed) return;
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}
