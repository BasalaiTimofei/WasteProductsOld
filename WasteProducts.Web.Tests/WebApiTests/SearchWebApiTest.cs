using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.Logic.Services;
using WasteProducts.Web.Controllers.Api;
using NLog;
using NUnit.Framework.Constraints;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Web.Tests.WebApiTests
{
    [TestFixture]
    public class SearchWebApiTest
    {
        private IEnumerable<Product> products;

        [OneTimeSetUp]
        public void Setup()
        {
            products = new List<Product>
            {
                new Product { Name = "Test Product1 Name1", Description = "Test Product1 Description1"},
                new Product { Name = "Test Product2 Name2", Description = "Test Product2 Description2"},
                new Product { Name = "Test Product3 Name3", Description = "Test Product3 Description3"},
                new Product { Name = "Test Product4 Name4", Description = "Test Product4 Description4"},
                new Product { Name = "Test Product5 Name5 Unique", Description = "Test Product5 Description5"}
            };

        }

        [Test]
        public void SearchControllerTestGetProductWithDefaultFields()
        {
            Mock<ILogger> _mockLogger = new Mock<ILogger>();
            LuceneSearchRepository repo = new LuceneSearchRepository(true);
            LuceneSearchService service = new LuceneSearchService(repo);
            SearchController sut = new SearchController(_mockLogger.Object, service);

            service.AddToSearchIndex<Product>(products);

            var result = sut.GetProductsDefaultFields("test");
            Assert.AreEqual(expected: 5, actual: result.ToArray().Length);

            result = sut.GetProductsDefaultFields("unique");
            Assert.AreEqual(expected: 1, actual: result.ToArray().Length);

            result = sut.GetProductsDefaultFields("lorem");
            Assert.AreEqual(expected: 0, actual: result.ToArray().Length);

            //SearchQuery query = new SearchQuery().SetQueryString("Test").AddField("Name").AddField("Description");
            //var result = service.Search<Product>(query);
        }

        [Test]
        public void SearchControllerTestGetProductWithCustomFields()
        {
            Mock<ILogger> _mockLogger = new Mock<ILogger>();
            LuceneSearchRepository repo = new LuceneSearchRepository(true);
            LuceneSearchService service = new LuceneSearchService(repo);
            SearchController sut = new SearchController(_mockLogger.Object, service);

            service.AddToSearchIndex<Product>(products);

            string[] fields = new string[] {"Description"};


            var result = sut.GetProductsDefaultFields("test");
            Assert.AreEqual(expected: 5, actual: result.ToArray().Length);

            result = sut.GetProducts("unique", fields);
            Assert.AreEqual(expected: 0, actual: result.ToArray().Length);
        }
    }
}
