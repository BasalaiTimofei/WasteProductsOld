using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;
using WasteProducts.DataAccess.Contexts.Security;

namespace WasteProducts.DataAccess.Repositories.Security
{
    internal abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class

    {
        #region Properties
        protected IdentityContext db;
        protected readonly DbSet<TEntity> dbSet;

        protected DbFactory DbFactory
        {
            get;
            private set;
        }

        protected DbContext DbContext
        {
            //TODO replace invalid param @"constring"
            get { return db ?? (db = DbFactory.Init(@"constring")); }
        }
        #endregion

        protected RepositoryBase(DbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<TEntity>();
        }

        #region Implementation
        public List<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return dbSet.ToListAsync();
        }

        public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return dbSet.ToListAsync(cancellationToken);
        }

        public List<TEntity> PageAll(int skip, int take)
        {
            return dbSet.Skip(skip).Take(take).ToList();
        }

        public Task<List<TEntity>> PageAllAsync(int skip, int take)
        {
            return dbSet.Skip(skip).Take(take).ToListAsync();
        }

        public Task<List<TEntity>> PageAllAsync(CancellationToken cancellationToken, int skip, int take)
        {
            return dbSet.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        public TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public Task<TEntity> GetByIdAsync(object id)
        {
            return dbSet.FindAsync(id);
        }

        public Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, object id)
        {
            return dbSet.FindAsync(cancellationToken, id);
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            var entry = db.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                dbSet.Attach(entity);
                entry = db.Entry(entity);
            }
            entry.State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void Remove(Expression<Func<TEntity, bool>> where)
        {
            dbSet.RemoveRange(dbSet.Where(where));
        }
        #endregion

    }
}
