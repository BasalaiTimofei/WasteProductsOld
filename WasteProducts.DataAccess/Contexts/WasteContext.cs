using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Contexts.Config;

namespace WasteProducts.DataAccess.Contexts
{
    [DbConfigurationType(typeof(MsSqlConfiguration))]
    public class WasteContext : IdentityDbContext
    {

    }
}