using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.Logic.Services;
using WasteProducts.Web.Controllers.Api;
using NLog;
using WasteProducts.DataAccess.Common.Models.Products;

namespace WasteProducts.Web.Tests.WebApiTests
{
    [TestFixture]
    public class SearchWebApiTest
    {
        private IEnumerable<ProductDB> products;

        [OneTimeSetUp]
        public void Setup()
        {
            products = new List<ProductDB>
            {
                new ProductDB { Name = "Test Product1 Name1", Description = "Test Product1 Description1"},
                new ProductDB { Name = "Test Product2 Name2", Description = "Test Product2 Description2"},
                new ProductDB { Name = "Test Product3 Name3", Description = "Test Product3 Description3"},
                new ProductDB { Name = "Test Product4 Name4", Description = "Test Product4 Description4"},
                new ProductDB { Name = "Test Product5 Name5 Unique", Description = "Test Product5 Description5"}
            };

        }

        [Test]
        public async Task SearchControllerTestGetProductWithDefaultFields()
        {
            Mock<ILogger> _mockLogger = new Mock<ILogger>();
            LuceneSearchRepository repo = new LuceneSearchRepository(true);
            LuceneSearchService service = new LuceneSearchService(repo);
            SearchController sut = new SearchController(service, _mockLogger.Object);

            service.AddToSearchIndex<ProductDB>(products);

            var result = await sut.GetProductsDefaultFields("test");
            Assert.AreEqual(expected: 5, actual: result.ToArray().Length);

            result = await sut.GetProductsDefaultFields("unique");
            Assert.AreEqual(expected: 1, actual: result.ToArray().Length);

            result = await sut.GetProductsDefaultFields("lorem");
            Assert.AreEqual(expected: 0, actual: result.ToArray().Length);

        }
    }
}
