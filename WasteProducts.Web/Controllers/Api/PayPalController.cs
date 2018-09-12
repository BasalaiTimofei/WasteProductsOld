using NLog;
using Swagger.Net.Annotations;
using System.Collections.Specialized;
using System.IO;
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
        /// Receives Instant Payment Notifications from PayPal.
        /// </summary>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Instant Payment Notification from PayPal was received.")]
        [HttpPost, Route("donation/log")]
        public OkResult Receive([FromUri]string query)
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            HttpRequestBase request = context.Request;
            Task.Run(() => VerifyRequest(request));
            return Ok();
        }

        /// <summary>
        /// Verifies that the request came from PayPal.
        /// </summary>
        private void VerifyRequest(HttpRequestBase payPalRequest)
        {
            var param = payPalRequest.BinaryRead(payPalRequest.ContentLength);
            var payPalRequestString = Encoding.ASCII.GetString(param);

            HttpWebRequest verificationRequest = PrepareVerificationRequest(payPalRequestString);

            // Send the request to PayPal and get the response
            string verificationResponse = null;
            using (var streamIn = new StreamReader(verificationRequest.GetResponse().GetResponseStream()))
                verificationResponse = streamIn.ReadToEnd();

            ProcessVerificationResponse(verificationResponse, payPalRequestString);
        }

        /// <summary>
        /// Prepares a verification request.
        /// </summary>
        private HttpWebRequest PrepareVerificationRequest(string payPalRequestString)
        {
            const string PAYPAL_URL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            const string VERIFICATION_PREFIX = "cmd=_notify-validate&";
            const string POST = "POST";
            const string CONTENT_TYPE = "application/x-www-form-urlencoded";

            HttpWebRequest verificationRequest = (HttpWebRequest)WebRequest.Create(PAYPAL_URL);

            // Set values for the verification request
            verificationRequest.Method = POST;
            verificationRequest.ContentType = CONTENT_TYPE;

            // Add cmd=_notify-validate to the payload
            string verificationString = VERIFICATION_PREFIX + payPalRequestString;
            verificationRequest.ContentLength = verificationString.Length;

            // Attach payload to the verification request
            using (var streamOut = new StreamWriter(verificationRequest.GetRequestStream(), Encoding.ASCII))
                streamOut.Write(verificationString);

            return verificationRequest;
        }

        private void ProcessVerificationResponse(string verificationResponse, string payPalRequestString)
        {
            const string INVALID = "INVALID";
            const string PAYMENT_STATUS = "payment_status";
            const string COMPLETED = "Completed";

            if (verificationResponse.Equals(INVALID))
                return;

            // check that Payment_status=Completed
            // check that Txn_id has not been previously processed
            // check that Receiver_email is your Primary PayPal email
            // check that Payment_amount/Payment_currency are correct
            // process payment
            NameValueCollection payPalArguments = HttpUtility.ParseQueryString(payPalRequestString);
            //.  more args as needed look at the list from paypal IPN doc
            if (payPalArguments[PAYMENT_STATUS] != COMPLETED)
                return;
            string user_email = payPalArguments["payer_email"];            
        }
    }
}