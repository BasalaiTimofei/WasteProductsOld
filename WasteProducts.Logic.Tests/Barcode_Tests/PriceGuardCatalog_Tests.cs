using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Services.Barcods;

namespace WasteProducts.Logic.Tests.Barcode_Tests
{
    [TestFixture]
    class PriceGuardCatalog_Tests
    {
        /// <summary>
        /// моделируем провальный поиск в каталоге:
        /// 1) на сайте нет товара, поэтому, на шаге загрузки страницы поисковой выдачи
        /// возвращаем 404 код и пустую HTML страницу.
        /// 
        /// для успешного прохождения теста нужно убедиться что:
        /// 1) результат равен null 
        /// </summary>
        [Test]
        public async Task TestMethod_Search_On_The_Site_There_Is_No_Product_Response_404_Result_Is_Null()
        {
            //Arrange

            HttpQueryResult emptySearchOutput = new HttpQueryResult()
            {
                StatusCode = 404,
                Page = ""
            };

            var httpHelper = new Mock<IHttpHelper>();
            httpHelper.SetupSequence(f => f.SendGETAsync("https://priceguard.ru/search?q="))
                .Returns(Task.FromResult(emptySearchOutput));

            PriceGuardCatalog catalog = new PriceGuardCatalog(httpHelper.Object);

            //Act

            var result = await catalog.GetAsync("");

            //Assert

            Assert.AreEqual(expected: null, actual: result);
        }

        /// <summary>
        /// моделируем провальный поиск в каталоге:
        /// 1)на сайте нет товара, поэтому, на шаге загрузки страницы поисковой выдачи
        /// возвращаем 200 код и пустую HTML страницу.
        /// 
        /// для успешного прохождения теста нужно убедиться что:
        /// 1) результат равен null 
        /// </summary>
        [Test]
        public async Task TestMethod_Search_On_The_Site_There_Is_No_Produc_Response_200_Result_Is_Null()
        {
            //Arrange

            HttpQueryResult emptySearchOutput = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = ""
            };

            var httpHelper = new Mock<IHttpHelper>();
            httpHelper.SetupSequence(f => f.SendGETAsync("https://priceguard.ru/search?q="))
                .Returns(Task.FromResult(emptySearchOutput));

            PriceGuardCatalog catalog = new PriceGuardCatalog(httpHelper.Object);

            //Act

            var result = await catalog.GetAsync("");

            //Assert

            Assert.AreEqual(expected: null, actual: result);
        }

        /// <summary>
        /// моделируем провальную попытку получения страницы описания товара:
        /// 1) на сайте есть товара, поэтому, на шаге загрузки страницы поисковой выдачи
        /// возвращаем 200 код и HTML страницу, где можно выпарсить ссылку на страницу описания товара.
        /// 2) но позже что-то идет не так и загрузить страницу с описанием товара не получается
        /// тк происходит 502 ошибка на сервере веб каталога.
        /// 
        /// для успешного прохождения теста нужно убедиться что:
        /// 1) результат равен null 
        /// </summary>
        [Test]
        public async Task TestMethod_Search_On_The_Site_There_Is_Product_Response_200_Then_502_Result_Is_Null()
        {
            //Arrange

            HttpQueryResult searchOutput = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = @"<a title=""Перейти на страницу с описанием"" href=""./offer/ozon-137673735"">"
            };

            HttpQueryResult productPage = new HttpQueryResult()
            {
                StatusCode = 502,
                Page = @""
            };

            var httpHelper = new Mock<IHttpHelper>();
            httpHelper.Setup(f => f.SendGETAsync("https://priceguard.ru/search?q="))
                .Returns(Task.FromResult(searchOutput));
            httpHelper.Setup(f => f.SendGETAsync("https://priceguard.ru/offer/ozon-137673735"))
                .Returns(Task.FromResult(productPage));

            PriceGuardCatalog catalog = new PriceGuardCatalog(httpHelper.Object);

            //Act

            var result = await catalog.GetAsync("");

            //Assert

