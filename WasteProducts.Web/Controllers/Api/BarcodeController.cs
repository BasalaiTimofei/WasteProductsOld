using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Ninject.Extensions.Logging;
using System.Net.Http;
using System.Web.Http;
using WasteProducts.Logic.Common.Services.Barcods;
using Swagger.Net.Annotations;
using WasteProducts.Logic.Common.Models.Barcods;
using System.Web;
using System.IO;
using System.Drawing;
using WasteProducts.Logic.Services.Barcods;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Controller that performs actions on barcode.
    /// </summary>
    [RoutePrefix("api/barcode")]
    public class BarcodeController : BaseApiController
    {
        private readonly IBarcodeScanService _scanner;
        private readonly IBarcodeCatalogSearchService _searcher;
        private readonly ICatalog _catalog;
        private readonly IHttpHelper _httpHelper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scanner">BarcodeScan service.</param>
        /// <param name="searcher"></param>
        /// <param name="logger">NLog logger.</param>
        public BarcodeController(IBarcodeScanService scanner, IBarcodeCatalogSearchService searcher, ILogger logger) : base(logger)
        {
            _scanner = scanner;
            _searcher = searcher;
        }

        /// <summary>
        /// Scan photo of barcode.
        /// </summary>
        /// <param name="upload">The photo of barcode.</param>
        /// <returns>Numerical code of barcode.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get numerical barcode", typeof(string))]
        [HttpPost, Route("getcode")]
        public IHttpActionResult PostCode(HttpPostedFileBase upload)
        {
            string code = "";
            using (Stream barcodeStream = upload.InputStream)
            {
                code = _scanner.ScanBySpire(new Bitmap(barcodeStream));
                if (code.Length != 8 || code.Length != 4)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(code);
                }
            }
        }

        /// <summary>
        /// Parsing e-dostavca.
        /// </summary>
        /// <param name="code">Numerical barcode.</param>
        /// <returns>Model of Barcode.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get barcode", typeof(Barcode))]
        [HttpPost, Route("{code}")]
        public IHttpActionResult GetBarcode(string code)
        {
            Barcode catalog = _searcher.Get(code);      
            return Ok(catalog);
        }
    }
}
