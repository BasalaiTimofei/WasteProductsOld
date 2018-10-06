using NUnit.Framework;
using System.Drawing;
using WasteProducts.Logic.Services.Barcods;
using ZXing;
using Moq;
using System.IO;
using System.Drawing.Imaging;

namespace WasteProducts.Logic.Tests.Barcode_Tests
{
    /// <summary>
    /// Сводное описание для Barcode_Tests
    /// </summary>
    [TestFixture]
    public class BarcodeScanService_Tests
    {
        private Stream _stream;
        private Bitmap _image;
        private Bitmap _imageGood = Properties.Resources.IMG_GoodImage;
        private Bitmap _imageBad = Properties.Resources.IMG_BadImage;
        private Bitmap _imageOriginal = Properties.Resources.IMG_NotResize;
        private string _verified = "4810064002096";
        private string _verifiedBad = null;

        [Test]
        public void TestMethod_Spire_WithGoodImage()
        {
            //Arrange
            string result = "";
            var service = new BarcodeScanService();

            //Act
            using (_stream = new MemoryStream())
            {
                _imageGood.Save(_stream, ImageFormat.Bmp);
                result = service.ScanBySpire(_stream);
            }

            //Assert
            Assert.AreEqual(_verified, result);
        }

        [Test]
        public void TestMethod_Spire_WithBadImage()
        {
            //Arrange
            string result = "";
            var service = new BarcodeScanService();

            //Act
            using (_stream = new MemoryStream())
            {
                _imageBad.Save(_stream, ImageFormat.Bmp);
                result = service.ScanBySpire(_stream);
            }

            //Assert
            Assert.AreEqual(_verifiedBad, result);
        }

        [Test]
        public void TestMethod_Zxing_WithGoodImage()
        {
            ////Arrange
            //byte[] rawBytes = null;
            //ResultPoint[] resultPoints = null;
            //Result result = new Result(_verified, rawBytes, resultPoints, BarcodeFormat.EAN_13);
            //var mock = new Mock<IBarcodeReader>();

            ////Act
            //mock.Setup(m => m.Decode(_imageGood)).Returns(result);
            //Result resultDecod = mock.Object.Decode(_imageGood);
            //string decoded = mock.Object.Decode(_imageGood).ToString().Trim();

            ////Assert
            //Assert.AreEqual(_verified, decoded);
            //Arrange
            string result = null;
            var service = new BarcodeScanService();

            //Act
            using (_stream = new MemoryStream())
            {
                _imageGood.Save(_stream, ImageFormat.Bmp);
                result = service.ScanByZxing(_stream);
            }

            //Assert
            Assert.AreEqual(_verified, result);
        }

        [Test]
        public void TestMethod_Zxing_WithBadImage()
        {
            //Arrange
            string result = null;
            var service = new BarcodeScanService();

            //Act
            using (_stream = new MemoryStream())
            {
                _imageBad.Save(_stream, ImageFormat.Bmp);
                result = service.ScanByZxing(_stream);
            }

            //Assert
            Assert.AreEqual(_verifiedBad, result);
        }

        [Test]
        public void TestResizeImage()
        {
            //Arrange
            var service = new BarcodeScanService();

            //Act
            using (_stream = new MemoryStream())
            {
                _imageOriginal.Save(_stream, ImageFormat.Bmp);
                using (var result = service.Resize(_stream))
                    _image = new Bitmap(result);
            }

            //Assert
            Assert.AreEqual(_imageGood.Width, _image.Width);
            Assert.AreEqual(_imageGood.Height, _image.Height);
        }
    }
}
