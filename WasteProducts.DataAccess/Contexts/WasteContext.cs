using System.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Products;
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

        // "name=ConStrByServer" - такой по этому имени в конфиге тестового проекта лежит ссыль на connectionString
        // к базе данных, которой я (Тишков Сергей, User Management) пользовался для функционального тестирования UserService
        public WasteContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.Log = (s) => Debug.WriteLine(s);
        }

        /// <summary>
        /// property added for to use an entity set that is used to perform
        ///  create, read, update, delete and to get product list operations in 'ProductRepository' class.
        /// </summary>
        public IDbSet<ProductDB> Products { get; set; }
    }
}