namespace WasteProducts.Logic.Common.Services.Donations
{
    /// <summary>
    /// This interface provides the verification method for requests.
    /// </summary>
    public interface IVerificationService
    {
        /// <summary>
        /// Verify a request.
        /// </summary>
        /// <param name="notificationRequest">The request string for verification.</param>
        bool IsVerified(string requestString);
    }
}