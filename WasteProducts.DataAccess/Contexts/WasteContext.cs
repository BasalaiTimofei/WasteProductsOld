using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Contexts.Config;

namespace WasteProducts.DataAccess.Contexts
{
    [DbConfigurationType(typeof(MsSqlConfiguration))]
    public class WasteContext : IdentityDbContext
    {
        /// <summary>
        /// property added for to use an entity set that is used to perform
        ///  create, read, update, delete and to get product list operations in 'ProductRepository' class.
        /// </summary>
        public IDbSet<ProductDB> Products { get; set; }

        public DbSet<GroupBoardDB> GroupBordDBs { get; set; }
        public DbSet<GroupDB> GroupDBs { get; set; }
        public DbSet<GroupUserDB> GroupUserDBs { get; set; }
        public DbSet<GroupUserInviteTimeDB> GroupUserInviteTimeDBs { get; set; }
        public DbSet<GroupProductBoardDB> GroupProductBoardDBs { get; set; }
    }
}
