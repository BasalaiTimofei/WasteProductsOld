using System.Drawing;
using System.Drawing.Drawing2D;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services;
using ZXing;

namespace WasteProducts.Logic.Services.BarcodeService
{
    public class BarcodeService : IBarcodeService
    {
        public Image Resize(Image img, int width, int height)
        {
            Image result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, width, height);
                g.Dispose();
            }
            return result;
        }

        public BarcodeInfo ScanByZxing(Image image)
        {
            BarcodeReader Reader = new BarcodeReader();
            Result result = Reader.Decode((Bitmap)image);
            BarcodeInfo barcode = new BarcodeInfo
            {
                Code = result.ToString().Trim(),
                Type = result.BarcodeFormat.ToString(),
            };
            string type = result.BarcodeFormat.ToString();
            string decoded = result.ToString().Trim();
            return barcode;
        }
    }
}
