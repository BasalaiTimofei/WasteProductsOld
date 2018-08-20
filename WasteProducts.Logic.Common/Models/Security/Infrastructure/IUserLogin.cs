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
        /// 
        /// </summary>
        string LoginProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string ProviderKey { get; set; }
    }
}
