using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using Spire.Barcode;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    /// <inheritdoc />
    public class BarcodeScanService : IBarcodeScanService
    {
        /// <inheritdoc />
        public Bitmap Resize(Bitmap img, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, width, height);
                g.Dispose();
            }
            return result;
        }

        /// <inheritdoc />
        public string ScanByZxing(Bitmap image)
        {
            string decoded = "";
            BarcodeReader Reader = new BarcodeReader();
            Result result = Reader.Decode(image);
            decoded = result.ToString().Trim();

            return decoded;
        }

        /// <inheritdoc />
        public string ScanBySpire(Bitmap image)
        {
            string decoded = "";
            using (Stream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Bmp);
                decoded = BarcodeScanner.ScanOne(stream, true);
            }
            return decoded;
        }
    }
}
