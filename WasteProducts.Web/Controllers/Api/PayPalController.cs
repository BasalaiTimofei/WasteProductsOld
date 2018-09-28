using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using WasteProducts.Logic.Common.Services.Donations;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Controller that receives Instant Payment Notifications from PayPal.
    /// </summary>
    [RoutePrefix("api/paypal")]
    public class PayPalController : BaseApiController
    {
        private readonly IDonationService _donationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="donationService">Donation service</param>
        /// <param name="logger">NLog logger</param>
        public PayPalController(IDonationService donationService, ILogger logger) : base(logger)
        {
            _donationService = donationService;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalController(ILogger logger) : base(logger)
        {
        }

        /// <summary>
        /// Receives Instant Payment Notifications from PayPal.
        /// </summary>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Instant Payment Notification from PayPal was received.")]
        [HttpPost, Route("donation/log")]
        public OkResult Receive()
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            HttpRequestBase payPalRequest = context.Request;
            Task.Run(() => VerifyAndLog(payPalRequest));
            return Ok();
        }

        /// <summary>
        /// Verify and log the notification.
        /// </summary>
        /// <param name="payPalRequest">PayPal request.</param>
        private void VerifyAndLog(HttpRequestBase payPalRequest)
        {
            byte[] param = payPalRequest.BinaryRead(payPalRequest.ContentLength);
            string payPalRequestString = Encoding.ASCII.GetString(param);
            _donationService.VerifyAndLog(payPalRequestString);
        }
    }
}