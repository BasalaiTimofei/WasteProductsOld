using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Security;
using WasteProducts.DataAccess.Contexts.Security;

namespace WasteProducts.DataAccess.Repositories.Security
{
    /// <summary>
    /// Abstract Base Repository TEntity, repository class inheritance from IRepositoryBase interface 
    /// </summary>
    internal abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        #region Properties
        /// <summary>
        /// _db field type of Security.IdentityContext
        /// </summary>
        protected IdentityContext _db;

        /// <summary>
        /// Read only field contains Data Set 
        /// </summary>
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// DbFactory field
        /// </summary>
        protected DbFactory DbFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// DbContext as DbContext from dbFactory
        /// </summary>
        protected DbContext DbContext
        {
            //TODO replace invalid param @"constring"
            get { return _db ?? (_db = DbFactory.Init(@"constring")); }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of RepositoryBase
        /// </summary>
        /// <param name="dbFactory">Used to set DbFactory</param>
        protected RepositoryBase(DbFactory dbFactory)
        {
            DbFactory = dbFactory;
            _dbSet = DbContext.Set<TEntity>();
        }

        #region Implementation

        /// <summary>
        /// GetAll method returns all records
        /// </summary>
        /// <returns>List of TEntity</returns>
        public List<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        /// <summary>
        /// GetAll Async method returns all records async
        /// </summary>
        /// <returns>Task List of TEntity</returns>
        public Task<List<TEntity>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        /// <summary>
        /// GetAll Async with CancellationToken returns all records method
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task List of TEntity</returns>
        public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _dbSet.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Page All method for getting some records
        /// </summary>
        /// <param name="skip">count of skipping element</param>
        /// <param name="take">result count elements</param>
        /// <returns>List of TEntity</returns>
        public List<TEntity> PageAll(int skip, int take)
        {
            return _dbSet.Skip(skip).Take(take).ToList();
        }


        /// <summary>
        /// Page All Async method for getting some records
        /// </summary>
        /// <param name="skip">count of skipping element</param>
        /// <param name="take">result count elements</param>
        /// <returns>Task List of TEntity</returns>
        public Task<List<TEntity>> PageAllAsync(int skip, int take)
        {
            return _dbSet.Skip(skip).Take(take).ToListAsync();
        }

        /// <summary>
        /// PageAllAsync method for getting some records with CancellationToken async
        /// </summary>
        /// <param name="skip">count of skipping element</param>
        /// <param name="take">result count elements</param>
        /// <returns>Task List of TEntity</returns>
        public Task<List<TEntity>> PageAllAsync(CancellationToken cancellationToken, int skip, int take)
        {
            return _dbSet.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// GetById method getting record by id 
        /// </summary>
        /// <param name="id">id object</param>
        /// <returns>TEntity</returns>
        public TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// GetById method getting record by id async
        /// </summary>
        /// <param name="id">id object</param>
        /// <returns>Task of TEntity</returns>
        public Task<TEntity> GetByIdAsync(object id)
        {
            return _dbSet.FindAsync(id);
        }

        /// <summary>
        /// GetById method getting record by id async with CancellationToken
        /// </summary>
        /// <param name="id">id object</param>
        /// <returns>Task of TEntity</returns>
        public Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, object id)
        {
            return _dbSet.FindAsync(cancellationToken, id);
        }


        /// <summary>
        /// Create operation
        /// </summary>
        /// <param name="entity">TEntity type of entity model</param>
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Update method for updating record
        /// </summary>
        /// <param name="entity">T type of entity model</param>
        public void Update(TEntity entity)
        {
            var entry = _db.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
                entry = _db.Entry(entity);
            }
            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// Remove Method for deleting record
        /// </summary>
        /// <param name="entity">T type of entity model</param>
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Remove Method for deleting record with Expression
        /// </summary>
        /// <param name="where">Expression</param>
        public void Remove(Expression<Func<TEntity, bool>> where)
        {
            _dbSet.RemoveRange(_dbSet.Where(where));
        }
        #endregion

    }
}
