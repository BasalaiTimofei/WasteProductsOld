using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.Logic.Common.Models;
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

        public IEnumerable<TEntity> Search<TEntity>(SearchQuery query) where TEntity : class
        {
            return _repository.GetAll<TEntity>(query.Query, query.SearchableFields, MaxResultCount);
        }

        public IEnumerable<TEntity> SearchDefault<TEntity>(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public void AddToSearchIndex<TEntity>(TEntity model) where TEntity : class
        {
            _repository.Insert(model);
        }

        public void AddToSearchIndex<TEntity>(IEnumerable<TEntity> models) where TEntity : class
        {
            foreach (var model in models)
            {
                _repository.Insert(model);
            }
        }

        public void RemoveFromSearchIndex<TEntity>(TEntity model) where TEntity : class
        {
            _repository.Delete(model);
        }

        public void RemoveFromSearchIndex<TEntity>(IEnumerable<TEntity> models) where TEntity : class
        {
            foreach (var model in models)
            {
                _repository.Delete(model);
            }
        }

        public void UpdateInSearchIndex<TEntity>(TEntity model) where TEntity : class
        {
            _repository.Update(model);
        }

        public void UpdateInSearchIndex<TEntity>(IEnumerable<TEntity> models) where TEntity : class
        {
            foreach (var model in models)
            {
                _repository.Update(model);
            }
        }

        public void ClearSearchIndex()
        {
            _repository.Clear();
        }

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

    }
}
