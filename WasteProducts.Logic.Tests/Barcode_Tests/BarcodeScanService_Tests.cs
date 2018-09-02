using NUnit.Framework;
using System.Drawing;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Services.BarcodeService;
using ZXing;
using Moq;

namespace WasteProducts.Logic.Tests.Barcode_Tests
{
    /// <summary>
    /// Сводное описание для Barcode_Tests
    /// </summary>
    [TestFixture]
    public class BarcodeScanService_Tests
    {
        public Image _image = Image.FromFile("S:\\IMG_GoodImage.jpg");
        public Image _imageOriginal = Image.FromFile("S:\\IMG_NotResize.jpg");
        public BarcodeInfo _info = new BarcodeInfo { Code = "4810064002096", Type = "EAN_13" };
        public string _verified = "4810064002096";

        [Test]
        public void TestMethod_Spire_WithGoodImage()
        {
            //Arrange
            var service = new BarcodeScanService();

            //Act
            string result = service.ScanBySpire(_image);

            //Assert
            Assert.AreEqual(_verified, result);
        }

        [Test]
        public void TestZxing()
        {
            //Arrange
            byte[] rawBytes = null;
            ResultPoint[] resultPoints = null;
            Result result = new Result(_verified, rawBytes, resultPoints, BarcodeFormat.EAN_13);
            var mock = new Mock<IBarcodeReader>();

            //Act
            mock.Setup(m => m.Decode((Bitmap)_image)).Returns(result);
            Result resultDecod = mock.Object.Decode((Bitmap)_image);
            string decoded = mock.Object.Decode((Bitmap)_image).ToString().Trim();

            //Assert
            Assert.AreEqual(_verified, decoded);
        }

        [Test]
        public void TestResizeImage()
        {
            //Arrange
            var service = new BarcodeScanService();

            //Act
            Image result = service.Resize(_imageOriginal, 400, 400);

            //Assert
            Assert.AreEqual(_image.Width, result.Width);
            Assert.AreEqual(_image.Height, result.Height);
        }
    }
}
