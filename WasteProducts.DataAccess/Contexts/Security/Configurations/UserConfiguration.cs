using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Contexts.Security.Configurations
{
    class UserConfiguration : EntityTypeConfiguration<IUserDb>
    {
        public UserConfiguration()
        {
            ToTable("Users");

            HasKey(c => c.Id)
              .Property(x => x.Id)
              .HasColumnName("UserId")
              .HasColumnType("int")
              .IsRequired();

            Property(c => c.Email)
                .HasMaxLength(256);

            Property(c => c.UserName)
                .HasColumnName("UserName")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));


            Property(c => c.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(c => c.SecurityStamp)
                .HasColumnName("SecurityStamp")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();


            HasMany(c => c.Roles)
                .WithMany(c => c.Users)
                .Map(c =>
                {
                    c.ToTable("UsersRoles");
                    c.MapLeftKey("UserId");
                    c.MapRightKey("RoleId");
                });

            HasMany(c => c.Claims)
                .WithRequired(c => c.User)
                .HasForeignKey(c => c.UserId);

            HasMany(c => c.Logins)
                .WithRequired(c => c.User)
                .HasForeignKey(c => c.UserId);


        }
    }
}
