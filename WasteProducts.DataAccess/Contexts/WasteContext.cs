using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Contexts.Config;

namespace WasteProducts.DataAccess.Contexts
{
    [DbConfigurationType(typeof(MsSqlConfiguration))]
    public class WasteContext : IdentityDbContext
    {
        public DbSet<GroupBordDB> GroupBordDBs { get; set; }
        public DbSet<GroupDB> GroupDBs { get; set; }
        public DbSet<GroupUserDB> GroupUserDBs { get; set; }
        public DbSet<ProductBoardDB> ProductBordDBs { get; set; }
    }
}