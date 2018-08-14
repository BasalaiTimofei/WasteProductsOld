using System;
using System.Collections.Generic;
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
using Directory = Lucene.Net.Store.Directory;
using System.Runtime.Serialization;

namespace WasteProducts.DataAccess.Repositories
{
    /// <summary>
    /// Implementation of ISearchRepository with Lucene
    /// </summary>
    public class LuceneSearchRepository : ISearchRepository, IDisposable
    {

        public const LuceneVersion MATCH_LUCENE_VERSION = LuceneVersion.LUCENE_48;
        public string IndexPath { get; private set; }
        private Lucene.Net.Store.Directory _directory;
        private Analyzer _analyzer;
        private IndexWriter _writer;
        private SearcherManager _searcherManager;

        public LuceneSearchRepository()
        {

            string assemblyFilename = Assembly.GetAssembly(typeof(LuceneSearchRepository)).Location;
            string assemblyPath = Path.GetDirectoryName(assemblyFilename);
            IndexPath = assemblyPath + ConfigurationManager.AppSettings["LuceneIndexStoragePath"];
            _analyzer = new WhitespaceAnalyzer(MATCH_LUCENE_VERSION);
            try
            {
                _directory = FSDirectory.Open(IndexPath);
                IndexWriterConfig config = new IndexWriterConfig(MATCH_LUCENE_VERSION, _analyzer);
                config.OpenMode = OpenMode.CREATE_OR_APPEND;
                _writer = new IndexWriter(_directory, config);
                _searcherManager = new SearcherManager(_writer, true, null);
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
                ClearIndex();
            }
        }

        public TEntity Get<TEntity>(string keyValue, string keyField = "Id") where TEntity : class
        {
            Query queryGet = new TermQuery(new Term(keyField, keyValue));
            var searcher = _searcherManager.Acquire();
            try
            {
                TopDocs docs = searcher.Search<TEntity>(queryGet, 1);
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
            finally
            {
                _searcherManager.Release(searcher);
            }
        }

        public IEnumerable<TEntity> GetAll<TEntity>(int numResults) where TEntity  :class
        {
            Query queryGet = new WildcardQuery(new Term("Id", "*"));
            List<TEntity> entityList = new List<TEntity>();
            var searcher = _searcherManager.Acquire();
            try
            {
                TopDocs docs = searcher.Search<TEntity>(queryGet, numResults);
                foreach (var scoreDoc in docs.ScoreDocs)
                {
                    Document doc = searcher.Doc(scoreDoc.Doc);
                    TEntity entity = doc.ToObject<TEntity>();
                    entityList.Add(entity);
                }
                return entityList;
            }
            finally
            {
                _searcherManager.Release(searcher);
            }
        }

        public IEnumerable<TEnity> GetAll<TEnity>(string queryString, string[] searchableFileds, int numResults) where TEnity : class
        {
            var searcher = _searcherManager.Acquire();
            try
            {
                BooleanQuery booleanQuery = new BooleanQuery();
                List<Query> queryList = new List<Query>();
                List<TEnity> entityList = new List<TEnity>();
                foreach (var s in searchableFileds)
                {

                    MultiFieldQueryParser queryParser =
                        new MultiFieldQueryParser(MATCH_LUCENE_VERSION, searchableFileds, _analyzer);
                    queryParser.DefaultOperator = QueryParser.OR_OPERATOR;
                    Query query = queryParser.Parse(queryString);
                    var docs = searcher.Search<TEnity>(query, numResults);
                    foreach (var scoreDoc in docs.ScoreDocs)
                    {
                        Document doc = searcher.Doc(scoreDoc.Doc);
                        TEnity entity = doc.ToObject<TEnity>();
                        entityList.Add(entity);
                    }
                }

                return entityList;
            }
            finally
            {
                _searcherManager.Release(searcher);
            }
        }

        public void Insert<TEntity>(TEntity obj) where TEntity : class
        {
            Document doc = obj.ToDocument();
            _writer.AddDocument(doc);
            _writer.Commit();
        }

        public void Update<TEntity>(TEntity obj) where TEntity : class 
        {
            Delete<TEntity>(obj);
            Insert<TEntity>(obj);
        }

        public void Delete<TEntity>(TEntity obj) where TEntity : class
        {

            System.Reflection.PropertyInfo keyFieldInfo = typeof(TEntity).GetProperty("Id");
            string id = keyFieldInfo.GetValue(obj).ToString();
            _writer.DeleteDocuments<TEntity>(new TermQuery(new Term("Id", id)));
            _writer.Commit();
        }

        public void ClearIndex()
        {
            _writer.DeleteAll();
            _writer.Commit();
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