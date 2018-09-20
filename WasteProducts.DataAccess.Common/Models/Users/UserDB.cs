using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    /// <summary>
    /// DataBase entity of user.
    /// </summary>
    public class UserDB : IdentityUser
    {
        /// <summary>
        /// List of Users which belong to group of friends related to current User.
        /// </summary>
        public virtual IList<UserDB> Friends { get; set; }

        /// <summary>
        /// List of products added and described by the user.
        /// </summary>
        public virtual IList<UserProductDescriptionDB> ProductDescriptions { get; set; }

        // TODO decomment after the "Groups" model is enabled
        /// <summary>
        /// List of all Groups to which current User is assigned.
        /// </summary>
        //public virtual List<Group> GroupMembership { get; set; }

        /// <summary>
        /// Specifies timestamp of creation of the User in Database.
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Specifies timestamp of modifying of any Property of the User in Database.
        /// </summary>
        public virtual DateTime? Modified { get; set; }
    }
}
