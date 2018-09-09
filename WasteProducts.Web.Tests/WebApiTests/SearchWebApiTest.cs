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
                new Product { Name = "Test Product1 Name1", Composition = "Test Product1 Composition1"},
                new Product { Name = "Test Product2 Name2", Composition = "Test Product2 Composition2"},
                new Product { Name = "Test Product3 Name3", Composition = "Test Product3 Composition3"},
                new Product { Name = "Test Product4 Name4", Composition = "Test Product4 Composition4"},
                new Product { Name = "Test Product5 Name5 Unique", Composition = "Test Product5 Composition5"}
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

            string[] fields = new string[] {"Composition"};


            var result = sut.GetProducts("test", fields);
            Assert.AreEqual(expected: 5, actual: result.ToArray().Length);

            result = sut.GetProducts("unique", fields);
            Assert.AreEqual(expected: 0, actual: result.ToArray().Length);
        }
    }
}
