using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models;
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
        public IDbSet<GroupBoardDB> GroupBordDBs { get; set; }
        public IDbSet<GroupDB> GroupDBs { get; set; }
        public IDbSet<GroupUserDB> GroupUserDBs { get; set; }
        public IDbSet<GroupUserInviteTimeDB> GroupUserInviteTimeDBs { get; set; }
        public IDbSet<GroupProductDB> GroupProductDBs { get; set; }
    }
}
