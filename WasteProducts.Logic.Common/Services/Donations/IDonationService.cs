using System.Web;

namespace WasteProducts.Logic.Common.Services.Donations
{
    /// <summary>
    /// This interface provides the verification and registration method for donations.
    /// </summary>
    public interface IDonationService
    {
        /// <summary>
        /// Verify a request and log a new donation.
        /// </summary>
        /// <param name="notificationRequest">Donation notification request.</param>
        void VerifyAndLog(HttpRequestBase notificationRequest);
    }
}