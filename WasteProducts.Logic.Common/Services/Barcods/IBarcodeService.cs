using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;
using System.IO;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    public interface IBarcodeService
    {
        Barcode Get(Stream imageStream);
    }
}
