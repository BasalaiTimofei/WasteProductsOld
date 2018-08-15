using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Common.Services
{
    public interface IBarcodService
    {
        /// <summary>
        /// get a numerical barcode on the photo
        /// </summary>
        /// <param name="image">image - barcode photo</param>
        /// <param name="UserId">User Id by User</param>
        /// <returns>a numerical barcode</returns>
        BarcodeInfo GetCode(Image image, int UserId);

        /// <summary>
        /// get product information
        /// </summary>
        /// <param name="barcodeInfo">a model jf BarcodeInfo</param>
        /// <returns>product information in the format Barcode</returns>
        Barcod GetBarcode(BarcodeInfo barcodeInfo);
    }
}
