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

        /// <summary>
        /// Processes a verification response.
        /// </summary>
        private void ProcessVerificationResponse(string verificationResponse, string payPalRequestString)
        {
            const string INVALID = "INVALID";
            
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
            if (payPalArguments[IPN.PAYMENT_STATUS] != COMPLETED)
                return;
            string user_email = payPalArguments["payer_email"];            
        }
    }

    /// <summary>
    /// Represents PayPal IPN variables.
    /// </summary>
    public static class IPN
    {
        public const string PAYMENT_STATUS = "payment_status";
    }

    /// <summary>
    /// Represents Transaction information variables from a IPN request.
    /// </summary>
    public static class Transaction
    {
        /// <summary>
        /// Email address or account ID of the payment recipient(that is, the merchant).
        /// Equivalent to the values of receiver_email(if payment is sent to primary account)
        /// and business set in the Website Payment HTML.
        /// Note: The value of this variable is normalized to lowercase characters.
        /// </summary>
        public const string BUSINESS = "business";

        /// <summary>
        /// Character set.
        /// </summary>
        public const string CHARSET = "charset";

        /// <summary>
        /// Custom value as passed by you, the merchant.
        /// These are pass-through variables that are never presented to your customer.
        /// Length: 255 characters.
        /// </summary>
        public const string CUSTOM = "custom";

        /// <summary>
        /// Internal; only for use by MTS.
        /// </summary>
        public const string IPN_TRACK_ID = "ipn_track_id";

        /// <summary>
        /// Message's version number.
        /// </summary>
        public const string NOTIFY_VERSION = "notify_version";

        /// <summary>
        /// In the case of a refund, reversal, or canceled reversal,
        /// this variable contains the "txn_id" of the original transaction,
        /// while "txn_id" contains a new ID for the new transaction.
        /// Length: 19 characters.
        /// </summary>
        public const string PARENT_TXN_ID = "parent_txn_id";

        /// <summary>
        /// Unique ID generated during guest checkout (payment by credit card without logging in).
        /// </summary>
        public const string RECEIPT_ID = "receipt_id";

        /// <summary>
        /// Primary email address of the payment recipient (that is, the merchant).
        /// If the payment is sent to a non-primary email address on your PayPal account,
        /// the receiver_email is still your primary email.
        /// Note: The value of this variable is normalized to lowercase characters.
        /// Length: 127 characters.
        /// </summary>
        public const string RECEIVER_EMAIL = "receiver_email";

        /// <summary>
        /// Unique account ID of the payment recipient (i.e., the merchant).
        /// This is the same as the recipient's referral ID. 
        /// Length: 13 characters.
        /// </summary>
        public const string RECEIVER_ID = "receiver_id";

        /// <summary>
        /// Whether this IPN message was resent (equals true);
        /// otherwise, this is the original message.
        /// </summary>
        public const string RESEND = "resend";

        /// <summary>
        /// ISO 3166 country code associated with the country of residence.
        /// Length: 2 characters.
        /// </summary>
        public const string RESIDENCE_COUNTRY = "residence_country";

        /// <summary>
        /// Whether the message is a test message. Value is:
        /// 1 — the message is directed to the Sandbox.
        /// </summary>
        public const string TEST_IPN = "test_ipn";

        /// <summary>
        /// The merchant's original transaction identification number for the payment from the buyer,
        /// against which the case was registered.
        /// </summary>
        public const string TXN_ID = "txn_id";

        /// <summary>
        /// The kind of transaction for which the IPN message was sent.
        /// </summary>
        public const string TXN_TYPE = "txn_type";

        /// <summary>
        /// Encrypted string used to validate the authenticity of the transaction.
        /// </summary>
        public const string VERIFY_SIGN = "verify_sign";
    }

    /// <summary>
    /// Represents Buyer information variables from a IPN request.
    /// </summary>
    public static class Buyer
    {
        /// <summary>
        /// Country of customer's address.
        /// Length: 64 characters.
        /// </summary>
        public const string ADDRESS_COUNTRY = "address_country";

        /// <summary>
        /// City of customer's address.
        /// Length: 40 characters.
        /// </summary>
        public const string ADDRESS_CITY = "address_city";

        /// <summary>
        /// ISO 3166 country code associated with customer's address.
        /// Length: 2 characters.
        /// </summary>
        public const string ADDRESS_COUNTRY_CODE = "address_country_code";

        /// <summary>
        /// Name used with address(included when the customer provides a Gift Address).
        /// Length: 128 characters.
        /// </summary>
        public const string ADDRESS_NAME = "address_name";

        /// <summary>
        /// State of customer's address.
        /// Length: 40 characters.
        /// </summary>
        public const string ADDRESS_STATE = "address_state";

        /// <summary>
        /// Whether the customer provided a confirmed address. Value is:
        /// "confirmed" — Customer provided a confirmed address;
        /// "unconfirmed" — Customer provided an unconfirmed address.
        /// </summary>
        public const string ADDRESS_STATUS = "address_status";

        /// <summary>
        /// Customer's street address.
        /// Length: 200 characters.
        /// </summary>
        public const string ADDRESS_STREET = "address_street";

        /// <summary>
        /// Zip code of customer's address.
        /// Length: 20 characters.
        /// </summary>
        public const string ADDRESS_ZIP = "address_zip";

        /// <summary>
        /// Customer's telephone number.
        /// Length: 20 characters.
        /// </summary>
        public const string CONTACT_PHONE = "contact_phone";

        /// <summary>
        /// Customer's first name.
        /// Length: 64 characters.
        /// </summary>
        public const string FIRST_NAME = "first_name";

        /// <summary>
        /// Customer's last name.
        /// Length: 64 characters.
        /// </summary>
        public const string LAST_NAME = "last_name";

        /// <summary>
        /// Customer's company name, if customer is a business.
        /// Length: 127 characters.
        /// </summary>
        public const string PAYER_BUSINESS_NAME = "payer_business_name";

        /// <summary>
        /// Customer's primary email address. Use this email to provide any credits.
        /// Length: 127 characters.
        /// </summary>
        public const string PAYER_EMAIL = "payer_email";

        /// <summary>
        /// Unique customer ID.
        /// Length: 13 characters.
        /// </summary>
        public const string PAYER_ID = "payer_id";
    }
}