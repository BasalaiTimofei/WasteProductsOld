using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Services.Barcods;

namespace WasteProducts.Logic.Tests.Barcode_Tests
{
    [TestFixture]
    class BarcodeCatalogSearchService_Tests
    {
        /// <summary>
        /// инициализируем массив из ДВУХ каталогов для поиска информации о товаре.
        /// 
        /// моделируем успешный поиск в ПЕРВОМ каталоге.
        /// 
        /// для успешного прохождения теста нужно убедиться что:
        /// 1) метод ICatalog.Get() был вызван только у первого каталога 
        /// 2) результат был возвращен ПЕРВЫМ каталогом
        /// </summary>
        [Test]
        public async Task Call_GetAsync_Only_First_Catalog()
        {
            //Arrange

            Barcode productInfo_1 = new Barcode()
            {
                ProductName = "Catalog_1"
            };

            var catalog_1 = new Mock<ICatalog>();

            catalog_1.Setup(f => f.GetAsync("")).
            Returns(Task.FromResult(productInfo_1));

            Barcode productInfo_2 = null;
            var catalog_2 = new Mock<ICatalog>();
            catalog_2.Setup(f => f.GetAsync("")).
                Returns(Task.FromResult(productInfo_2));

            var catalogs = new List<ICatalog>();
            catalogs.Add(catalog_1.Object);
            catalogs.Add(catalog_2.Object);

            var calaogSearcher = new BarcodeCatalogSearchService(catalogs);

            //Act

            var result = await calaogSearcher.GetAsync("");

            //Assert

            catalog_1.Verify(m => m.GetAsync(""), () => Times.Exactly(1));
            catalog_2.Verify(m => m.GetAsync(""), () => Times.Exactly(0));

            Assert.AreEqual(expected: "Catalog_1", actual: result.ProductName);
        }

        /// <summary>
        /// инициализируем массив из ДВУХ каталогов для поиска информации о товаре.
        /// 
        /// моделируем успешный поиск во ВТОРОМ каталоге. 
        ///  
        /// для успешного прохождения теста нужно убедиться что:
        /// 1) метод ICatalog.Get() был вызван у первого и второго каталога
        /// 2) результат был возвращен ВТОРЫМ каталогом
        /// </summary>
        [Test]
        public async Task Call_GetAsync_First_And_Second_Catalogs()
        {
            //Arrange

            Barcode productInfo_1 = null;
            var catalog_1 = new Mock<ICatalog>();
            catalog_1.Setup(f => f.GetAsync("")).
                Returns(Task.FromResult(productInfo_1));

            Barcode productInfo_2 = new Barcode()
            {
                ProductName = "Catalog_2"
            };
            var catalog_2 = new Mock<ICatalog>();
            catalog_2.Setup(f => f.GetAsync("")).
                Returns(Task.FromResult(productInfo_2));

            var catalogs = new List<ICatalog>();
            catalogs.Add(catalog_1.Object);
            catalogs.Add(catalog_2.Object);

            var calaogSearcher = new BarcodeCatalogSearchService(catalogs);

            //Act

            var result = await calaogSearcher.GetAsync("");

            //Assert

            catalog_1.Verify(m => m.GetAsync(""), () => Times.Exactly(1));
            catalog_2.Verify(m => m.GetAsync(""), () => Times.Exactly(1));

            Assert.AreEqual(expected: "Catalog_2", actual: result.ProductName);
        }

        /// <summary>
        /// инициализируем массив из ДВУХ каталогов для поиска информации о товаре.
        /// 
        /// моделируем провальный поиск в обоих каталогах. 
        ///  
        /// для успешного прохождения теста нужно убедиться что:
        /// 1) метод ICatalog.Get() был вызван ДВА раза
        /// 2) результат равен null
        /// </summary>
        [Test]
        public async Task Call_GetAsync_Twice_Catalog()
        {
            //Arrange

            Barcode productInfo_1 = null;
            var catalog_1 = new Mock<ICatalog>();
            catalog_1.Setup(f => f.GetAsync("")).
                Returns(Task.FromResult(productInfo_1));

            Barcode productInfo_2 = null;
            var catalog_2 = new Mock<ICatalog>();
            catalog_2.Setup(f => f.GetAsync("")).
                Returns(Task.FromResult(productInfo_2));

            var catalogs = new List<ICatalog>();
            catalogs.Add(catalog_1.Object);
            catalogs.Add(catalog_2.Object);

            var calaogSearcher = new BarcodeCatalogSearchService(catalogs);

            //Act

            var result = await calaogSearcher.GetAsync("");

            //Assert

            catalog_1.Verify(m => m.GetAsync(""), () => Times.Exactly(1));
            catalog_2.Verify(m => m.GetAsync(""), () => Times.Exactly(1));

            Assert.AreEqual(expected: null, actual: result);
        }
    }
}
