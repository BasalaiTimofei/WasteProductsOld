using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using System.Drawing;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Services.Barcods;

namespace WasteProducts.Logic.Tests.Barcode_Tests
{
    [TestFixture]
    class EDostavkaCatalog_Tests
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
        public void Get_Test01()
        {
            //Arrange

            HttpQueryResult emptySearchOutput = new HttpQueryResult()
            {
                StatusCode = 404,
                Page = ""
            };

            var httpHelper = new Mock<IHttpHelper>();
            httpHelper.SetupSequence(f => f.SendGETAsync("https://e-dostavka.by/search/?searchtext="))
                .Returns(emptySearchOutput);

            EDostavkaCatalog catalog = new EDostavkaCatalog(httpHelper.Object);

            //Act

            var result = catalog.GetAsync("");

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
        public void Get_Test02()
        {
            //Arrange

            HttpQueryResult emptySearchOutput = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = ""
            };

            var httpHelper = new Mock<IHttpHelper>();
            httpHelper.SetupSequence(f => f.SendGETAsync("https://e-dostavka.by/search/?searchtext="))
                .Returns(emptySearchOutput);

            EDostavkaCatalog catalog = new EDostavkaCatalog(httpHelper.Object);

            //Act

            var result = catalog.GetAsync("");

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
        public void Get_Test03()
        {
            //Arrange

            HttpQueryResult searchOutput = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = @"<!--/noindex--><div class=""img""><a href=""https://e-dostavka.by/catalog/item_628616.html"" class=""fancy_ajax"">"
            };

            HttpQueryResult productPage = new HttpQueryResult()
            {
                StatusCode = 502,
                Page = @""
            };

            var httpHelper = new Mock<IHttpHelper>();
            httpHelper.Setup(f => f.SendGETAsync("https://e-dostavka.by/search/?searchtext="))
                .Returns(searchOutput);
            httpHelper.Setup(f => f.SendGETAsync("https://e-dostavka.by/catalog/item_628616.html"))
                .Returns(productPage);

            EDostavkaCatalog catalog = new EDostavkaCatalog(httpHelper.Object);

            //Act

            var result = catalog.GetAsync("");

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
        public void Get_Test04()
        {
            //Arrange

            HttpQueryResult searchOutput = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = @"<!--/noindex--><div class=""img""><a href=""https://e-dostavka.by/catalog/item_628616.html"" class=""fancy_ajax"">"
            };

            HttpQueryResult productPage = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = @""
            };

            var httpHelper = new Mock<IHttpHelper>();
            httpHelper.Setup(f => f.SendGETAsync("https://e-dostavka.by/search/?searchtext="))
                .Returns(searchOutput);
            httpHelper.Setup(f => f.SendGETAsync("https://e-dostavka.by/catalog/item_628616.html"))
                .Returns(productPage);

            EDostavkaCatalog catalog = new EDostavkaCatalog(httpHelper.Object);

            //Act

            var result = catalog.GetAsync("");

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
        public void Get_Test05()
        {
            //Arrange

            HttpQueryResult searchOutput = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = @"<!--/noindex--><div class=""img""><a href=""https://e-dostavka.by/catalog/item_628616.html"" class=""fancy_ajax"">"
            };

            HttpQueryResult productPage = new HttpQueryResult()
            {
                StatusCode = 200,
                Page = @"<h1>Имя</h1>
<div class=""title"">Описание</div><table><tr class=""property_3220""><td class=""name"">Состав</td><td class=""value"">Состав</td></tr><tr class=""property_3221""><td class=""name"">Краткое описание</td><td class=""value"">Охлажденная.</td></tr></table></div>
<li class=""product_card_country""><strong>Страна производства:</strong><span>Страна</span></li><li><strong>Торговая марка:</strong><span>Марка</span></li>
<a class=""increaseImage no_click"" href=""https://img.e-dostavka.by/UserFiles/images/catalog/Goods/thumbs/4811/4811040118787_1000x1000""><img class=""retina_redy"" src=""ссылкаНаКартинку"" alt=""Колбаса вареная «Мортаделла» высшего сорта, 650 г.""/></a>"
            };
            
            var httpHelper = new Mock<IHttpHelper>();
            httpHelper.Setup(f => f.SendGETAsync("https://e-dostavka.by/search/?searchtext="))
                .Returns(searchOutput);
            httpHelper.Setup(f => f.SendGETAsync("https://e-dostavka.by/catalog/item_628616.html"))
                .Returns(productPage);

            EDostavkaCatalog catalog = new EDostavkaCatalog(httpHelper.Object);

            //Act

            var result = catalog.GetAsync("");

            //Assert

            Assert.IsTrue(result != null);
            Assert.AreEqual(expected: "Имя", actual: result.ProductName);
            Assert.AreEqual(expected: "Марка", actual: result.Brend);
            Assert.AreEqual(expected: "Страна", actual: result.Country);
            Assert.AreEqual(expected: "Состав", actual: result.Composition);
            httpHelper.Verify(m => m.DownloadPictureAsync("ссылкаНаКартинку"), () => Times.Exactly(1));
        }
    }
}
