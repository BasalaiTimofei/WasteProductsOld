using System;
using WasteProducts.DataAccess.Common.Models.Donations;
using WasteProducts.DataAccess.Common.Repositories.Donations;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.Donations
{
    public class DonationRepository : IDonationRepository
    {
        private readonly WasteContext _context;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of DonationRepository.
        /// </summary>
        /// <param name="context">The specific context of WasteContext.</param>
        public DonationRepository(WasteContext context) => _context = context;

        /// <summary>
        /// Allows you to log new donation to the database.
        /// </summary>
        /// <param name="donation">The new donation for adding.</param>
        public void Add(DonationDB donation)
        {
            _context.Donations.Add(donation);
            _context.SaveChanges();
        }

        /// <summary>
        /// Use ONLY with TestDB!
        /// </summary>
        public void RecreateTestDatabase()
        {
            _context.Database.Delete();
            _context.Database.Create();
        }

        /// <summary>
        /// Releases all resources used by the DonationRepository.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }
    }
}