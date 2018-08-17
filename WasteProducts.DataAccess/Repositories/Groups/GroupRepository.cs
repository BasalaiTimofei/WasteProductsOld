using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Contexts;
using System.Data.Entity;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.DataAccess.Repositories.Groups
{
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
        /// Create - add a new line
        /// </summary>
        /// <param name="item"></param>
        public void Create(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }
        /// <summary>
        /// Update - correct or add new data to the created line
        /// </summary>
        /// <param name="item"></param>
        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
        /// <summary>
        /// Delete - delete da
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var group = _dbSet.Find(id);
            if (group != null)
                _dbSet.Remove(group);
            _context.SaveChanges();
        }
        /// <summary>
        /// Find - returns a data set with condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }
        /// <summary>
        /// Get - getting data on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(int id)
        {
            return _dbSet.Find(id);
        }
        /// <summary>
        /// GetAll - returns all data
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }
        /// <summary>
        /// GetWithInclude - immediate loading of data with condition
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }
        /// <summary>
        /// GetWithInclude - immediate loading of data with condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
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

    }
}
