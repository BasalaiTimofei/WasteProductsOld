using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Donations;
using WasteProducts.DataAccess.Common.Repositories.Donations;
using WasteProducts.Logic.Common.Services.Donations;
using WasteProducts.Logic.Common.Services.Mail;
using WasteProducts.Logic.Constants.Donations;
using WasteProducts.Logic.Mappings.Donations;
using WasteProducts.Logic.Services.Donations;

namespace WasteProducts.Logic.Tests.Donation_Tests
{
    [TestFixture]
    public class PayPalServiceTests
    {
        private readonly NameValueCollection _appSettings = ConfigurationManager.AppSettings;
        private readonly IRuntimeMapper _mapper;
        private Mock<IVerificationService> _verificationServiceMock;
        private Mock<IDonationRepository> _donationRepositoryMock;

        public PayPalServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AddressProfile());
                cfg.AddProfile(new DonorProfile());
                cfg.AddProfile(new DonationProfile());
            });
            _mapper = (new Mapper(config)).DefaultContext.Mapper;
        }

        [SetUp]
        public void SetupEachTest()
        {
            _verificationServiceMock = new Mock<IVerificationService>();            
            _verificationServiceMock.Setup(s => s.IsVerifiedAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            _donationRepositoryMock = new Mock<IDonationRepository>();
            _donationRepositoryMock.Setup(r => r.ContainsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));
        }

        [Test]
        public async Task TestFraudTryAsync()
        {
            _verificationServiceMock.Setup(s => s.IsVerifiedAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            await TestAsync().ConfigureAwait(false);

            WhetherAddRepositoryMethodHasBeenCalled(0);
        }

        [Test]
        public async Task TestNotCompletedPaymentAsync()
        {
            await TestAsync(paymentStatus: IPN.Payment.Status.PROCESSED).ConfigureAwait(false);
            WhetherAddRepositoryMethodHasBeenCalled(0);
        }

        [Test]
        public async Task TestWithWrongReceiverEmailAsync()
        {
            await TestAsync("WrongEmail@gmail.com").ConfigureAwait(false);
            WhetherAddRepositoryMethodHasBeenCalled(0);
        }

        [Test]
        public async Task TestAttemptToLogDonationThatHaveAlreadyBeenLoggedAsync()
        {
            _donationRepositoryMock.Setup(r => r.ContainsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            await TestAsync().ConfigureAwait(false);

            WhetherAddRepositoryMethodHasBeenCalled(0);
        }

        [Test]
        public async Task TestCaseWhenYouNeedToLogAsync()
        {
            await TestAsync().ConfigureAwait(false);
            WhetherAddRepositoryMethodHasBeenCalled(1);
        }

        private async Task TestAsync(string receiverEmail = AppSettings.OUR_PAYPAL_EMAIL,
            string paymentStatus = IPN.Payment.Status.COMPLETED)
        {
            PayPalService payPalService = CreatePayPalService();
            string payPalRequestString = GetPayPalRequestString(
                receiverEmail,
                paymentStatus
                );

            await payPalService.VerifyAndLogAsync(payPalRequestString).ConfigureAwait(false);
        }

        private void WhetherAddRepositoryMethodHasBeenCalled(int count)
        {
            _donationRepositoryMock
                .Verify(d => d.AddAsync(It.IsAny<DonationDB>()), Times.Exactly(count));
        }

        private PayPalService CreatePayPalService()
        {
            return new PayPalService(
                _verificationServiceMock.Object,
                _donationRepositoryMock.Object,
                _mapper
                );
        }

        private string GetPayPalRequestString(
            string receiverEmail = AppSettings.OUR_PAYPAL_EMAIL,            
            string paymentStatus = IPN.Payment.Status.COMPLETED,
            string transactionId = "1"
            )
        {
            if (receiverEmail == AppSettings.OUR_PAYPAL_EMAIL)
                receiverEmail = _appSettings[AppSettings.OUR_PAYPAL_EMAIL];
            return "https://ipnpb.paypal.com/cgi-bin/webscr?" +
                "cmd=_notify-validate&" +
                "mc_gross=19.95&" +
                "protection_eligibility=Eligible&" +
                "address_status=confirmed&" +
                "payer_id=LPLWNMTBWMFAY&" +
                "tax=0.00&" +
                "address_street=1+Main+St&" +
                "payment_date=20%3A12%3A59+Jan+13%2C+2009+PST&" +
                "payment_status=" + paymentStatus + "&" +
                "charset=windows-1252&" +
                "address_zip=95131&" +
                "first_name=Test&" +
                "mc_fee=0.88&" +
                "address_country_code=US&" +
                "address_name=Test+User&" +
                "notify_version=2.6&" +
                "custom=&" +
                "payer_status=verified&" +
                "address_country=United+States&" +
                "address_city=San+Jose&" +
                "quantity=1&" +
                "verify_sign=AtkOfCXbDm2hu0ZELryHFjY-Vb7PAUvS6nMXgysbElEn9v-1XcmSoGtf&" +
                "payer_email=gpmac_1231902590_per%40paypal.com&" +
                "txn_id=" + transactionId + "&" +
                "payment_type=instant&" +
                "last_name=User&" +
                "address_state=CA&" +
                "receiver_email=" + receiverEmail + "&" +
                "payment_fee=0.88&" +
                "receiver_id=S8XGHLYDW9T3S&" +
                "txn_type=express_checkout&" +
                "item_name=&" +
                "mc_currency=USD&" +
                "item_number=&" +
                "residence_country=US&" +
                "test_ipn=1&" +
                "handling_amount=0.00&" +
                "transaction_subject=&" +
                "payment_gross=19.95&" +
                "shipping=0.00";
        }
    }
}