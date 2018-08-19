using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using System;
using System.Linq;
using System.Security.Claims;
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





// FUNCTIONAL TESTS OF UserService AND UserRepository WITH REAL CONNECTION TO DATABASE 
// PLEASE DON'T DELETE THIS FILE 

// ФУНКЦИОНАЛЬНЫЕ ТЕСТЫ UserService И UserRepository С РЕАЛЬНЫМ ПОДКЛЮЧЕНИЕМ К БАЗЕ ДАНЫХ
// ПОЖАЛУЙСТА, НЕ УДАЛЯЙТЕ ЭТОТ ФАЙЛ





namespace WasteProducts.Logic.Tests.UserManagementTests
{
    public class UserDBTestDrive
    {
        public const string NAME_OR_CONNECTION_STRING = "name=ConStrByServer";
        // Просто загружаем то, что нам надо будет для работы
        [OneTimeSetUp]
        public void Setup()
        {
            // если в прошлый раз дебажил и не почистил таблицу
            TearDown();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new UserClaimProfile());
                cfg.AddProfile(new UserLoginProfile());
            });

            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IMailService mailService = new MailService(null, null, null);
            IUserService userService = new UserService(mailService, userRepo);

            if (userRepo.Select("umanetto@mail.ru") == null)
            {
                User user = userService.RegisterAsync("umanetto@mail.ru", "qwerty", "Sergei", "qwerty").GetAwaiter().GetResult();
                user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
                user.EmailConfirmed = true;
                userService.UpdateAsync(user).GetAwaiter().GetResult();
            }

            IUserRoleRepository roleRepo = new UserRoleRepository(NAME_OR_CONNECTION_STRING);

            if (roleRepo.FindByNameAsync("Simple user").GetAwaiter().GetResult() == null)
            {
                roleRepo.AddAsync(new IdentityRole("Simple user")).GetAwaiter().GetResult();
            }
        }

        // чистим таблицу после работы, чтобы таблица была всегда одинаковая
        [OneTimeTearDown]
        public void TearDown()
        {
            using (WasteContext wc = new WasteContext(NAME_OR_CONNECTION_STRING))
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
            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            IUserRoleRepository roleRepo = new UserRoleRepository(NAME_OR_CONNECTION_STRING);

            User user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            IdentityRole role = roleRepo.FindByNameAsync("Simple user").GetAwaiter().GetResult();

            Assert.AreEqual(user.Email, "umanetto@mail.ru");
            Assert.AreEqual(role.Name, "Simple user");
        }

        // тестим, правильно ли работает функционал добавления роли и добавления юзера в роль
        [Test]
        public void _02TestOfAddingToTheUserDBNewRole()
        {
            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            User user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();

            userService.AddToRoleAsync(user, "Simple user").GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());

            user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());
        }

        // тестируем, как работает метот GetRolesAsynс IUserService
        [Test]
        public void _03TestOfGettingRolesOfTheUser()
        {
            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            User user = Mapper.Map<User>(userRepo.Select(u => u.Email == "umanetto@mail.ru", false));
            user.Roles = userService.GetRolesAsync(user).GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());
        }

        // тестируем изъятие из роли
        [Test]
        public void _04TestOfRemovingUserFromRole()
        {
            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            User user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(user.Roles.FirstOrDefault(), "Simple user");

            userService.RemoveFromRoleAsync(user, "Simple user").GetAwaiter().GetResult();
            Assert.IsNull(user.Roles.FirstOrDefault());

            user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user.Roles.FirstOrDefault());
        }

        // Тестируем добавление утверждения (Claim) в юзера
        [Test]
        public void _05TestOfAddingClaimToUser()
        {
            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            User user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            var claim = new Claim("SomeType", "SomeValue");

            userService.AddClaimAsync(user, claim).GetAwaiter().GetResult();
            var userClaim = user.Claims.FirstOrDefault();
            Assert.AreEqual(userClaim.Type, claim.Type);
            Assert.AreEqual(userClaim.Value, claim.Value);

            user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            userClaim = user.Claims.FirstOrDefault();
            Assert.AreEqual(userClaim.Type, claim.Type);
            Assert.AreEqual(userClaim.Value, claim.Value);
        }

        // тестируем удаление утверждения из юзера
        [Test]
        public void _06TestOfDeletingClaimFromUser()
        {
            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            User user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 1);

            userService.RemoveClaimAsync(user, user.Claims.FirstOrDefault()).GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 0);

            user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 0);
        }

        // тестируем добавление логина в юзера
        [Test]
        public void _07TestOfAddingLoginToUser()
        {
            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            User user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            userService.AddLoginAsync(user, login).GetAwaiter().GetResult();
            var userLogin = user.Logins.FirstOrDefault();
            Assert.AreEqual(login, userLogin);

            user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            userLogin = user.Logins.FirstOrDefault();
            Assert.AreEqual(login, userLogin);
        }

        // тестируем удаление логина из юзера
        [Test]
        public void _08TestOfDeletingLoginFromUser()
        {
            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            User user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            Assert.AreEqual(user.Logins.Count, 1);
            userService.RemoveLoginAsync(user, login).GetAwaiter().GetResult();
            Assert.AreEqual(user.Logins.Count, 0);

            user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(user.Logins.Count, 0);
        }

        // тестируем апдейт юзера

        [Test]
        public void _09TestOfUserUpdating()
        {
            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            User user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();

            string userPhoneNumber = user.PhoneNumber;
            Assert.AreEqual(userPhoneNumber, null);

            user.PhoneNumber = "+375172020327";
            userService.UpdateAsync(user).GetAwaiter().GetResult();

            user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(user.PhoneNumber, "+375172020327");
        }
    }
}