using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.Repositories.UserManagement;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.MailService;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Mappings.UserMappings;
using WasteProducts.Logic.Services.MailService;
using WasteProducts.Logic.Services.UserService;

namespace WasteProducts.Logic.Tests.UserManagementTests
{
    public class UserDBTestDrive
    {
        // Просто загружаем то, что нам надо будет для работы
        [OneTimeSetUp]
        public void Setup()
        {
            // если в прошлый раз дебажил и не почистил таблицу
            TearDown();

            IUserRepository userRepo = new UserRepository();

            if (userRepo.Select("umanetto@mail.ru") == null)
            {
                UserDB userDB = new UserDB()
                {
                    UserName = "Sergei",
                    Email = "umanetto@mail.ru",
                    EmailConfirmed = true,
                    PasswordHash = "qwerty",
                    Created = DateTime.UtcNow,
                };

                userRepo.AddAsync(userDB);
            }

            IUserRoleRepository roleRepo = new UserRoleRepository();

            if (roleRepo.FindByNameAsync("Simple user").GetAwaiter().GetResult() == null)
            {
                roleRepo.AddAsync(new IdentityRole("Simple user")).GetAwaiter().GetResult();
            }
        }

        // чистим таблицу после работы, чтобы таблица была всегда одинаковая
        [OneTimeTearDown]
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

        // проверяем, правильно ли выполнен Setup + проверяем запрос юзера по емейлу
        // и запрос роли по названию
        [Test]
        public void _01TestOfQueryingByEmail()
        {
            IUserRepository userRepo = new UserRepository();
            IUserRoleRepository roleRepo = new UserRoleRepository();

            UserDB userDB = userRepo.Select("umanetto@mail.ru");
            IdentityRole role = roleRepo.FindByNameAsync("Simple user").GetAwaiter().GetResult();

            Assert.IsNotNull(userDB);
            Assert.IsNotNull(role);
        }

        // тестим, правильно ли работает функционал добавления роли и добавления юзера в роль
        [Test]
        public void _02TestOfAddingToTheUserDBNewRole()
        {
            IUserRepository userRepo = new UserRepository();
            IUserRoleRepository roleRepo = new UserRoleRepository();
            IUserService userService = new UserService(null, userRepo);

            userService.LogIn("umanetto@mail.ru", "qwerty", out User user);

            userService.AddToRoleAsync(user, "Simple user").GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());

            userService.LogIn("umanetto@mail.ru", "qwerty", out user);

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());
        }

        // тестируем, как работает метот GetRolesAsynс IUserService
        [Test]
        public void _03TestOfMappingFromUserLoginTo()
        {
            IUserRepository userRepo = new UserRepository();
            IUserRoleRepository roleRepo = new UserRoleRepository();
            IUserService userService = new UserService(null, userRepo);

            User user = Mapper.Map<User>(userRepo.Select(u => u.Email == "umanetto@mail.ru", false));
            user.Roles = userService.GetRolesAsync(user).GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());
        }

        // тестируем добавление логина юзеру
        [Test]
        public void _04TestOfAddingLoginToTheUser()
        {

        }
    }
}
