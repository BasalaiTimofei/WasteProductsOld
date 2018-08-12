using System.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Contexts.Config;

namespace WasteProducts.DataAccess.Contexts
{
    [DbConfigurationType(typeof(MsSqlConfiguration))]
    public class WasteContext : IdentityDbContext
    {
        public WasteContext()
        {
            Database.Log = (s) => Debug.WriteLine(s);
        }

        public DbSet<UserDB> Users { get; set; }
    }
}