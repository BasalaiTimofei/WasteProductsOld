using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services
{
    public interface IBarcodService
    {
        /// <summary>
        /// get a numerical barcode on the photo
        /// </summary>
        /// <param name="image">image - barcode photo</param>
        /// <returns>a numerical barcode</returns>
        string GetCode(Image image);

        /// <summary>
        /// get product information
        /// </summary>
        /// <param name="code">a numerical barcode</param>
        /// <returns>product information in the format BarcodeInfo</returns>
        BarcodeInfo GetInfo(string code);
    }
}
