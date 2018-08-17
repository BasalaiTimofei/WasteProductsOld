using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Contexts.Config;

namespace WasteProducts.DataAccess.Contexts
{
    [DbConfigurationType(typeof(MsSqlConfiguration))]
    public class WasteContext : IdentityDbContext
    {
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
    }
}