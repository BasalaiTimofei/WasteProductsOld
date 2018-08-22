using System;
using System.ComponentModel.DataAnnotations;

namespace WasteProducts.DataAccess.Common.Models.DonationManagment
{
    class DonationDB
    {
        /// <summary>
        /// Unique identifier for a specific donation.
        /// </summary>
        [Key]
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

        /// <summary>
        /// Specifies the timestamp for creating of a specific donation in the database.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}