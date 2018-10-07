using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services.Donations
{
    /// <summary>
    /// Provides the verification and log method for PayPal notifications.
    /// </summary>
    public interface IDonationService
    {
        /// <summary>
        /// Asynchronously verify and log the request.
        /// </summary>
        /// <param name="notificationRequestString">String of notification request.</param>
        Task VerifyAndLogAsync(string notificationRequestString);
    }
}