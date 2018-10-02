namespace WasteProducts.Logic.Common.Models.Donations
{
    /// <summary>
    /// Represents an address of a donor.
    /// </summary>
    public class Address
    {
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
    }
}