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
        public LuceneSearchService(ISearchRepository repository)
        {
            _repository = repository;
        }

        public SearchResult Search<TEntity>(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public SearchResult SearchAll(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public SearchResult SearchDefault<TEntity>(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public SearchResult SearchAllDefault(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public void AddToSearchIndex<TEntity>(TEntity model)
        {
            throw new NotImplementedException();
        }

        public void AddToSearchIndex<TEntity>(IEnumerable<TEntity> model)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromSearchIndex<TEntity>(TEntity model)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromSearchIndex<TEntity>(IEnumerable<TEntity> model)
        {
            throw new NotImplementedException();
        }

        public void UpdateInSearchIndex<TEntity>(TEntity model)
        {
            throw new NotImplementedException();
        }

        public void UpdateInSearchIndex<TEntity>(IEnumerable<TEntity> model)
        {
            throw new NotImplementedException();
        }

        public bool ClearSearchIndex()
        {
            throw new NotImplementedException();
        }

        public bool OptimizeSearchIndex()
        {
            throw new NotImplementedException();
        }

        //todo: implement async methods later

        public Task<SearchResult> SearchAsync<TEntity>(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<SearchResult> SearchAllAsync(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<SearchResult> SearchDefaultAsync<TEntity>(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<SearchResult> SearchAllDefaultAsync(SearchQuery query)
        {
            throw new NotImplementedException();
        }

    }
}
