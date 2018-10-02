using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Specialized;
using System.Configuration;
using WasteProducts.DataAccess.Common.Models.Donations;
using WasteProducts.DataAccess.Common.Repositories.Donations;
using WasteProducts.Logic.Common.Services.Donations;
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
            _verificationServiceMock.Setup(s => s.IsVerified(It.IsAny<string>()))
                .Returns(true);

            _donationRepositoryMock = new Mock<IDonationRepository>();
            _donationRepositoryMock.Setup(r => r.Contains(It.IsAny<string>()))
                .Returns(false);
        }

        [Test]
        public void TestFraudTry()
        {
            _verificationServiceMock.Setup(s => s.IsVerified(It.IsAny<string>()))
                .Returns(false);

            Test();

            WhetherAddRepositoryMethodHasBeenCalled(0);
        }

        [Test]
        public void TestNotCompletedPayment()
        {
            Test(paymentStatus: IPN.Payment.Status.PROCESSED);
            WhetherAddRepositoryMethodHasBeenCalled(0);
        }

        [Test]
        public void TestWithWrongReceiverEmail()
        {
            Test("WrongEmail@gmail.com");
            WhetherAddRepositoryMethodHasBeenCalled(0);
        }

        [Test]
        public void TestAttemptToLogDonationThatHaveAlreadyBeenLogged()
        {
            _donationRepositoryMock.Setup(r => r.Contains(It.IsAny<string>()))
                .Returns(true);

            Test();

            WhetherAddRepositoryMethodHasBeenCalled(0);
        }

        [Test]
        public void TestCaseWhenYouNeedToLog()
        {
            Test();
            WhetherAddRepositoryMethodHasBeenCalled(1);
        }

        private void Test(string receiverEmail = AppSettings.OUR_PAYPAL_EMAIL,
            string paymentStatus = IPN.Payment.Status.COMPLETED)
        {
            PayPalService payPalService = CreatePayPalService();
            string payPalRequestString = GetPayPalRequestString(
                receiverEmail,
                paymentStatus
                );

            payPalService.VerifyAndLog(payPalRequestString);
        }

        private void WhetherAddRepositoryMethodHasBeenCalled(int count)
        {
            _donationRepositoryMock
                .Verify(d => d.Add(It.IsAny<DonationDB>()), Times.Exactly(count));
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