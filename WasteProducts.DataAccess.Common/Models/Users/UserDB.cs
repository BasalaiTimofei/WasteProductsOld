using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Products;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    /// <summary>
    /// DAL level model of user.
    /// </summary>
    public class UserDB : IdentityUser
    {
        /// <summary>
        /// List of Users which belong to group of friends related to current User.
        /// </summary>
        public virtual IList<UserDB> Friends { get; set; }

        /// <summary>
        /// List of Products which User have ever captured.
        /// </summary>
        public virtual IList<ProductDB> Products { get; set; }

        // TODO decomment after the "Groups" model is enabled
        /// <summary>
        /// List of all Groups to which current User is assigned.
        /// </summary>
        //public virtual List<Group> GroupMembership { get; set; }

        /// <summary>
        /// Specifies timestamp of creation of concrete User in Database.
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Specifies timestamp of modifying of any Property of User in Database.
        /// </summary>
        public virtual DateTime? Modified { get; set; }
    }
}
