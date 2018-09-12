using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Donations;

namespace WasteProducts.DataAccess.Common.Repositories.Donations
{
    /// <summary>
    /// This interface provides necessary CRUD methods for donation repository
    /// </summary>
    public interface IDonationRepository : IDisposable
    {
        /// <summary>
        /// Returns all donations in an enumerable.
        /// </summary>
        /// <returns>All the donations.</returns>
        IEnumerable<DonationDB> GetDonationList();

        /// <summary>
        /// Returns the donation by its payment number.
        /// </summary>
        /// <param name="paymentNo">Payment number of the requested donation.</param>
        /// <returns>Donation with the specific payment number.</returns>
        DonationDB GetDonation(int paymentNo);

        /// <summary>
        /// Creates new donation in the repository.
        /// </summary>
        /// <param name="user">New donation to add.</param>
        void Create(DonationDB donation);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        void Save();
    }
}