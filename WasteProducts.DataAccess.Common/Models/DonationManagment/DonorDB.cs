using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WasteProducts.DataAccess.Common.Models.DonationManagment
{
    public class DonorDB
    {
        /// <summary>
        /// Unique donor ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Donor's primary email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// That either a donor verified or not.
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Account holder's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Account holder's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Specifies the foreign key.
        /// </summary>
        [ForeignKey("Address")]
        public Guid AddressId { get; set; }

        /// <summary>
        /// Address of donor.
        /// </summary>
        public virtual AddressDB Address { get; set; }

        /// <summary>
        /// Specifies all donor donations.
        /// </summary>
        public virtual ICollection<DonationDB> Donations { get; set; }

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