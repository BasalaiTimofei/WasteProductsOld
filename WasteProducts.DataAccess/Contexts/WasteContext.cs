using System;
using System.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Contexts.Config;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.ModelConfigurations;
using WasteProducts.DataAccess.ModelConfigurations.UserManagement;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserDB>()
                .HasMany(u => u.Friends)
                .WithMany()
                .Map(t => t.MapLeftKey("UserId")
                    .MapRightKey("FriendId")
                    .ToTable("UserFriends"));

            modelBuilder.Entity<ProductDB>()
                .HasOptional(p => p.Barcode)
                .WithRequired(b => b.Product);

            modelBuilder.Configurations.Add(new UserProductDescriptionConfiguration());

            modelBuilder.Configurations.Add(new GroupBoardConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new GroupUserConfiguration());
            modelBuilder.Configurations.Add(new GroupCommentConfiguration());
            modelBuilder.Configurations.Add(new GroupProductConfiguration());
        }

        /// <summary>
        /// Entity represents many-to-many relationship between User and Product and includes ratings and descriptions of products by specific users.
        /// </summary>
        public IDbSet<UserProductDescriptionDB> UserProductDescriptions { get; set; }

        /// <summary>
        /// Property added for to use an entity set that is used to perform
        ///  create, read, update, delete and to get product list operations in 'ProductRepository' class.
        /// </summary>
        public IDbSet<ProductDB> Products { get; set; }

        /// <summary>
        /// Property added for to use an entity set that is used to perform
        ///  create, read, update, delete and to get category list operations in 'CategoryRepository' class.
        /// </summary>
        public IDbSet<CategoryDB> Categories { get; set; }

        public IDbSet<GroupBoardDB> GroupBordDBs { get; set; }
        public IDbSet<GroupDB> GroupDBs { get; set; }
        public IDbSet<GroupUserDB> GroupUserDBs { get; set; }
        public IDbSet<GroupCommentDB> GroupCommentDBs { get; set; }
        public IDbSet<GroupProductDB> GroupProductDBs { get; set; }
    }
}
