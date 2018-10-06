using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services.Donations
{
    /// <summary>
    /// Provides the verification method for requests.
    /// </summary>
    public interface IVerificationService
    {
        /// <summary>
        /// Asynchronously verify a request.
        /// </summary>
        /// <param name="requestString">The request string for verification.</param>
        Task<bool> IsVerifiedAsync(string requestString);
    }
}