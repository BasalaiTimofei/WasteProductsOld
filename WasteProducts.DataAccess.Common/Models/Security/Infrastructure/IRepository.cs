using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// IRepository interface 
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// GetAll method returns all records
        /// </summary>
        /// <returns>List of TEntity</returns>
        List<TEntity> GetAll();

        /// <summary>
        /// GetAll Async method returns all records async
        /// </summary>
        /// <returns>Task List of TEntity</returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// GetAll Async with CancellationToken returns all records method
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task List of TEntity</returns>
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Page All method for getting some records
        /// </summary>
        /// <param name="skip">count of skipping element</param>
        /// <param name="take">result count elements</param>
        /// <returns>List of TEntity</returns>
        List<TEntity> PageAll(int skip, int take);


        /// <summary>
        /// Page All Async method for getting some records
        /// </summary>
        /// <param name="skip">count of skipping element</param>
        /// <param name="take">result count elements</param>
        /// <returns>Task List of TEntity</returns>
        Task<List<TEntity>> PageAllAsync(int skip, int take);

        /// <summary>
        /// PageAllAsync method for getting some records with CancellationToken async
        /// </summary>
        /// <param name="skip">count of skipping element</param>
        /// <param name="take">result count elements</param>
        /// <returns>Task List of TEntity</returns>
        Task<List<TEntity>> PageAllAsync(CancellationToken cancellationToken, int skip, int take);

        /// <summary>
        /// GetById method getting record by id 
        /// </summary>
        /// <param name="id">id object</param>
        /// <returns>TEntity</returns>
        TEntity GetById(object id);

        /// <summary>
        /// GetById method getting record by id async
        /// </summary>
        /// <param name="id">id object</param>
        /// <returns>Task of TEntity</returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// GetById method getting record by id async with CancellationToken
        /// </summary>
        /// <param name="id">id object</param>
        /// <returns>Task of TEntity</returns>
        Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, object id);

        /// <summary>
        /// Create operation
        /// </summary>
        /// <param name="entity">TEntity type of entity model</param>
        void Add(TEntity entity);

        /// <summary>
        /// Update method for updating record
        /// </summary>
        /// <param name="entity">T type of entity model</param>
        void Update(TEntity entity);

        /// <summary>
        /// Remove Method for deleting record
        /// </summary>
        /// <param name="entity">T type of entity model</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Remove Method for deleting record with Expression
        /// </summary>
        /// <param name="where">Expression</param>
        void Remove(Expression<Func<TEntity, bool>> where);

    }
}
