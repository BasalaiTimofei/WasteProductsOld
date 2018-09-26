// <copyright file="DefaultSetup.cs">
//    2017 - Johan Boström
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityServer3.Core.Models;
using IdentityServer3.EntityFramework;
using WasteProducts.IdentityServer.Db;
using WasteProducts.IdentityServer.Managers;
using WasteProducts.IdentityServer.Models;
using WasteProducts.IdentityServer.Stores;
using Microsoft.AspNet.Identity;

namespace WasteProducts.IdentityServer.Config
{
    public class DefaultSetup
    {

        public static void Configure(EntityFrameworkServiceOptions options)
        {
           
        }
    }
}