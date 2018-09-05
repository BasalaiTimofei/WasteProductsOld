﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Search;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    /// <summary>
    /// Implementation of ISearchService with Lucene
    /// </summary>
    public class LuceneSearchService : ISearchService
    {

        public const int DEFAULT_MAX_LUCENE_RESULTS = 1000;
        public int MaxResultCount { get; set; } = DEFAULT_MAX_LUCENE_RESULTS;

        private ISearchRepository _repository;

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


        public IEnumerable<Product> SearchProduct(SearchQuery query)
        {
            var productDbList = Search<ProductDB>(query);

            //TODO: map all values in productDbList to Product
            List<Product> result = new List<Product>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDB>()
                    .ForMember(m => m.Created, opt => opt.Ignore())
                    .ForMember(m => m.Modified, opt => opt.Ignore())
                    .ReverseMap();

                cfg.CreateMap<Barcode, BarcodeDB>()
                    .ForMember(m => m.Created, opt => opt.Ignore())
                    .ForMember(m => m.Modified, opt => opt.Ignore())
                    .ReverseMap();

                cfg.CreateMap<Category, CategoryDB>()
                    .ForMember(m => m.Id, opt => opt.Ignore())
                    .ForMember(m => m.Marked, opt => opt.Ignore())
                    .ForMember(m => m.Products, opt => opt.Ignore())
                    .ReverseMap();
            });

            var mapper = config.CreateMapper();

            foreach (var productDb in productDbList)
            {
                result.Add(mapper.Map<Product>(productDb));
            }

            return result;
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
