using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.Repositories.UserManagement;
using WasteProducts.Logic.Common.Models.Products;
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

        public static IUserRepository UserRepo;

        public static IMailService MailService;

        public static IUserService UserService;

        public static IUserRoleRepository RoleRepo;

        static UserDBTestDrive()
        {
            UserRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            RoleRepo = new UserRoleRepository(NAME_OR_CONNECTION_STRING);
            MailService = new MailService(null, null, null);
            UserService = new UserService(MailService, UserRepo);
        }

        // Просто загружаем то, что нам надо будет для работы
        [OneTimeSetUp]
        public void Setup()
        {
            // чистим таблицу перед запуском, чтобы она всегда в тестах была одинаковая
            // чистим её не после тестов для того, чтобы можно было залезть в неё после
            // тестов и посмотреть на фактические результаты
            using (WasteContext wc = new WasteContext(NAME_OR_CONNECTION_STRING))
            {
                wc.Database.Delete();
                wc.Database.CreateIfNotExists();
            }

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new UserClaimProfile());
                cfg.AddProfile(new UserLoginProfile());
            });

            User user = UserService.RegisterAsync("umanetto@mail.ru", "Sergei", "qwerty", "qwerty").GetAwaiter().GetResult();
            user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            user.EmailConfirmed = true;
            UserService.UpdateAsync(user).GetAwaiter().GetResult();

            UserService.RegisterAsync("test50someemail@gmail.com", "Anton", "qwerty2", "qwerty2").GetAwaiter().GetResult();
            UserService.RegisterAsync("test51someemail@gmail.com", "Alexander", "qwerty3", "qwerty3").GetAwaiter().GetResult();

            RoleRepo.AddAsync(new IdentityRole("Simple user")).GetAwaiter().GetResult();
        }

        // проверяем, правильно ли выполнен Setup + проверяем запрос юзера по емейлу
        // и запрос роли по названию
        [Test]
        public void _01TestOfQueryingByEmail()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            IdentityRole role = RoleRepo.FindByNameAsync("Simple user").GetAwaiter().GetResult();

            Assert.AreEqual(user.Email, "umanetto@mail.ru");
            Assert.AreEqual(role.Name, "Simple user");
        }

        // тестим, правильно ли работает функционал добавления роли и добавления юзера в роль
        [Test]
        public void _02TestOfAddingToTheUserDBNewRole()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();

            UserService.AddToRoleAsync(user, "Simple user").GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());

            user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());
        }

        // тестируем, как работает метот GetRolesAsynс IUserService
        [Test]
        public void _03TestOfGettingRolesOfTheUser()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            user.Roles = UserService.GetRolesAsync(user).GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());
        }

        // тестируем изъятие из роли
        [Test]
        public void _04TestOfRemovingUserFromRole()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(user.Roles.FirstOrDefault(), "Simple user");

            UserService.RemoveFromRoleAsync(user, "Simple user").GetAwaiter().GetResult();
            Assert.IsNull(user.Roles.FirstOrDefault());

            user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user.Roles.FirstOrDefault());
        }

        // Тестируем добавление утверждения (Claim) в юзера
        [Test]
        public void _05TestOfAddingClaimToUser()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            var claim = new Claim("SomeType", "SomeValue");

            UserService.AddClaimAsync(user, claim).GetAwaiter().GetResult();
            var userClaim = user.Claims.FirstOrDefault();
            Assert.AreEqual(userClaim.Type, claim.Type);
            Assert.AreEqual(userClaim.Value, claim.Value);

            user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            userClaim = user.Claims.FirstOrDefault();
            Assert.AreEqual(userClaim.Type, claim.Type);
            Assert.AreEqual(userClaim.Value, claim.Value);
        }

        // тестируем удаление утверждения из юзера
        [Test]
        public void _06TestOfDeletingClaimFromUser()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 1);

            UserService.RemoveClaimAsync(user, user.Claims.FirstOrDefault()).GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 0);

            user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 0);
        }

        // тестируем добавление логина в юзера
        [Test]
        public void _07TestOfAddingLoginToUser()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            UserService.AddLoginAsync(user, login).GetAwaiter().GetResult();
            var userLogin = user.Logins.FirstOrDefault();
            Assert.AreEqual(login, userLogin);

            user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            userLogin = user.Logins.FirstOrDefault();
            Assert.AreEqual(login, userLogin);
        }

        // тестируем удаление логина из юзера
        [Test]
        public void _08TestOfDeletingLoginFromUser()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            Assert.AreEqual(user.Logins.Count, 1);
            UserService.RemoveLoginAsync(user, login).GetAwaiter().GetResult();
            Assert.AreEqual(user.Logins.Count, 0);

            user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(user.Logins.Count, 0);
        }

        // тестируем апдейт юзера
        [Test]
        public void _09TestOfUserUpdating()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();

            string userPhoneNumber = user.PhoneNumber;
            Assert.AreEqual(userPhoneNumber, null);

            user.PhoneNumber = "+375172020327";
            UserService.UpdateAsync(user).GetAwaiter().GetResult();

            user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(user.PhoneNumber, "+375172020327");
        }

        // TODO настроить Fluent API, иначе не реализую многие ко многим связь друзей
        // тестируем добавление друзей
        [Test]
        public void _10TestOfAddingNewFriendsToUser()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);

            User user2 = UserService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = UserService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            UserService.AddFriendAsync(user, user2).GetAwaiter().GetResult();
            UserService.AddFriendAsync(user, user3).GetAwaiter().GetResult();

            Assert.AreEqual(2, user.Friends.Count);

            UserDB u = Mapper.Map<UserDB>(user);

            user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);
        }

        // тестируем удаление друзей
        [Test]
        public void _11TestOfDeletingFriendsFromUser()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);

            User user2 = UserService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = UserService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            UserService.DeleteFriendAsync(user, user2).GetAwaiter().GetResult();
            UserService.DeleteFriendAsync(user, user3).GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);

            user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);
        }

        // тестируем добавление продуктов
        [Test]
        public void _12TestOfAddingNewProductsToUser()
        {
            User user = UserService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();

            Product product = new Product
            {

            };

            UserService.AddProductAsync(user, product);

            //using (var db = new WasteContext(NAME_OR_CONNECTION_STRING))
            //{
            //    var user = db.Users.FirstOrDefault();

            //    var prod = new ProductDB
            //    {
            //        Created = DateTime.UtcNow,
            //        Name = "WasteProduct",
            //    };

            //    user.Products.Add(prod);
            //    db.SaveChanges();
            //    Assert.AreEqual(user.Products.FirstOrDefault().Name, "WasteProduct");
            //}

            //using (var db = new WasteContext(NAME_OR_CONNECTION_STRING))
            //{
            //    var user = db.Users.FirstOrDefault();
            //    Assert.AreEqual(user.Products.FirstOrDefault().Name, "WasteProduct");
            //}
        }

        // тестируем удаление продуктов
        [Test]
        public void _13TestOfDeletingProductsFromUser()
        {

        }
    }
}