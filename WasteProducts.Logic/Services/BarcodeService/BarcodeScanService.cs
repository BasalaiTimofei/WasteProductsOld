using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services;
using ZXing;
using Spire.Barcode;

namespace WasteProducts.Logic.Services.BarcodeService
{
    public class BarcodeScanService : IBarcodeScanService
    {
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

        public string ScanByZxing(Bitmap image)
        {
            string decoded = "";
            BarcodeReader Reader = new BarcodeReader();
            Result result = Reader.Decode(image);
            decoded = result.ToString().Trim();

            return decoded;
        }

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
