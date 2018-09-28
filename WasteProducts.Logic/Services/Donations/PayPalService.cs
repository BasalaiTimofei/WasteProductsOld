using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using WasteProducts.Logic.Common.Models.Donations;
using WasteProducts.Logic.Common.Services.Donations;
using WasteProducts.Logic.Constants.Donations;

namespace WasteProducts.Logic.Services.Donations
{
    class PayPalService : IDonationService
    {
        private readonly NameValueCollection _appSettings = ConfigurationManager.AppSettings;

        public void VerifyAndLog(HttpRequestBase notificationRequest)
        {
            VerifyRequest(notificationRequest);
        }

        /// <summary>
        /// Verifies that the request came from PayPal.
        /// </summary>
        private void VerifyRequest(HttpRequestBase payPalRequest)
        {
            var param = payPalRequest.BinaryRead(payPalRequest.ContentLength);
            var payPalRequestString = Encoding.ASCII.GetString(param);



            ProcessVerificationResponse(verificationResponse, payPalRequestString);
        }



        /// <summary>
        /// Processes a verification response.
        /// </summary>
        private void ProcessVerificationResponse(string verificationResponse, string payPalRequestString)
        {
            const string COMPLETED = "Completed";
            const string OUR_PAYPAL_EMAIL = "OurPayPalEmail";
            const string INVALID = "INVALID";

            if (verificationResponse.Equals(INVALID))
                return; // It was fraud try.

            // check that Payment_status=Completed
            // check that Txn_id has not been previously processed
            // check that Receiver_email is your Primary PayPal email
            // check that Payment_amount/Payment_currency are correct
            // process payment
            NameValueCollection payPalArguments = HttpUtility.ParseQueryString(payPalRequestString);
            if (payPalArguments[IPN.Payment.PAYMENT_STATUS] != COMPLETED ||
                _appSettings[OUR_PAYPAL_EMAIL] != payPalArguments[IPN.Transaction.RECEIVER_EMAIL])
                return;

            Donation donation = FillDonation(payPalArguments);
        }

        private Donation FillDonation(NameValueCollection payPalArguments)
        {
            return new Donation
            {
                Donor = FillDonor(payPalArguments),
                TransactionId = payPalArguments[IPN.Transaction.TXN_ID],
                Date = ConvertPayPalDateTime(payPalArguments[IPN.Payment.PAYMENT_DATE]),
                Gross = Convert.ToDecimal(payPalArguments[IPN.Payment.MC_GROSS]),
                Currency = payPalArguments[IPN.Payment.MC_CURRENCY],
                Fee = Convert.ToDecimal(payPalArguments[IPN.Payment.MC_FEE])
            };
        }

        private Donor FillDonor(NameValueCollection payPalArguments)
        {
            const string VERIFIED = "verified";

            return new Donor
            {
                Address = FillAddress(payPalArguments),
                Id = payPalArguments[IPN.Buyer.PAYER_ID],
                Email = payPalArguments[IPN.Buyer.PAYER_EMAIL],
                IsVerified = payPalArguments[IPN.Payment.PAYER_STATUS] == VERIFIED,
                FirstName = payPalArguments[IPN.Buyer.FIRST_NAME],
                LastName = payPalArguments[IPN.Buyer.LAST_NAME]
            };
        }

        private Address FillAddress(NameValueCollection payPalArguments)
        {
            const string CONFIRMED = "confirmed";

            return new Address
            {
                City = payPalArguments[IPN.Buyer.ADDRESS_CITY],
                Country = payPalArguments[IPN.Buyer.ADDRESS_COUNTRY],
                State = payPalArguments[IPN.Buyer.ADDRESS_STATE],
                IsConfirmed = payPalArguments[IPN.Buyer.ADDRESS_STATUS] == CONFIRMED,
                Name = payPalArguments[IPN.Buyer.ADDRESS_NAME],
                Street = payPalArguments[IPN.Buyer.ADDRESS_STREET],
                Zip = payPalArguments[IPN.Buyer.ADDRESS_ZIP]
            };
        }

        private DateTime ConvertPayPalDateTime(string payPalDateTime)
        {
            const string PAYPAL_TIME_FORMAT = "PayPalTimeFormat";
            const string PAYPAL_SANDBOX_TIME_FORMAT = "ddd MMM dd yyyy HH:mm:ss \"GMT\"zz\"00\"";

            string[] dateFormats = { _appSettings[PAYPAL_TIME_FORMAT], PAYPAL_SANDBOX_TIME_FORMAT };
            DateTime.TryParseExact(
                payPalDateTime,
                dateFormats, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime outputDateTime);
            return outputDateTime;
        }
    }
}