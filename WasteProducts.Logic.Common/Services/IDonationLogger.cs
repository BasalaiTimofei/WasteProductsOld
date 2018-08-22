using WasteProducts.Logic.Common.Models.DonationManagment;

namespace WasteProducts.Logic.Common.Services
{
    interface IDonationLogger
    {
        void Log(Donation donation);
    }
}