using System;
using System.Collections.Generic;
using System.Linq;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using System.Data.Entity;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.DataAccess.Repositories.Groups
{
    public class GroupRepository : IGroupRepository
    {
        IdentityDbContext _context;
        private bool _disposed = false;

        public GroupRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public void Create<T>(T item) where T : class
        {
            _context.Set<T>().Add(item);
        }

        public void Update<T>(T item) where T : class
        {
            _context.Entry(item).State = EntityState.Modified;
        }
 
        public void Update<T>(IEnumerable<T> items) where T : class
        {
            foreach (var item in items)
            {
                _context.Entry(item).State = EntityState.Modified;
            }
        }
 
        public void Delete<T>(Guid id) where T : class
        {
            var group = _context.Set<T>().Find(id);
            if (group != null)
                _context.Set<T>().Remove(group);
        }

        public T Get<T>(Guid id) where T : class
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>();
        }

        public IEnumerable<T> Find<T>(Func<T, bool> predicate) where T : class
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        public IEnumerable<T> GetWithInclude<T>(
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return Include(includeProperties).ToList();
        }

        public IEnumerable<T> GetWithInclude<T>(Func<T, bool> predicate,
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

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
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

        private IQueryable<T> Include<T>(
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T> query = _context.Set<T>();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        ~GroupRepository()
        {
            Dispose();
        }
    }
}
