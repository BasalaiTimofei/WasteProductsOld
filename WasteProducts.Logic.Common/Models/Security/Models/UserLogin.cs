

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
        /// 
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProviderKey { get; set; }
    }
}
