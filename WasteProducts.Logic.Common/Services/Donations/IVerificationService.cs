namespace WasteProducts.Logic.Common.Services.Donations
{
    /// <summary>
    /// Provides the verification method for requests.
    /// </summary>
    public interface IVerificationService
    {
        /// <summary>
        /// Verify a request.
        /// </summary>
        /// <param name="requestString">The request string for verification.</param>
        bool IsVerified(string requestString);
    }
}