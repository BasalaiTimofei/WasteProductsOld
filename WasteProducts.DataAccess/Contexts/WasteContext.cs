using System.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Contexts.Config;

namespace WasteProducts.DataAccess.Contexts
{
    [DbConfigurationType(typeof(MsSqlConfiguration))]
    public class WasteContext : IdentityDbContext<UserDB, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public WasteContext()
        {
            Database.Log = (s) => Debug.WriteLine(s);
        }

        public WasteContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.Log = (s) => Debug.WriteLine(s);
        }

        /// <summary>
        /// property added for to use an entity set that is used to perform
        ///  create, read, update, delete and to get product list operations in 'ProductRepository' class.
        /// </summary>
        public IDbSet<ProductDB> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserDB>()
                .HasMany(u => u.Friends)
                .WithMany()
                .Map(t => t.MapLeftKey("UserId")
                           .MapRightKey("FriendId")
                           .ToTable("UserFriends"));

            modelBuilder.Entity<UserDB>()
                .HasMany(u => u.Products)
                .WithMany(p => p.Users)
                .Map(t => t.MapLeftKey("UserId")
                           .MapRightKey("ProductId")
                           .ToTable("UserProducts"));
        }

        public IDbSet<GroupBoardDB> GroupBordDBs { get; set; }
        public IDbSet<GroupDB> GroupDBs { get; set; }
        public IDbSet<GroupUserDB> GroupUserDBs { get; set; }
        public IDbSet<GroupUserInviteTimeDB> GroupUserInviteTimeDBs { get; set; }
        public IDbSet<GroupProductDB> GroupProductDBs { get; set; }
    }
}
