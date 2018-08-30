using System;
using System.Collections.Generic;
using System.Linq;
using WasteProducts.DataAccess.Common.Repositories;
using System.Data.Entity;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.DataAccess.Repositories
{
    /// <summary>
    /// Group repository
    /// </summary>
    /// <typeparam name="T">Object</typeparam>
    public class GroupRepository : IGroupRepository
    {
        IdentityDbContext _context;
        private bool _disposed = false;

        public GroupRepository(IdentityDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Create - add a new object in db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">New object</param>
        public void Create<T>(T item) where T : class
        {
            _context.Set<T>().Add(item);
        }
        /// <summary>
        /// Update - correct object in db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">New object</param>
        public void Update<T>(T item) where T : class
        {
            _context.Entry(item).State = EntityState.Modified;
        }
        /// <summary>
        /// Update - correct object in db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="items">New objects</param>
        public void Update<T>(IEnumerable<T> items) where T : class
        {
            foreach (var item in items)
            {
                _context.Entry(item).State = EntityState.Modified;
            }
        }
        /// <summary>
        /// Delete - delete object from db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="id">Primary key object</param>
        public void Delete<T>(int id) where T : class
        {
            var group = _context.Set<T>().Find(id);
            if (group != null)
                _context.Set<T>().Remove(group);
        }
        /// <summary>
        /// Get - getting object from db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="id">Primary key object</param>
        /// <returns>Object</returns>
        public T Get<T>(int id) where T : class
        {
            return _context.Set<T>().Find(id);
        }
        /// <summary>
        /// GetAll - returns all objects
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <returns>IEnumerable objects</returns>
        public IEnumerable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>();
        }
        /// <summary>
        /// Find - returns objects set with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="predicate">lambda function</param>
        /// <returns>IEnumerable objects</returns>
        public IEnumerable<T> Find<T>(Func<T, bool> predicate) where T : class
        {
            return _context.Set<T>().Where(predicate).ToList();
        }
        /// <summary>
        /// GetWithInclude - immediate loading objects with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="includeProperties">expression trees</param>
        /// <returns>IEnumerable objects</returns>
        public IEnumerable<T> GetWithInclude<T>(
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return Include(includeProperties).ToList();
        }
        /// <summary>
        /// GetWithInclude - immediate loading objects with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="predicate">lambda function</param>
        /// <param name="includeProperties">expression trees</param>
        /// <returns>IEnumerable objects</returns>
        public IEnumerable<T> GetWithInclude<T>(Func<T, bool> predicate,
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }
        /// <summary>
        /// Save = save model 
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }
        /// <summary>
        /// Dispose = delete contecst
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
        private IQueryable<T> Include<T>(
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T> query = _context.Set<T>();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
