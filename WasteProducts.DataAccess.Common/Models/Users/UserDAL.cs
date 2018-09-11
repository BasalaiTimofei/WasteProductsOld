using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    /// <summary>
    /// Data access level model of user.
    /// </summary>
    public class UserDAL
    {
        /// <summary>
        /// Unique key for the user
        /// </summary>
        public virtual string Id { get; set ; }

        /// <summary>
        /// Unique username
        /// </summary>
        public virtual string UserName { get ; set ; }

        /// <summary>
        /// List of Users which belong to group of friends related to current User
        /// </summary>
        public virtual IList<UserDB> Friends { get; set; }

        /// <summary>
        /// List of products added and described by the user
        /// </summary>
        public virtual IList<UserProductDescriptionDB> ProductDescriptions { get; set; }
    }
}
