using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Product;
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
        public DbSet<ProductDB> Products { get; set; }
    }
}