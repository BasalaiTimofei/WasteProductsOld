using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories
{
    class CategoryRepository : ICategoryRepository
    {
        private readonly WasteContext _context;
        private bool _disposed;

        public CategoryRepository(WasteContext context)
        {
            _context = context;
        }

        public void Add(CategoryDB category)
        {
            if (category != null) _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Delete(CategoryDB category)
        {
            if (category != null && _context.Categories.Contains(category))
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public void DeleteById(int id)
        {
            var category = _context.Categories.Find(id);
            Delete(category);
        }

        public IEnumerable<CategoryDB> SelectAll() => _context.Categories.ToList();

        public IEnumerable<CategoryDB> SelectWhere(Predicate<CategoryDB> predicate)
        {
            var condition = new Func<CategoryDB, bool>(predicate);
            return _context.Categories.Where(condition);
        }

        public CategoryDB GetById(int id) => _context.Categories.Find(id);

        public void Update(CategoryDB category)
        {
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

        }

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
