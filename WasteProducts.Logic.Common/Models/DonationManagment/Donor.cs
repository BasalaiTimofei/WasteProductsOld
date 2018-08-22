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
        /// Address of donor.
        /// </summary>
        Address Address { get; set; }
    }
}