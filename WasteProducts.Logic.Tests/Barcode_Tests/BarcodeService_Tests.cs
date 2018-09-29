using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Services.Barcods;
using Ninject;
using WasteProducts.DataAccess.Common.Repositories.Barcods;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace WasteProducts.Logic.Tests.Barcode_Tests
{
    [TestFixture]
    class BarcodeService_Tests
    {
        private Bitmap _imageGood = Properties.Resources.IMG_GoodImage;
        private IBarcodeService _service;
        private StandardKernel _kernel;

        [OneTimeSetUp]
        public void Init()
        {
            _kernel = new StandardKernel();
            _kernel.Load(new DataAccess.InjectorModule(), new Logic.InjectorModule());

            //Barcode barcode = new Barcode()
            //{
            //    Id = null,
            //    Code = 
            //    ProductName
            //    Composition
            //    Brend
            //    Country
            //    Weight
            //    PinturePath
            //    Product
            //};
        }

        //[OneTimeTearDown]
        //public void LastTearDown()
        //{
        //    _kernel?.Dispose();
        //}

        [SetUp]
        public void SetUp()
        {
            _service = _kernel.Get<IBarcodeService>();
        }

        [Test]
        public async Task Call_GetAsync_Only_First_Catalog()
        {
            //Arrange
            Barcode barcode = new Barcode();
            var mock = new Mock<IBarcodeService>();

            //Act
            using (Stream stream = new MemoryStream())
            {
                _imageGood.Save(stream, ImageFormat.Bmp);
                barcode = await _service.GetBarcodeByStreamAsync(stream);
            }

            //Assert
            Assert.AreNotEqual(null, barcode);
        }
    }
}
