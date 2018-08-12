using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Barcods
{
    class BarcodeDB
    {
        /// <summary>
        /// ID.
        /// </summary>
        public Guid Id { get; set; }

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

        /// <summary>
        /// Date of record creation in DB.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Date of record modified in DB.
        /// </summary>
        public DateTime? Modified { get; set; }
    }
}
