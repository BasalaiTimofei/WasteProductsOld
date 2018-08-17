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

        public LuceneSearchRepository()
        {

            string assemblyFilename = Assembly.GetAssembly(typeof(LuceneSearchRepository)).Location;
            string assemblyPath = Path.GetDirectoryName(assemblyFilename);
            IndexPath = Path.Combine(assemblyPath, ConfigurationManager.AppSettings["LuceneIndexStoragePath"]);
            _analyzer = new WhitespaceAnalyzer(MATCH_LUCENE_VERSION);
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

        public LuceneSearchRepository(bool clearIndex):this()
        {
            if (clearIndex)
            {
                Clear();
            }
        }

        public TEntity GetById<TEntity>(int id) where TEntity : class
        {
            Query queryGet = NumericRangeQuery.NewInt64Range(IDField, id, id, true, true);
            return ProceedQuery<TEntity>(queryGet, 1);
        }

        public TEntity Get<TEntity>(string keyValue, string keyField) where TEntity : class
        {
            Query queryGet = new TermQuery(new Term(keyField, keyValue));
            return ProceedQuery<TEntity>(queryGet, 1);
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity  :class
        {           
            Query queryGet = NumericRangeQuery.NewInt64Range(IDField, 0, Int32.MaxValue, true, true);
            return ProceedQueryList<TEntity>(queryGet, Int32.MaxValue);
        }

        public IEnumerable<TEnity> GetAll<TEnity>(string queryString, IEnumerable<string> searchableFields, int numResults) where TEnity : class
        {
            CheckQueryString(queryString);
            BooleanQuery booleanQuery = new BooleanQuery();
            var searchTerms = queryString.Split(' ');
            foreach (var term in searchTerms)
            {
                foreach (var field in searchableFields)
                {
                    WildcardQuery wildcardQuery = new WildcardQuery(new Term(field, $"{term}*"));
                    booleanQuery.Add(wildcardQuery, Occur.SHOULD);
                }
            }
            return ProceedQueryList<TEnity>(booleanQuery, numResults);
        }

        public IEnumerable<TEntity> GetAll<TEntity>(string queryString, IEnumerable<string> searchableFields, ReadOnlyDictionary<string, float> boosts, int numResults) where TEntity : class
        {
            CheckQueryString(queryString);
            BooleanQuery booleanQuery = new BooleanQuery();
            var searchTerms = queryString.Split(' ');
            foreach (var term in searchTerms)
            {
                foreach (var field in searchableFields)
                {
                    WildcardQuery wildcardQuery = new WildcardQuery(new Term(field, $"{term}*"));
                    wildcardQuery.Boost = boosts[field];
                    booleanQuery.Add(wildcardQuery, Occur.SHOULD);
                }
            }
            return ProceedQueryList<TEntity>(booleanQuery, numResults);
        }

        public void Insert<TEntity>(TEntity obj) where TEntity : class
        {
            Document doc = obj.ToDocument();
            _writer.AddDocument(doc);
            _writer.Commit();
        }

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

        private TEntity ProceedQuery<TEntity>(Query query, int numResults) where TEntity : class
        {
            using (var reader = DirectoryReader.Open(_directory))
            {
                var searcher = new IndexSearcher(reader);
                try
                {
                    TopDocs docs = searcher.Search<TEntity>(query, numResults);
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

        private void CheckQueryString(string queryString)
        {

            if (String.IsNullOrEmpty(queryString))
                throw new LuceneSearchRepositoryException("Search string can't be empty or null.");
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