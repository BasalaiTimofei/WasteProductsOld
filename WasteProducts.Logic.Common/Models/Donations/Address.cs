namespace WasteProducts.Logic.Common.Models.Donations
{
    public class Address
    {
        /// <summary>
        /// City of donor's address.
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// Country of donor's address.
        /// </summary>
        string Country { get; set; }

        /// <summary>
        /// State of donor's address.
        /// </summary>
        string State { get; set; }

        /// <summary>
        /// That either a donor address confirmed or not.
        /// </summary>
        bool IsConfirmed { get; set; }

        /// <summary>
        /// Name used with address (included when the donor provides a Gift Address).
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Donor's street address.
        /// </summary>
        string Street { get; set; }

        /// <summary>
        /// Zip code of donor's address.
        /// </summary>
        string Zip { get; set; }
    }
}