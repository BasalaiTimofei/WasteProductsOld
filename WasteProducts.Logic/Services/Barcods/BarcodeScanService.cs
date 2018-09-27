using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using Spire.Barcode;
using WasteProducts.Logic.Common.Services.Barcods;
using System;
using System.Text.RegularExpressions;

namespace WasteProducts.Logic.Services.Barcods
{
    /// <inheritdoc />
    public class BarcodeScanService : IBarcodeScanService
    {
        private Bitmap _image;
        private Stream _stream;
        private Graphics _graphics;

        /// <inheritdoc />
        public Bitmap Resize(Stream stream, int width, int height)
        {
            Bitmap img = new Bitmap(stream);
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
            _image = Resize(stream, 400, 400);
            var barcodeReader = new BarcodeReader
            {
                Options = new ZXing.Common.DecodingOptions()
                {
                    TryHarder = true
                },
                AutoRotate = true
            };
            var result = barcodeReader.Decode(_image);

            string decoded = result.ToString().Trim();
            if (!IsValid(decoded))
            {
                decoded = null;
            }
            return decoded;
        }

        /// <inheritdoc />
        public string ScanBySpire(Stream stream)
        {
            string decoded = "";
            _image = Resize(stream, 400, 400);

            using (_stream = new MemoryStream())
            {
                _image.Save(_stream, ImageFormat.Bmp);
                decoded = BarcodeScanner.ScanOne(_stream, true);
            }
            if (!IsValid(decoded))
            {
                decoded = null;
            }
            return decoded;
        }

        /// <summary>
        /// String code validation.
        /// </summary>
        /// <param name="code">String code.</param>
        /// <returns>true or false</returns>
        private bool IsValid(string code)
        {
            Regex regex = new Regex(@"\d{13}");
            return regex.IsMatch(code);
        }
    }
}
