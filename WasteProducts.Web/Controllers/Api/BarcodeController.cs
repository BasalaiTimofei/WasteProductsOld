using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NLog;
using System.Net.Http;
using System.Web.Http;
using WasteProducts.Logic.Common.Services.Barcods;
using Swagger.Net.Annotations;
using WasteProducts.Logic.Common.Models.Barcods;
using System.Web;
using System.IO;
using System.Drawing;

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
        /// <param name="logger">NLog logger.</param>
        public BarcodeController(IBarcodeScanService scanner, ILogger logger) : base(logger)
        {
            _scanner = scanner;
        }

        /// <summary>
        /// Scan photo of barcode.
        /// </summary>
        /// <param name="upload">The photo of barcode.</param>
        /// <returns>Numerical code of barcode.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get numerical barcode", typeof(string))]
        [HttpPost, Route("getcode")]
        public IHttpActionResult PostCode([FromBody]HttpPostedFileBase upload)
        {
            string code = "";
            using (Stream barcodeStream = upload.InputStream)
            {
                code = _scanner.ScanBySpire(new Bitmap(barcodeStream));
                if(code.Length != 8 || code.Length != 4)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(code);
                }
            }
        }
    }
}
