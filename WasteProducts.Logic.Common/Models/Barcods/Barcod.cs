using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models.Barcods
{
    class Barcod
    {
        /// <summary>
        /// ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Barcode number.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Barcode type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// User ID of the sending photo.
        /// </summary>
        public int UserID { get; set; }
    }
}
