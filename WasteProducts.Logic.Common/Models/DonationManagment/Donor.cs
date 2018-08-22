namespace WasteProducts.Logic.Common.Models.DonationManagment
{
    class Donor
    {
        /// <summary>
        /// Unique donor ID.
        /// </summary>
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
        /// City of donor's address.
        /// </summary>
        string AddressCity { get; set; }

        /// <summary>
        /// Country of donor's address.
        /// </summary>
        string AddressCountry { get; set; }

        /// <summary>
        /// State of donor's address.
        /// </summary>
        string AddressState { get; set; }

        /// <summary>
        /// That either a donor address confirmed or not.
        /// </summary>
        bool IsAddressConfirmed { get; set; }

        /// <summary>
        /// Name used with address (included when the donor provides a Gift Address).
        /// </summary>
        string AddressName { get; set; }

        /// <summary>
        /// Donor's street address.
        /// </summary>
        string AddressStreet { get; set; }

        /// <summary>
        /// Donor's street address.
        /// </summary>
        string AddressZip { get; set; }
    }
}