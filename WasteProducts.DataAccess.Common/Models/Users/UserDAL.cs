using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    public class UserDAL
    {
        public virtual string Id { get; set ; }
        public virtual string UserName { get ; set ; }
        public virtual IList<UserDB> Friends { get; set; }
        public virtual IList<UserProductDescriptionDB> ProductDescriptions { get; set; }

        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }

        public virtual ICollection<IdentityUserRole> Roles { get; set; }
        public virtual ICollection<IdentityUserClaim> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin> Logins { get; set; }

        public virtual DateTime Created { get; set; }
        public virtual DateTime? Modified { get; set; }
    }
}
