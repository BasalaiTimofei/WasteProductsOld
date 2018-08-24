using WasteProducts.Logic.Common.Models.DonationManagment;

namespace WasteProducts.Logic.Common.Services
{
    interface IDonationLogger
    {
        /// <summary>
        /// Log new donation.
        /// </summary>
        /// <param name="donation">New donation to log.</param>
        void Log(Donation donation);
    }
}