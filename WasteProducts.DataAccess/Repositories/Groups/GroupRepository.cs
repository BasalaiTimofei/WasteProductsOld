﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.DataAccess.Contexts;
using System.Data.Entity;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.DataAccess.Repositories
{
    /// <summary>
    /// Group repository
    /// </summary>
    /// <typeparam name="T">Object</typeparam>
    public class GroupRepository<T> : IGroupRepository<T> where T : class
    {
        DbSet<T> _dbSet;
        IdentityDbContext _context;
        
        public GroupRepository(IdentityDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        /// <summary>
        /// Create - add a new object in db
        /// </summary>
        /// <param name="item">New object</param>
        public void Create(T item)
        {
            _dbSet.Add(item);
            Save(_context);
        }
        /// <summary>
        /// Update - correct object in db
        /// </summary>
        /// <param name="item">New object</param>
        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            Save(_context);
        }
        /// <summary>
        /// Delete - delete object from db
        /// </summary>
        /// <param name="id">Primary key object</param>
        public void Delete(int id)
        {
            var group = _dbSet.Find(id);
            if (group != null)
                _dbSet.Remove(group);
            Save(_context);
        }
        /// <summary>
        /// Get - getting object from db
        /// </summary>
        /// <param name="id">Primary key object</param>
        /// <returns>Object</returns>
        public T Get(int id)
        {
            return _dbSet.Find(id);
        }
        /// <summary>
        /// GetAll - returns all objects
        /// </summary>
        /// <returns>IEnumerable objects</returns>
        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }
        /// <summary>
        /// Find - returns objects set with condition
        /// </summary>
        /// <param name="predicate">lambda function</param>
        /// <returns>IEnumerable objects</returns>
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }
        /// <summary>
        /// GetWithInclude - immediate loading objects with condition
        /// </summary>
        /// <param name="includeProperties">expression trees</param>
        /// <returns>IEnumerable objects</returns>
        public IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }
        /// <summary>
        /// GetWithInclude - immediate loading objects with condition
        /// </summary>
        /// <param name="predicate">lambda function</param>
        /// <param name="includeProperties">expression trees</param>
        /// <returns>IEnumerable objects</returns>
        public IEnumerable<T> GetWithInclude(Func<T, bool> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }
        private IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        private void Save(IdentityDbContext context)
        {
            context.SaveChanges();
        }
    }
}