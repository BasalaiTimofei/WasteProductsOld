using System;
using System.ComponentModel.DataAnnotations;

namespace WasteProducts.DataAccess.Common.Models.DonationManagment
{
    class DonorDB
    {
        /// <summary>
        /// Unique donor ID.
        /// </summary>
        [Key]
        string Id { get; set; }

        /// <summary>
        /// Donor's primary email address.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// That either a donor verified or not.
        /// </summary>
        bool IsVerified { get; set; }

        /// <summary>
        /// Account holder's first name.
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Account holder's last name.
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Address of donor.
        /// </summary>
        AddressDB Address { get; set; }

        /// <summary>
        /// Specifies the timestamp for creating of a specific donor in the database.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Specifies the timestamp for modifying of a specific donor in the database.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
    }
}