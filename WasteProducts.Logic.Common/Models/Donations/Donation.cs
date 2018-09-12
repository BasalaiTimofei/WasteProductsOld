using System;

namespace WasteProducts.Logic.Common.Models.Donations
{
    /// <summary>
    /// Represents a donation.
    /// </summary>
    public class Donation
    {
        /// <summary>
        /// Unique identifier for a specific donation.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Specifies the date of donation.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Specifies the gross of donation.
        /// </summary>
        public decimal Gross { get; set; }

        /// <summary>
        /// Specifies the currency code.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Transaction fee associated with the donation.
        /// Gross minus Fee equals the amount deposited into the receiver E-Mail account.
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// Specifies the donor.
        /// </summary>
        public Donor Donor { get; set; }
    }
}