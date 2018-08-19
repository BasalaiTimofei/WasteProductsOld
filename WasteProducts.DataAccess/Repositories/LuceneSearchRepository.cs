using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers.Classic;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.DataAccess.Common.Exceptions;
using Lucene.Net.Analysis.Standard;

namespace WasteProducts.DataAccess.Repositories
{
    /// <summary>
    /// Implementation of ISearchRepository with Lucene
    /// </summary>
    public class LuceneSearchRepository : ISearchRepository, IDisposable
    {

        public const LuceneVersion MATCH_LUCENE_VERSION = LuceneVersion.LUCENE_48;
        public string IndexPath { get; private set; }
        public string IDField { get; private set; } = "Id";

        private Lucene.Net.Store.Directory _directory;
        private Analyzer _analyzer;
        private IndexWriter _writer;
        //private SearcherManager _searcherManager;

        /// <summary>
        /// Creates Lucene repository
        /// </summary>
        public LuceneSearchRepository()
        {

            string assemblyFilename = Assembly.GetAssembly(typeof(LuceneSearchRepository)).Location;
            string assemblyPath = Path.GetDirectoryName(assemblyFilename);
            IndexPath = Path.Combine(assemblyPath, ConfigurationManager.AppSettings["LuceneIndexStoragePath"]);
            //_analyzer = new WhitespaceAnalyzer(MATCH_LUCENE_VERSION);
            _analyzer = new StandardAnalyzer(MATCH_LUCENE_VERSION);
            try
            {
                _directory = FSDirectory.Open(IndexPath);
                _writer = new IndexWriter(_directory, new IndexWriterConfig(MATCH_LUCENE_VERSION, _analyzer)
                {
                    OpenMode = OpenMode.CREATE_OR_APPEND
                });
                if (IndexWriter.IsLocked(_directory))
                {
                    IndexWriter.Unlock(_directory);
                }
                //_searcherManager = new SearcherManager(_writer, true, null);
                _writer.Commit();
            }
            catch (Exception ex)
            {
                throw new LuceneSearchRepositoryException($"Can't open Lucene index. {ex.Message}",ex);
            }
        }

        /// <summary>
        /// Creates Lucene repository 
        /// </summary>
        /// <param name="clearIndex">If true existing index will be cleared</param>
        public LuceneSearchRepository(bool clearIndex):this()
        {
            if (clearIndex)
            {
                Clear();
            }
        }

        /// <summary>
        /// Returns entity from repository by numeric Id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public TEntity GetById<TEntity>(int id) where TEntity : class
        {
            Query queryGet = NumericRangeQuery.NewInt64Range(IDField, id, id, true, true);
            return ProceedQuery<TEntity>(queryGet);
        }

        /// <summary>
        /// Returns entity from repository by field value
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValue">Field value</param>
        /// <param name="keyField">Name of field</param>
        /// <returns></returns>
        public TEntity Get<TEntity>(string keyValue, string keyField) where TEntity : class
        {
            Query queryGet = new TermQuery(new Term(keyField, keyValue));
            return ProceedQuery<TEntity>(queryGet);
        }
        
        /// <summary>
        /// Returns list of objects of TEntity type from repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity  :class
        {           
            Query queryGet = NumericRangeQuery.NewInt64Range(IDField, 0, Int32.MaxValue, true, true);
            return ProceedQueryList<TEntity>(queryGet, Int32.MaxValue);
        }

        /// <summary>
        /// Returns list of objects of TEntity type from repository which contain any word
        /// from query string in searchable fields
        /// </summary>
        /// <typeparam name="TEnity"></typeparam>
        /// <param name="queryString">String containing words to search</param>
        /// <param name="searchableFields">Searchable fields</param>
        /// <param name="numResults">Maximal number of results</param>
        /// <returns></returns>
        public IEnumerable<TEnity> GetAll<TEnity>(string queryString, IEnumerable<string> searchableFields, int numResults) where TEnity : class
        {
            BooleanQuery booleanQuery = PrepareLuceneQuery(queryString, searchableFields, null);
            return ProceedQueryList<TEnity>(booleanQuery, numResults);
        }

        /// <summary>
        /// Returns list of objects of TEntity type from repository which contain any word
        /// from query string in searchable fields with boost values for the fields
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="queryString">String containing words to search</param>
        /// <param name="searchableFields">Searchable fields</param>
        /// <param name="boosts">Boost values for searchable fields</param>
        /// <param name="numResults"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll<TEntity>(string queryString, IEnumerable<string> searchableFields, ReadOnlyDictionary<string, float> boosts, int numResults) where TEntity : class
        {
            BooleanQuery booleanQuery = PrepareLuceneQuery(queryString, searchableFields, boosts);
            return ProceedQueryList<TEntity>(booleanQuery, numResults);
        }

        /// <summary>
        /// Inserts object into repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        public void Insert<TEntity>(TEntity obj) where TEntity : class
        {
            Document doc = obj.ToDocument();
            _writer.AddDocument(doc);
            _writer.Commit();
        }

