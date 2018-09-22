using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using Spire.Barcode;
using WasteProducts.Logic.Common.Services.Barcods;
using System;

namespace WasteProducts.Logic.Services.Barcods
{
    /// <inheritdoc />
    public class BarcodeScanService : IBarcodeScanService
    {
        private Bitmap _image;
        private Stream _stream;
        private Graphics _graphics;

        /// <inheritdoc />
        public Bitmap Resize(Bitmap img, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (_graphics = Graphics.FromImage(result))
            {
                _graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                _graphics.DrawImage(img, 0, 0, width, height);
                _graphics.Dispose();
            }
            return result;
        }

        /// <inheritdoc />
        public string ScanByZxing(Stream stream)
        {
            string decoded = "";
            _image = new Bitmap(stream);
            _image = Resize(_image, 400, 400);

            BarcodeReader Reader = new BarcodeReader();
            Result result = Reader.Decode(_image);
            decoded = result.ToString().Trim();

            return decoded;
        }

        /// <inheritdoc />
        public string ScanBySpire(Stream stream)
        {
            string decoded = "";
            _image = new Bitmap(stream);
            _image = Resize(_image, 400, 400);

            using (_stream = new MemoryStream())
            {
                _image.Save(_stream, ImageFormat.Bmp);
                decoded = BarcodeScanner.ScanOne(_stream, true);
            }

            return decoded;
        }
    }
}
