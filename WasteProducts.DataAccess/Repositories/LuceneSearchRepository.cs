using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Search;

namespace WasteProducts.DataAccess.Repositories
{
    /// <summary>
    /// Implementation of ISearchRepository with Lucene
    /// </summary>
    public class LuceneSearchRepository : ISearchRepository
    {
        public TEntity Get<TEntity>(string keyValue, string keyField = "Id")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll<TEntity>()
        {
            throw new NotImplementedException();
        }

        public void Insert<TEntity>(TEntity obj)
        {
            throw new NotImplementedException();
        }

        public void Update<TEntity>(TEntity obj)
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(TEntity obj)
        {
            throw new NotImplementedException();
        }

        //todo: delete async methods later
  
        public Task<TEntity> GetAsync<TEntity>(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync<TEntity>(TEntity obj)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync<TEntity>(TEntity obj)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync<TEntity>(TEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
