using System;
using System.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Contexts.Config;
using System.Linq;
using System.Collections.Generic;
using System;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.ModelConfigurations;
using WasteProducts.DataAccess.ModelConfigurations.UserManagement;

namespace WasteProducts.DataAccess.Contexts
{
    [DbConfigurationType(typeof(MsSqlConfiguration))]
    public class WasteContext : IdentityDbContext<UserDB, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        private ISearchRepository _searchRepository { get; }        

        public WasteContext(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
            Database.Log = (s) => Debug.WriteLine(s);
        }

        public WasteContext(string nameOrConnectionString, ISearchRepository searchRepository) : base(nameOrConnectionString)
        {
            _searchRepository = searchRepository;
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
                
        public override int SaveChanges()
        {            
            SaveChangesToSearchRepository();
            return base.SaveChanges();
        }

        /// <summary>
        /// Save changes to Lucene search repository. Runs 3 method with different params (Entity.State)
        /// </summary>
        private void SaveChangesToSearchRepository()
        {
            DetectAndSaveChanges(EntityState.Added, new List<Type> { typeof(ProductDB) });
            DetectAndSaveChanges(EntityState.Modified, new List<Type> { typeof(ProductDB) });
            DetectAndSaveChanges(EntityState.Deleted, new List<Type> { typeof(ProductDB) });            
        }

        /// <summary>
        /// Detectes changes and save it to Lucene using LuceneSearchRepository
        /// </summary>
        /// <param name="state">EntityState that needed to detect and save</param>
        /// <param name="types">Object type that needed to detect and save</param>
        protected void DetectAndSaveChanges(EntityState state, IEnumerable<Type> types)
        {            
            this.Configuration.AutoDetectChangesEnabled = false;

            var changedList = this.ChangeTracker.Entries()
                .Where(x => x.State == state)
                .Select(x => x.Entity).ToList();

            this.Configuration.AutoDetectChangesEnabled = true;

            foreach (var item in changedList)
            {
                if (types.Contains(item.GetType()))
                {                    
                    if (state == EntityState.Added)
                        _searchRepository.Insert(item);
                    else if (state == EntityState.Modified)
                        _searchRepository.Update(item);
                    else _searchRepository.Delete(item);
                }
            }
        }
    }
}
