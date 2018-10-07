using Newtonsoft.Json;
using System;
using WasteProducts.DataAccess.Common.Models.Products;

namespace WasteProducts.DataAccess.Common.Models.Barcods
{
    /// <summary>
    /// DataBase entity of barcode.
    /// </summary>
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
        [JsonIgnore]
        public virtual ProductDB Product { get; set; }
    }
}
