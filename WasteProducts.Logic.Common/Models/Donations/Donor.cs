namespace WasteProducts.Logic.Common.Models.Donations
{
    /// <summary>
    /// Represents a donor.
    /// </summary>
    public class Donor
    {
        /// <summary>
        /// Unique donor ID.
        /// </summary>
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
        /// Address of donor.
        /// </summary>
        public Address Address { get; set; }
    }
}