        /// <summary>
        /// Updates object in repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        public void Update<TEntity>(TEntity obj) where TEntity : class 
        {
            System.Reflection.PropertyInfo keyFieldInfo = typeof(TEntity).GetProperty(IDField);
            int id = (int)keyFieldInfo.GetValue(obj);
            if (id>0)
            {
                if (GetById<TEntity>(id) != null)
                {
                    Delete<TEntity>(obj);
                    Insert<TEntity>(obj);
                }
            }
            else
            {
                throw new LuceneSearchRepositoryException("Сan't update entity with empty id.");
            }
        }

        /// <summary>
        /// Deletes object from reposytory
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        public void Delete<TEntity>(TEntity obj) where TEntity : class
        {

            System.Reflection.PropertyInfo keyFieldInfo = typeof(TEntity).GetProperty(IDField);
            int id = (int)keyFieldInfo.GetValue(obj);
            if (id>0)
            {
                _writer.DeleteDocuments<TEntity>(NumericRangeQuery.NewInt64Range(IDField, id, id, true, true));
                _writer.Commit();
            }
            else
            {
                throw new LuceneSearchRepositoryException("Сan't delete entity with empty id.");
            }
        }

        /// <summary>
        /// Clears repository
        /// </summary>
        public void Clear()
        {
            try
            {
                _writer.DeleteAll();
                _writer.Commit();
            }catch(Exception ex)
            {
                throw new LuceneSearchRepositoryException($"Can't clear index. {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Optimizes repository by merging index files
        /// </summary>
        public void Optimize()
        {
            try
            {
                _writer.ForceMerge(1);
                _writer.Commit();
            }
            catch (Exception ex)
            {
                _writer.Dispose();
                _writer = new IndexWriter(_directory, new IndexWriterConfig(MATCH_LUCENE_VERSION, _analyzer)
                {
                    OpenMode = OpenMode.CREATE_OR_APPEND
                });
                throw new LuceneSearchRepositoryException($"Can't optimize index. {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Proceeds Lucene's query and returns single object
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query">Lucene's query</param>
        /// <returns></returns>
        private TEntity ProceedQuery<TEntity>(Query query) where TEntity : class
        {
            using (var reader = DirectoryReader.Open(_directory))
            {
                var searcher = new IndexSearcher(reader);
                try
                {
                    TopDocs docs = searcher.Search<TEntity>(query, 1000);
                    ScoreDoc firstScoreDoc = docs.ScoreDocs.FirstOrDefault();
                    if (firstScoreDoc != null)
                    {
                        Document doc = searcher.Doc(docs.ScoreDocs[0].Doc);
                        TEntity entity = doc.ToObject<TEntity>();
                        return entity;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch(Exception ex)
                {
                    //_searcherManager.Release(searcher);
                    throw new LuceneSearchRepositoryException($"Can't proceed query. {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Proceeds Lucene's query and returns list of objects
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query">Lucene's query</param>
        /// <param name="numResults">Maximal number of results</param>
        /// <returns></returns>
        private IEnumerable<TEntity> ProceedQueryList<TEntity>(Query query, int numResults) where TEntity : class
        {
            using (var reader = DirectoryReader.Open(_directory))
            {
                var searcher = new IndexSearcher(reader);
                List<TEntity> entityList = new List<TEntity>();
                try
                {
                    var docs = searcher.Search<TEntity>(query, numResults);
                    foreach (var scoreDoc in docs.ScoreDocs)
                    {
                        Document doc = searcher.Doc(scoreDoc.Doc);
                        TEntity entity = doc.ToObject<TEntity>();
                        entityList.Add(entity);
                    }
                    return entityList;
                }
                catch(Exception ex)
                {
                    //_searcherManager.Release(searcher);
                    throw new LuceneSearchRepositoryException($"Can't proceed query. {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Checks whether query string is null or empty
        /// </summary>
        /// <param name="queryString"></param>
        private void CheckQueryString(string queryString)
        {

            if (String.IsNullOrEmpty(queryString))
                throw new ArgumentException("Search string can't be empty or null or consist only from whitespaces");
        }

        /// <summary>
        /// Prepares correct Lucene's query
        /// </summary>
        /// <param name="queryString">String with words to search</param>
        /// <param name="searchableFields">Searchable fields</param>
        /// <param name="boosts">Boost values fo searchable fields</param>
        /// <returns></returns>
        private BooleanQuery PrepareLuceneQuery(string queryString, IEnumerable<string> searchableFields, ReadOnlyDictionary<string, float> boosts)
        {
            char[] charsToTrim = { '*', ' ' };
            queryString = queryString.ToLower().Trim(charsToTrim);
            CheckQueryString(queryString);
            if (!searchableFields.Any())
            {
                throw new ArgumentException("Can't search with empty filelds.");
            }
            BooleanQuery booleanQuery = new BooleanQuery();
            var searchTerms = queryString.Split(' ');
            foreach (var term in searchTerms)
            {
                foreach (var field in searchableFields)
                {
                    WildcardQuery wildcardQuery = new WildcardQuery(new Term(field, $"{term}*"));
                    if (boosts!=null)
                    {
                        wildcardQuery.Boost = boosts[field];
                    }
                    booleanQuery.Add(wildcardQuery, Occur.SHOULD);
                }
            }
            return booleanQuery;
        }

        //TODO: add realization of async methods later
        #region Async methods

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
        #endregion

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _writer.Commit();
                    _writer.Dispose();
                    _directory.Dispose();
                    _analyzer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LuceneSearchRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}