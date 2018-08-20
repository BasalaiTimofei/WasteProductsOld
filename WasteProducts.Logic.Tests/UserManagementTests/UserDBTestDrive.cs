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
            // если в прошлый раз дебажил и прервал дебаг, не почистив таблицу, вызываем метод-чистильщик "вручную"
            TearDown();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new UserClaimProfile());
                cfg.AddProfile(new UserLoginProfile());
            });

            //Database.Delete("name=ConStrByServer");

            // декомментить, когда надо обновить структуру базы данных
            using (var db = new WasteContext(NAME_OR_CONNECTION_STRING))
            {
                //db.Database.Delete();
                db.Database.CreateIfNotExists();
            }

            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IMailService mailService = new MailService(null, null, null);
            IUserService userService = new UserService(mailService, userRepo);

            User user = userService.RegisterAsync("umanetto@mail.ru", "Sergei", "qwerty",  "qwerty").GetAwaiter().GetResult();
            user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            user.EmailConfirmed = true;
            userService.UpdateAsync(user).GetAwaiter().GetResult();

            userService.RegisterAsync("test50someemail@gmail.com", "Anton", "qwerty2", "qwerty2").GetAwaiter().GetResult();
            userService.RegisterAsync("test51someemail@gmail.com", "Alexander", "qwerty3", "qwerty3").GetAwaiter().GetResult();

            IUserRoleRepository roleRepo = new UserRoleRepository(NAME_OR_CONNECTION_STRING);

            roleRepo.AddAsync(new IdentityRole("Simple user")).GetAwaiter().GetResult();
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
                    entry.State = EntityState.Deleted;
                }

                foreach (var role in wc.Roles)
                {
                    var entry = wc.Entry(role);
                    entry.State = EntityState.Deleted;
                }

                foreach (var product in wc.Products)
                {
                    var entry = wc.Entry(product);
                    entry.State = EntityState.Deleted;
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

        // TODO настроить Fluent API, иначе не реализую многие ко многим связь друзей
        // тестируем добавление друзей
        [Test]
        public void _10TestOfAddingNewFriendsToUser()
        {
            //using (var db = new WasteContext(NAME_OR_CONNECTION_STRING))
            //{
            //    var sergei = db.Users.FirstOrDefault(u => u.UserName == "Sergei");
            //    var anton = db.Users.FirstOrDefault(u => u.UserName == "Anton");

            //    db.Friends.Add(new Friend(sergei.Id, anton.Id));

            //    db.SaveChanges();
            //}

            //using (var db = new WasteContext(NAME_OR_CONNECTION_STRING))
            //{
            //    var sergei = db.Users.FirstOrDefault(u => u.UserName == "Sergei");
            //    var listOfFriendsId = from user in db.Users
            //                          join friend in db.Friends on user.Id equals friend.UserId
            //                          select friend.FriendOfUserId;


            //}


            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            User user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);

            User user2 = userService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = userService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            userService.AddFriendAsync(user, user2).GetAwaiter().GetResult();
            userService.AddFriendAsync(user, user3).GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);

            UserDB u = Mapper.Map<UserDB>(user);

            user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);
        }

        // тестируем удаление друзей
        [Test]
        public void _11TestOfDeletingFriendsFromUser()
        {
            IUserRepository userRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            IUserService userService = new UserService(null, userRepo);

            User user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);

            User user2 = userService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = userService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            userService.DeleteFriendAsync(user, user2).GetAwaiter().GetResult();
            userService.DeleteFriendAsync(user, user3).GetAwaiter().GetResult();
            Assert.AreEqual(user.Friends.Count, 0);

            user = userService.LogInAsync("umanetto@mail.ru", "qwerty").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);
        }

        // тестируем добавление продуктов
        [Test]
        public void _12TestOfAddingNewProductsToUser()
        {
            using (var db = new WasteContext(NAME_OR_CONNECTION_STRING))
            {
                var user = db.Users.FirstOrDefault();

                var prod = new ProductDB
                {
                    Created = DateTime.UtcNow,
                    Name = "WasteProduct",
                };

                user.Products.Add(prod);
                db.SaveChanges();
                Assert.AreEqual(user.Products.FirstOrDefault().Name, "WasteProduct");
            }

            using (var db = new WasteContext(NAME_OR_CONNECTION_STRING))
            {
                var user = db.Users.FirstOrDefault();
                Assert.AreEqual(user.Products.FirstOrDefault().Name, "WasteProduct");
            }
        }

        // тестируем удаление продуктов
        [Test]
        public void _13TestOfDeletingProductsFromUser()
        {

        }
    }
}