using System;
using System.Collections.Generic;

namespace WasteProducts.DataAccess.Common.Models.DonationManagment
{
    public class AddressDB
    {
        /// <summary>
        /// Unique address ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// City of donor's address.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Country of donor's address.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// State of donor's address.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// That either a donor address confirmed or not.
        /// </summary>
        public bool IsConfirmed { get; set; }

        /// <summary>
        /// Name used with address (included when the donor provides a Gift Address).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Donor's street address.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Zip code of donor's address.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Specifies all the donors who live at.
        /// </summary>
        public virtual ICollection<DonorDB> Donors { get; set; }

        /// <summary>
        /// Specifies the timestamp for creating of a specific address in the database.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}