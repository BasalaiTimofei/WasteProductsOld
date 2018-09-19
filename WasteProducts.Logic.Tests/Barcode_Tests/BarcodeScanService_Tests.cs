using NUnit.Framework;
using System.Drawing;
using WasteProducts.Logic.Services.Barcods;
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
        private Bitmap _imageGood = Properties.Resources.IMG_GoodImage;
        private Bitmap _imageBad = Properties.Resources.IMG_BadImage;
        private Bitmap _imageOriginal = Properties.Resources.IMG_NotResize;
        private string _verified = "4810064002096";
        private string _verifiedBad = "";

        [Test]
        public void TestMethod_Spire_WithGoodImage()
        {
            //Arrange
            var service = new BarcodeScanService();

            //Act
            string result = service.ScanBySpire(_imageGood);

            //Assert
            Assert.AreEqual(_verified, result);
        }

        [Test]
        public void TestMethod_Spire_WithBadImage()
        {
            //Arrange
            var service = new BarcodeScanService();

            //Act
            string result = service.ScanBySpire(_imageBad);

            //Assert
            Assert.AreNotEqual(_verified, result);
        }

        [Test]
        public void TestMethodMock_Zxing_WithGoodImage()
        {
            //Arrange
            byte[] rawBytes = null;
            ResultPoint[] resultPoints = null;
            Result result = new Result(_verified, rawBytes, resultPoints, BarcodeFormat.EAN_13);
            var mock = new Mock<IBarcodeReader>();

            //Act
            mock.Setup(m => m.Decode(_imageGood)).Returns(result);
            Result resultDecod = mock.Object.Decode(_imageGood);
            string decoded = mock.Object.Decode(_imageGood).ToString().Trim();

            //Assert
            Assert.AreEqual(_verified, decoded);
        }

        [Test]
        public void TestMethodMock_Zxing_WithBadImage()
        {
            //Arrange
            byte[] rawBytes = null;
            ResultPoint[] resultPoints = null;
            Result result = new Result(_verifiedBad, rawBytes, resultPoints, BarcodeFormat.EAN_13);
            var mock = new Mock<IBarcodeReader>();

            //Act
            mock.Setup(m => m.Decode(_imageBad)).Returns(result);
            Result resultDecod = mock.Object.Decode(_imageBad);
            string decoded = mock.Object.Decode(_imageBad).ToString().Trim();

            //Assert
            Assert.AreEqual(_verifiedBad, decoded);
        }

        [Test]
        public void TestResizeImage()
        {
            //Arrange
            var service = new BarcodeScanService();

            //Act
            Image result = service.Resize(_imageOriginal, 400, 400);

            //Assert
            Assert.AreEqual(_imageGood.Width, result.Width);
            Assert.AreEqual(_imageGood.Height, result.Height);
        }
    }
}
