using AutoMapper;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Web;
using WasteProducts.DataAccess.Common.Models.Donations;
using WasteProducts.DataAccess.Common.Repositories.Donations;
using WasteProducts.Logic.Common.Models.Donations;
using WasteProducts.Logic.Common.Services.Donations;
using WasteProducts.Logic.Constants.Donations;

namespace WasteProducts.Logic.Services.Donations
{
    /// <inheritdoc />
    class PayPalService : IDonationService
    {
        private readonly NameValueCollection _appSettings = ConfigurationManager.AppSettings;
        private readonly IVerificationService _payPalVerificationService;
        private readonly IDonationRepository _donationRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="payPalVerificationService">IVerificationService implementation that verifies PayPal request.</param>
        /// <param name="donationRepository">IDonationRepository implementation for operations with database.</param>
        /// <param name="mapper">AutoMapper.</param>
        public PayPalService(
            IVerificationService payPalVerificationService,
            IDonationRepository donationRepository,
            IMapper mapper
            )
        {
            _payPalVerificationService = payPalVerificationService;
            _donationRepository = donationRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Verifies that the request comes from PayPal and registers it.
        /// </summary>
        /// <param name="payPalRequestString">String of PayPal request.</param>
        public void VerifyAndLog(string payPalRequestString)
        {
            if (_payPalVerificationService.IsVerified(payPalRequestString))
                ProcessPayPalRequest(payPalRequestString);
        }

        /// <summary>
        /// Processes the PayPal notification.
        /// </summary>
        /// <param name="payPalRequestString"></param>
        private void ProcessPayPalRequest(string payPalRequestString)
        {
            const string COMPLETED = "Completed";
            const string OUR_PAYPAL_EMAIL = "OurPayPalEmail";

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
            DonationDB donationDB = _mapper.Map<DonationDB>(donation);
            _donationRepository.Add(donationDB);
        }

        /// <summary>
        /// Fills the donation object from the PayPal request arguments.
        /// </summary>
        /// <param name="payPalArguments">PayPal arguments.</param>
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

        /// <summary>
        /// Fills the donor object from the PayPal request arguments.
        /// </summary>
        /// <param name="payPalArguments">PayPal arguments.</param>
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

        /// <summary>
        /// Fills the address object from the PayPal request arguments.
        /// </summary>
        /// <param name="payPalArguments">PayPal arguments.</param>
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

        /// <summary>
        /// Converts a date and time string in PayPal format to a DateTime object.
        /// </summary>
        /// <param name="payPalTimeAndDate">PayPal time and date.</param>
        private DateTime ConvertPayPalDateTime(string payPalTimeAndDate)
        {
            const string PAYPAL_TIME_FORMAT = "PayPalTimeFormat";
            const string PAYPAL_SANDBOX_TIME_FORMAT = "ddd MMM dd yyyy HH:mm:ss \"GMT\"zz\"00\"";

            string[] dateFormats = { _appSettings[PAYPAL_TIME_FORMAT], PAYPAL_SANDBOX_TIME_FORMAT };
            DateTime.TryParseExact(
                payPalTimeAndDate,
                dateFormats, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime outputDateTime);
            return outputDateTime;
        }
    }
}