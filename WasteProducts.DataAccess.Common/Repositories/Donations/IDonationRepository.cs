using System;
using WasteProducts.DataAccess.Common.Models.Donations;

namespace WasteProducts.DataAccess.Common.Repositories.Donations
{
    /// <summary>
    /// This interface provides necessary CRUD methods for donation repository
    /// </summary>
    public interface IDonationRepository : IDisposable
    {
        /// <summary>
        /// Creates new donation in the repository.
        /// </summary>
        /// <param name="user">New donation to add.</param>
        void Create(DonationDB donation);
    }
}