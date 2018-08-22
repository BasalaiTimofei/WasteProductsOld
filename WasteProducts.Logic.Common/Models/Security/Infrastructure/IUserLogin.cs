using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.Logic.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the IUserLogin.
    /// </summary>
    public interface IUserLogin
    {
        /// <summary>
        /// User Id
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// Navigation user property
        /// </summary>
        IAppUser User { get; set; }

        /// <summary>
        /// Gets or sets the provider 
        /// </summary>
        string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user identity user provided by the login provider.
        /// </summary>
        string ProviderKey { get; set; }
    }
}
