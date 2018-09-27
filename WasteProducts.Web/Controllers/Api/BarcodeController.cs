using System.Net;
using Ninject.Extensions.Logging;
using System.Web.Http;
using WasteProducts.Logic.Common.Services.Barcods;
using Swagger.Net.Annotations;
using WasteProducts.Logic.Common.Models.Barcods;
using System.IO;
using System.Threading.Tasks;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Controller that performs actions on barcode.
    /// </summary>
    [RoutePrefix("api/barcode")]
    public class BarcodeController : BaseApiController
    {
        private Barcode _barcode = null;
        private readonly IBarcodeScanService _scanner;
        private readonly IBarcodeCatalogSearchService _searcher;

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
        /// <param name="uploadStream">The photo of barcode.</param>
        /// <returns>Numerical code of barcode.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get numerical barcode", typeof(string))]
        [HttpPost, Route("onlyread")]
        public async Task<IHttpActionResult> GetCodeByStreamPhotoAsync(Stream uploadStream)
        {
            string code = "";
            await Task.Run(() =>
            {  
                code = _scanner.ScanBySpire(uploadStream);
            });
            return Ok(code);
        }

        /// <summary>
        /// Parsing e-dostavca.
        /// </summary>
        /// <param name="code">Numerical barcode.</param>
        /// <returns>Model of Barcode.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get barcode", typeof(Barcode))]
        [HttpPost, Route("{code}")]
        public async Task<IHttpActionResult> GetBarcodeByCodeAsync(string code)
        {
            _barcode = await _searcher.GetAsync(code);      
            return Ok(_barcode);
        }

        /// <summary>
        /// Scan photo of barcode and return a model of Barcode.
        /// </summary>
        /// <param name="uploadStream">Photo stream barcode.</param>
        /// <returns>Model of Barcode.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get barcode", typeof(Barcode))]
        [HttpPost, Route("read")]
        public async Task<IHttpActionResult> GetBarcodeAsync(Stream uploadStream)
        {
            string code = "";
            code = _scanner.ScanBySpire(uploadStream);
            _barcode = await _searcher.GetAsync(code);
            return Ok(_barcode);
        }
    }
}
