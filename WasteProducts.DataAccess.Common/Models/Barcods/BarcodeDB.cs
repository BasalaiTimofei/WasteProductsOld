using System;
using WasteProducts.DataAccess.Common.Models.Products;

namespace WasteProducts.DataAccess.Common.Models.Barcods
{
    public class BarcodeDB
    {
        /// <summary>
        /// Id.
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
        /// Product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Product brend.
        /// </summary>
        public string Brend { get; set; }

        /// <summary>
        /// Product country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Product weight.
        /// </summary>
        public double Weight { get; set; }

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

        /// <summary>
        /// Specifies the concreat product
        /// </summary>
        //public virtual ProductDB Product { get; set; }
    }
}
