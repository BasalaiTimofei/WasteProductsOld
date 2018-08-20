using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;
using WasteProducts.DataAccess.Contexts.Security.Configurations;

namespace WasteProducts.DataAccess.Contexts.Security
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        
        public virtual DbSet<IUserDb> Users { get; set; }
        public virtual DbSet<IUserLoginDb> UserLogins { get; set; }
        public virtual DbSet<IClaimDb> UserClaims { get; set; }
        public virtual DbSet<IUserRoleDb> UserRoles { get; set; }
        public virtual DbSet<IRoleDb> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());
            modelBuilder.Configurations.Add(new UserClaimConfiguration());
            modelBuilder.Configurations.Add(new UserLoginConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
