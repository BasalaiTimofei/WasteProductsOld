using System;

namespace WasteProducts.Logic.Common.Models.Donation
{
    class Donation
    {
        /// <summary>
        /// Unique identifier for a specific donation.
        /// </summary>
        public int PaymentNo { get; set; }

        /// <summary>
        /// Specifies the date of donation.
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Specifies the amount of donation.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Specifies the currency code.
        /// </summary>
        public int Currency { get; set; }
    }
}