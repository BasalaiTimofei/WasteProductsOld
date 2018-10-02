using System;
using WasteProducts.DataAccess.Common.Models.Donations;

namespace WasteProducts.DataAccess.Common.Repositories.Donations
{
    /// <summary>
    /// Provides necessary CRUD methods for donation repository
    /// </summary>
    public interface IDonationRepository : IDisposable
    {
        /// <summary>
        /// Allows you to log new donation to the database.
        /// </summary>
        /// <param name="donation">The new donation for adding.</param>
        void Add(DonationDB donation);

        /// <summary>
        /// Determines whether the repository contains an donation with the specified id.
        /// </summary>
        /// <param name="user">The id of DonationDB object.</param>
        /// <returns><c>true</c> if the source repository contains an element that has the specified id; otherwise, <c>false</c>.</returns>
        bool Contains(string id);
    }
}