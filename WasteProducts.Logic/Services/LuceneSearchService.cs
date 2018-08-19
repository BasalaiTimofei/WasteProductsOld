using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Search;
using WasteProducts.Logic.Common.Services.Search;

namespace WasteProducts.Logic.Services
{
    /// <summary>
    /// Implementation of ISearchService with Lucene
    /// </summary>
    public class LuceneSearchService : ISearchService
    {
        private ISearchRepository _repository;
        public const int DEFAULT_MAX_LUCENE_RESULTS = 1000;
        public int MaxResultCount { get; set; } = DEFAULT_MAX_LUCENE_RESULTS;

        public LuceneSearchService(ISearchRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Performs search in repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query">SearchQuery object containing query information</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Search<TEntity>(SearchQuery query) where TEntity : class
        {
            CheckQuery(query);
            return _repository.GetAll<TEntity>(query.Query, query.SearchableFields, MaxResultCount);
        }

        /// <summary>
        /// Performs search in repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query">BoostedSearchQuery object containing query information</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Search<TEntity>(BoostedSearchQuery query) where TEntity : class
        {
            CheckQuery(query);
            return _repository.GetAll<TEntity>(query.Query, query.SearchableFields, query.BoostValues, MaxResultCount);
        }

        public IEnumerable<TEntity> SearchDefault<TEntity>(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds object to repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        public void AddToSearchIndex<TEntity>(TEntity model) where TEntity : class
        {
            _repository.Insert(model);
        }

        /// <summary>
        /// Adds list of objects to repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        public void AddToSearchIndex<TEntity>(IEnumerable<TEntity> models) where TEntity : class
        {
            foreach (var model in models)
            {
                _repository.Insert(model);
            }
        }

        /// <summary>
        /// Removes object from repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        public void RemoveFromSearchIndex<TEntity>(TEntity model) where TEntity : class
        {
            _repository.Delete(model);
        }

        /// <summary>
        /// Removes list of objects from repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        public void RemoveFromSearchIndex<TEntity>(IEnumerable<TEntity> models) where TEntity : class
        {
            foreach (var model in models)
            {
                _repository.Delete(model);
            }
        }

        /// <summary>
        /// Updates object in repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        public void UpdateInSearchIndex<TEntity>(TEntity model) where TEntity : class
        {
            _repository.Update(model);
        }

        /// <summary>
        /// Updates list of objects in repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        public void UpdateInSearchIndex<TEntity>(IEnumerable<TEntity> models) where TEntity : class
        {
            foreach (var model in models)
            {
                _repository.Update(model);
            }
        }

        /// <summary>
        /// Clears repository
        /// </summary>
        public void ClearSearchIndex()
        {
            _repository.Clear();
        }

        /// <summary>
        /// Optimizes repository for faster search
        /// </summary>
        public void OptimizeSearchIndex()
        {
            _repository.Optimize();
        }

        //todo: implement async methods later if necessary

        public Task<IEnumerable<TEntity>> SearchAsync<TEntity>(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> SearchDefaultAsync<TEntity>(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        private void CheckQuery(SearchQuery query)
        {
            if (String.IsNullOrEmpty(query.Query) || query.SearchableFields.Count == 0)
            {
                throw new ArgumentException("Incorrect query.");
            }
        }

    }
}
