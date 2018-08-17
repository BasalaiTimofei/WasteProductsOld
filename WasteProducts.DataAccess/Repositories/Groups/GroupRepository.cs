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
        public void Create(T item)
        {
            _dbSet.Add(item);
            Save(_context);
        }
        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            Save(_context);
        }
        public void Delete(int id)
        {
            var group = _dbSet.Find(id);
            if (group != null)
                _dbSet.Remove(group);
            Save(_context);
        }
        public T Get(int id)
        {
            return _dbSet.Find(id);
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }
        public IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }
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
