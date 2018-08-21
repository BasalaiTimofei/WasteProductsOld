using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    public interface IUserDb : IUser<int>
    {
        int AccessFailedCount { get; set; }
        ICollection<IClaimDb> Claims { get; set; }
        DateTime CreateDate { get; set; }
        string Email { get; set; }
        bool EmailConfirmed { get; set; }
        bool LockoutEnabled { get; set; }
        DateTime? LockoutEndDateUtc { get; set; }
        ICollection<IUserLoginDb> Logins { get; set; }
        string PasswordHash { get; set; }
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        ICollection<IRoleDb> Roles { get; set; }
        string SecurityStamp { get; set; }
        bool TwoFactorEnabled { get; set; }
    }
}