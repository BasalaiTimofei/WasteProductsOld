

using WasteProducts.Logic.Common.Models.Security.Infrastructure;

namespace WasteProducts.Logic.Common.Models.Security.Models
{
    public class UserLogin : IUserLogin
    {
        /// <summary>
        /// User Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation user property
        /// </summary>
        public IAppUser User { get; set; }

        /// <summary>
        /// Gets or sets the provider 
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user identity user provided by the login provider.
        /// </summary>
        public string ProviderKey { get; set; }
    }
}
