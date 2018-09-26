// <copyright file="User.cs">
//    2017 - Johan Boström
// </copyright>

using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.IdentityServer.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}