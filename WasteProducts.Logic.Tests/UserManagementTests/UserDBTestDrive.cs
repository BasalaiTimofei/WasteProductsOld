using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.Repositories.UserManagement;

namespace WasteProducts.Logic.Tests.UserManagementTests
{
    public class UserDBTestDrive
    {
        [SetUp]
        public void Setup()
        {
            IUserRepository repo = new UserRepository();

            if (repo.Select(u => u.Email == "umanetto@mail.ru") == null)
            {
                UserDB userDB = new UserDB()
                {
                    UserName = "Sergei",
                    Email = "umanetto@mail.ru",
                    EmailConfirmed = true,
                    PasswordHash = "qwerty",
                    Created = DateTime.UtcNow,
                };

                repo.AddAsync(userDB);
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (WasteContext wc = new WasteContext())
            {
                foreach (var user in wc.Users)
                {
                    var entry = wc.Entry(user);
                    entry.State = System.Data.Entity.EntityState.Deleted;
                }

                foreach (var role in wc.Roles)
                {
                    var entry = wc.Entry(role);
                    entry.State = System.Data.Entity.EntityState.Deleted;
                }

                wc.SaveChanges();
            }
        }

        [Test]
        public void TestMethod1()
        {
            IUserRepository repo = new UserRepository();
            Assert.IsNotNull(repo.Select(u => u.Email == "umanetto@mail.ru"));
        }
    }
}
