using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Contexts.Config;
using WasteProducts.Logic.Common.Models.Product;

namespace WasteProducts.DataAccess.Contexts
{
    [DbConfigurationType(typeof(MsSqlConfiguration))]
    public class WasteContext : IdentityDbContext
    {
        /// <summary>
        /// Comment: Added for to use an entity set that is used to perform
        ///  create, read, update, and delete operations in 'ProductRepository' class.
        /// </summary>
        public DbSet<Product> Products { get; set; }
    }
}