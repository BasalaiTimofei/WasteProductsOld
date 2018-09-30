using NUnit.Framework;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Mappings.Barcods;
using Ninject;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using WasteProducts.Logic.Tests.Integrational.Properties;

namespace WasteProducts.Logic.Tests.Barcode_Tests
{
    [TestFixture]
    class BarcodeService_Tests
    {
        private Bitmap _imageGood = Resource.IMG_GoodImage;
        private Bitmap _imageBad = Resource.IMG_BadImage;
        private IBarcodeService _service;
        private StandardKernel _kernel;
        private Barcode _verified;

        [OneTimeSetUp]
        public void Init()
        {
            _kernel = new StandardKernel();
            _kernel.Load(new DataAccess.InjectorModule(), new Logic.InjectorModule());
            _verified = new Barcode()
            {
                Id = null,
                Code = "4810064002096",
                ProductName = "Печенье «Слодыч» с какао 450 г.",
                Composition = "мука пшеничная в/с, пудра сахарная, маргарин, какао-порошок, инвертный сироп, " +
                   "яичный порошок, молоко сухое обезжиренное, пудра ванильная, соль пищевая йодированная, " +
                   "разрыхлители (натрий двууглекислый, углеаммонийная соль), эмульгатор - лактилат натрия, " +
                   "краситель Сахарный колер (натуральная Карамель).",
                Brend = "Слодыч",
                Country = "Беларусь",
                Weight = 0,
                PicturePath = "https://img.e-dostavka.by/UserFiles/images/catalog/Goods/thumbs/4810/4810064002096_450x450_w.png.jpg?1423771900",
                Product = null
            };
        }

        [SetUp]
        public void SetUp()
        {
            _service = _kernel.Get<IBarcodeService>();
        }

        [Test]
        public void GetAsync_Barcode_By_GoodImage()
        {
            //Arrange

            Barcode barcode = new Barcode();

            //Act
            using (Stream stream = new MemoryStream())
            {
                _imageGood.Save(stream, ImageFormat.Bmp);
                barcode = _service.GetBarcodeByStreamAsync(stream).Result;
            }
            
            //Assert
            Assert.AreEqual(_verified.PicturePath, barcode.PicturePath);
        }

        [Test]
        public void GetAsync_Barcode_By_BadImage()
        {
            //Arrange
            Barcode barcode = new Barcode();

            //Act
            using (Stream stream = new MemoryStream())
            {
                _imageBad.Save(stream, ImageFormat.Bmp);
                barcode = _service.GetBarcodeByStreamAsync(stream).Result;
            }

            //Assert
            Assert.AreEqual(null, barcode);
        }
    }
}
