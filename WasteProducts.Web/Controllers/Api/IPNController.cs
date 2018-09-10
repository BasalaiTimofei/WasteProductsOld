using NLog;
using Swagger.Net.Annotations;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Controller that receives Instant Payment Notifications from PayPal.
    /// </summary>
    [RoutePrefix("api/IPN")]
    public class IPNController : BaseApiController
    {
        private IDonationService _donationService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="donationService">Donation service</param>
        /// <param name="logger">NLog logger</param>
        public IPNController(IDonationService donationService, ILogger logger) : base(logger)
        {
            _donationService = donationService;
        }

        /// <summary>
        /// Receives Instant Payment Notifications from PayPal.
        /// </summary>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get search result collection", typeof(IEnumerable<Product>))]
        [HttpPost, Route("receive")]
        public OkResult Receive([FromUri]string query)
        {
            return Ok();
        }
    }
}