            Assert.AreEqual(expected: null, actual: result);
        }

        /// <summary>
        /// моделируем провальную попытку получения страницы описания товара:
        /// 1) на сайте есть товара, поэтому, на шаге загрузки страницы поисковой выдачи
        /// возвращаем 200 код и HTML страницу, где можно выпарсить ссылку на страницу описания товара.
        /// 2) но позже что-то идет не так и загрузить страницу с описанием товара получается частично
        /// и она не содержит минимальной информации (название товара).
        /// 
        /// для успешного прохождения теста нужно убедиться что:
        /// 1) результат равен null 
        /// </summary>
        [Test]
        public async Task TestMethod_Search_On_The_Site_There_Is_Product_Response_200_No_ProductName_Result_Is_Null()
        {
            //Arrange

            HttpQueryResult searchOutput = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = @"<a title=""Перейти на страницу с описанием"" href=""./offer/ozon-137673735"">"
            };

            HttpQueryResult productPage = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = @""
            };

            var httpHelper = new Mock<IHttpHelper>();
            httpHelper.Setup(f => f.SendGETAsync("https://priceguard.ru/search?q="))
                .Returns(Task.FromResult(searchOutput));
            httpHelper.Setup(f => f.SendGETAsync("https://priceguard.ru/offer/ozon-137673735"))
                .Returns(Task.FromResult(productPage));

            PriceGuardCatalog catalog = new PriceGuardCatalog(httpHelper.Object);

            //Act

            var result = await catalog.GetAsync("");

            //Assert

            Assert.AreEqual(expected: null, actual: result);
        }

        /// <summary>
        /// моделируем успешный поиск в каталоге.
        /// 
        /// для успешного прохождения теста нужно убедиться что:
        /// 1) результат не равен null
        /// 2) результат не содержит пустых полей (все парсеры отработали верно)
        /// 3) метод IHttpHelper.DownloadPicture() был вызван 1 раз
        /// </summary>
        [Test]
        public async Task TestMethod_Search_On_The_Site_Successful_Search()
        {
            //Arrange

            HttpQueryResult searchOutput = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = @"<a title=""Перейти на страницу с описанием"" href=""./offer/ozon-137673735"">"
            };

            HttpQueryResult productPage = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = @"<h1 class=""op-title"">Имя</h1>
<tr>\n<td class=""vat nw"">\n<span class=""op-param-name"">Производитель</span>\n</td>\n<td class=""pl15 vat"">\n<span>\n\n<span>\n<a class=""op-param-link"" href=""../search?g=food&amp;q=Nesquik&amp;t=vendor"" title=""Найти все предложения с «Nesquik»""><span class=""op-param-value nw"">Марка</span></a>\n</span>\n\n</span>\n</td>\n</tr>
<tr>\n<td class=""vat nw"">\n<span class=""op-param-name"">Страна-изготовитель</span>\n</td>\n<td class=""pl15 vat"">\n<span>\n\n<span>\n<a class=""op-param-link"" href=""../search?g=1121180086&amp;q=%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F&amp;t=country"" title=""Найти все предложения с «Россия»""><span class=""op-param-value nw"">Страна</span></a>\n</span>\n\n</span>\n</td>\n</tr><tr>\n<td class=""vat nw"">\n<span class=""op-param-name"">Срок годности</span>\n</td>\n<td class=""pl15 vat"">\n<span class=""op-param-value"">9 мес.</span>\n</td>\n</tr>
<img class=""op-image"" src=""ссылкаНаКартинку"" "
            };

            var httpHelper = new Mock<IHttpHelper>();
            httpHelper.Setup(f => f.SendGETAsync("https://priceguard.ru/search?q="))
                .Returns(Task.FromResult(searchOutput));
            httpHelper.Setup(f => f.SendGETAsync("https://priceguard.ru/offer/ozon-137673735"))
                .Returns(Task.FromResult(productPage));

            PriceGuardCatalog catalog = new PriceGuardCatalog(httpHelper.Object);

            //Act

            var result = await catalog.GetAsync("");

            //Assert

            Assert.IsTrue(result != null);
            Assert.AreEqual(expected: "Имя", actual: result.ProductName);
            Assert.AreEqual(expected: "Марка", actual: result.Brend);
            Assert.AreEqual(expected: "Страна", actual: result.Country);
            Assert.AreEqual(expected: "", actual: result.Composition);
            httpHelper.Verify(m => m.DownloadPictureAsync("ссылкаНаКартинку"), () => Times.Exactly(1));
        }
    }
}
