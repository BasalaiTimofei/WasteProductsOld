using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    /// <summary>
    /// Represents a role of users.
    /// </summary>
    public class UserRoleDB
    {
        /// <summary>
        /// Id of the role.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the role.
        /// </summary>
        public string Name { get; set; }
    }
}
