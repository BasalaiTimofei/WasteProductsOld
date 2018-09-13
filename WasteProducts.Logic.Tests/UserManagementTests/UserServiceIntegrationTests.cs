using Ninject;
using NUnit.Framework;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Repositories.UserManagement;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Services.Products;
using WasteProducts.Logic.Common.Services.UserService;

namespace WasteProducts.Logic.Tests.UserManagementTests
{
    [TestFixture]
    public class UserServiceIntegrationTests
    {
        private IUserService _userService;

        private IUserRoleService _roleService;

        private StandardKernel _kernel;

        private readonly List<string> _usersIds = new List<string>();

        private readonly List<string> _productIds = new List<string>();

        ~UserServiceIntegrationTests()
        {
            _userService?.Dispose();
            _roleService?.Dispose();
        }

        [OneTimeSetUp]
        public void Init()
        {
            _kernel = new StandardKernel();
            _kernel.Load(new DataAccess.InjectorModule(), new Logic.InjectorModule());

            using (var userRepo = _kernel.Get<IUserRepository>())
            {
                // не делал метода в интерфейсе ради безопасности,
                // надо знать, что лишь после приведения появляется такой метод.
                ((UserRepository)userRepo).RecreateTestDatabase();
            }
        }

        [OneTimeTearDown]
        public void LastTearDown()
        {
            _kernel?.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _userService = _kernel.Get<IUserService>();
            _roleService = _kernel.Get<IUserRoleService>();
        }

        [TearDown]
        public void TearDown()
        {
            _userService.Dispose();
            _roleService.Dispose();
        }

        // тестируем регистрирование юзеров и делаем начальное заполнение таблицы юзерами
        [Test]
        public void UserIntegrTest_00AddingUsers()
        {
            User user1 = _userService.RegisterAsync("test49someemail@gmail.com", "Sergei", "qwerty1", "qwerty1").GetAwaiter().GetResult();
            User user2 = _userService.RegisterAsync("test50someemail@gmail.com", "Anton", "qwerty2", "qwerty2").GetAwaiter().GetResult();
            User user3 = _userService.RegisterAsync("test51someemail@gmail.com", "Alexander", "qwerty3", "qwerty3").GetAwaiter().GetResult();

            Assert.AreEqual("test49someemail@gmail.com", user1.Email);
            Assert.AreEqual("Anton", user2.UserName);
            Assert.IsNotNull(user1.Id);

            _usersIds.Add(user1.Id);
            _usersIds.Add(user2.Id);
            _usersIds.Add(user3.Id);
        }

        // пытаемся зарегистрировать юзера с некорректным емейлом
        [Test]
        public void UserIntegrTest_01AddingUserWithIncorrectEmail()
        {
            User user = _userService.RegisterAsync("Incorrect email", "NewLogin", "qwerty", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);

            user = _userService.LogInAsync("Incorrect email", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);
        }

        // пытаемся зарегистрировать юзера с уже использованным емейлом
        [Test]
        public void UserIntegrTest_02AddingUserWithAlreadyRegisteredEmail()
        {
            User user = _userService.RegisterAsync("test49someemail@gmail.com", "NewLogin", "qwerty", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);
        }

        // пытаемся зарегистрировать юзера с неуникальным юзернеймом
        [Test]
        public void UserIntegrTest_03AddingUserWithAlreadyRegisteredNickName()
        {
            User user = _userService.RegisterAsync("test100someemail@gmail.com", "Sergei", "qwerty", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);

            user = _userService.LogInAsync("test100someemail@gmail.com", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);
        }

        // пытаемся зарегистрировать юзера с null-овыми аргументами, не должно крашить, должно возвращать null
        [Test]
        public void UserIntegrTest_04RegisteringUserWithNullArguements()
        {
            User user1 = _userService.RegisterAsync(null, "Sergei1", "qwert1", "qwert1").GetAwaiter().GetResult();
            User user2 = _userService.RegisterAsync("test101someemail@gmail.com", null, "qwert2", "qwert2").GetAwaiter().GetResult();
            User user3 = _userService.RegisterAsync("test102someemail@gmail.com", "Sergei3", null, "qwert3").GetAwaiter().GetResult();
            User user4 = _userService.RegisterAsync("test103someemail@gmail.com", "Sergei4", "qwert4", null).GetAwaiter().GetResult();

            Assert.IsNull(user1);
            Assert.IsNull(user2);
            Assert.IsNull(user3);
            Assert.IsNull(user4);
        }

        // проверяем запрос юзера по правильным емейлу и паролю (должно вернуть соответствующего юзера)
        [Test]
        public void UserIntegrTest_05CorrectLoggingInByEmail()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Email, "test49someemail@gmail.com");
        }

        // проверяем запрос юзера по неверным емейлу и паролю (юзер должен быть null-овым)
        [Test]
        public void UserIntegrTest_06IncorrectQueryingByEmail()
        {
            User user = _userService.LogInAsync("incorrectEmail", "incorrectPassword").GetAwaiter().GetResult();
            Assert.IsNull(user);
        }

        // пытаемся поменять зарегистрированному юзеру емейл на корректный уникальный емейл (должно поменять)
        [Test]
        public void UserIntegrTest_07ChangingUserEmailToAvailableEmail()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("test49someemail@gmail.com", user.Email);

            bool result = _userService.UpdateEmailAsync(user.Id, "uniqueemail@gmail.com").GetAwaiter().GetResult();

            Assert.IsTrue(result);

            _userService.UpdateEmailAsync(user.Id, "test49someemail@gmail.com").GetAwaiter().GetResult();
        }

        // пытаемся поменять зарегистрированному юзеру емейл на некорректный уникальный емейл (не должно поменять)
        [Test]
        public void UserIntegrTest_08ChangingUserEmailToIncorrectEmail()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("test49someemail@gmail.com", user.Email);

            bool result = _userService.UpdateEmailAsync(user.Id, "uniqueButIncorrectEmail").GetAwaiter().GetResult();

            Assert.IsFalse(result);
            Assert.AreEqual("test49someemail@gmail.com", user.Email);
        }

        // пытаемся поменять зарегистрированному юзеру емейл на корректный неуникальный емейл (не должно поменять)
        [Test]
        public void UserIntegrTest_09ChangingUserEmailToAlreadyRegisteredEmail()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("test49someemail@gmail.com", user.Email);

            bool result = _userService.UpdateEmailAsync(user.Id, "test50someemail@gmail.com").GetAwaiter().GetResult();

            Assert.IsFalse(result);
            Assert.AreEqual("test49someemail@gmail.com", user.Email);

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("test49someemail@gmail.com", user.Email);
        }

        // пытаемся передать в метод UpdateEmailAsync null-овые аргументы (не должно поменять емейла, не должно выдать ошибку)
        [Test]
        public void UserIntegrTest_10CallUpdateEmailAsyncWithNulArguements()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.IsNotNull(user);

            bool result1 = _userService.UpdateEmailAsync(user.Id, null).GetAwaiter().GetResult();
            bool result2 = _userService.UpdateEmailAsync(null, "correctuniqueemail@gmail.com").GetAwaiter().GetResult();

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.AreEqual("test49someemail@gmail.com", user.Email);
        }

        // пытаемся изменить юзеру юзернейм на юзернейм, уже имеющийся в системе (не должно поменять)
        [Test]
        public void UserIntegrTest_11ChangingUserNameToAlreadyExistingUserName()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("Sergei", user.UserName);

            bool result = _userService.UpdateUserNameAsync(user, "Anton").GetAwaiter().GetResult();

            Assert.IsFalse(result);
            Assert.AreEqual("Sergei", user.UserName);
        }

        // тестируем создание роли, а так же проверяем, действительно ли роль создается в базе данных
        [Test]
        public void UserIntegrTest_12FindingRoleByCorrectRoleName()
        {
            UserRole roleToCreate = new UserRole() { Name = "Simple user" };
            _roleService.CreateAsync(roleToCreate).GetAwaiter().GetResult();

            UserRole role = _roleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();
            Assert.AreEqual(role.Name, "Simple user");
        }

        // проверяем запрос роли по несуществующему названию
        [Test]
        public void UserIntegrTest_13FindingRoleByIncorrectRoleName()
        {
            UserRole role = _roleService.FindByNameAsync("Not existing role name").GetAwaiter().GetResult();
            Assert.IsNull(role);
        }

        // тестим, правильно ли работает функционал добавления роли и добавления юзера в роль
        [Test]
        public void UserIntegrTest_14AddingToTheUserDBNewRole()
        {
            User user1 = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            User user2 = _userService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = _userService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            _userService.AddToRoleAsync(user1.Id, "Simple user").GetAwaiter().GetResult();
            _userService.AddToRoleAsync(user2.Id, "Simple user").GetAwaiter().GetResult();
            _userService.AddToRoleAsync(user3.Id, "Simple user").GetAwaiter().GetResult();

            user1 = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("Simple user", user1.Roles.FirstOrDefault());
        }

        // тестируем, как работает метот GetRolesAsynс IUserService
        [Test]
        public void UserIntegrTest_15GettingRolesOfTheUser()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            user.Roles = _userService.GetRolesAsync(user).GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());
        }

        // тестируем изъятие из роли
        [Test]
        public void UserIntegrTest_16RemovingUserFromRole()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Roles.FirstOrDefault(), "Simple user");

            _userService.RemoveFromRoleAsync(user.Id, "Simple user").GetAwaiter().GetResult();

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.IsNull(user.Roles.FirstOrDefault());
        }

        // Тестируем добавление утверждения (Claim) в юзера
        [Test]
        public void UserIntegrTest_17AddingClaimToUser()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            var claim = new Claim("SomeType", "SomeValue");

            _userService.AddClaimAsync(user.Id, claim).GetAwaiter().GetResult();

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            var userClaim = user.Claims.FirstOrDefault();
            Assert.AreEqual(userClaim.Type, claim.Type);
            Assert.AreEqual(userClaim.Value, claim.Value);
        }

        // тестируем удаление утверждения из юзера
        [Test]
        public void UserIntegrTest_18DeletingClaimFromUser()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 1);

            _userService.RemoveClaimAsync(user.Id, user.Claims.FirstOrDefault()).GetAwaiter().GetResult();

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 0);
        }

        // тестируем добавление логина в юзера
        [Test]
        public void UserIntegrTest_19AddingLoginToUser()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            _userService.AddLoginAsync(user.Id, login).GetAwaiter().GetResult();

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            var userLogin = user.Logins.FirstOrDefault();
            Assert.AreEqual(login, userLogin);
        }

        // тестируем удаление логина из юзера
        [Test]
        public void UserIntegrTest_20DeletingLoginFromUser()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            Assert.AreEqual(user.Logins.Count, 1);
            _userService.RemoveLoginAsync(user.Id, login).GetAwaiter().GetResult();

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Logins.Count, 0);
        }

        // тестируем апдейт юзера
        [Test]
        public void UserIntegrTest_21UserUpdating()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();

            string userPhoneNumber = user.PhoneNumber;
            Assert.AreEqual(userPhoneNumber, null);

            user.PhoneNumber = "+375172020327";
            _userService.UpdateAsync(user).GetAwaiter().GetResult();

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.PhoneNumber, "+375172020327");
        }

        // тестируем изменение пароля пользователя
        [Test]
        public void UserIntegrTest_22ResettingUserPassword()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            user.PhoneNumber = "3334455";
            _userService.ResetPasswordAsync(user, "qwerty1", "New password", "New password").GetAwaiter().GetResult();

            user = _userService.LogInAsync("test49someemail@gmail.com", "New password").GetAwaiter().GetResult();
            Assert.AreNotEqual("3334455", user.PhoneNumber);
            _userService.ResetPasswordAsync(user, "New password", "qwerty1", "qwerty1").GetAwaiter().GetResult();
        }

        // тестируем добавление друзей
        [Test]
        public void UserIntegrTest_23AddingNewFriendsToUser()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);

            User user2 = _userService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = _userService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            _userService.AddFriendAsync(user, user2).GetAwaiter().GetResult();
            _userService.AddFriendAsync(user, user3).GetAwaiter().GetResult();

            Assert.AreEqual(2, user.Friends.Count);

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);
        }

        // тестируем удаление друзей
        [Test]
        public void UserIntegrTest_24DeletingFriendsFromUser()
        {
            User user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);

            User user2 = _userService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = _userService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            _userService.DeleteFriendAsync(user, user2).GetAwaiter().GetResult();
            _userService.DeleteFriendAsync(user, user3).GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);
        }

        // тестируем создание продукта (не относится к юзер сервису, но необходимо для следующего теста)
        [Test]
        public void UserIntegrTest_25AddingNewProductsToDB()
        {
            string productName = "Waste product";

            using (var prodService = _kernel.Get<IProductService>())
            {
                prodService.Add(productName, out var addedProduct);
                var product = prodService.GetByNameAsync(productName).GetAwaiter().GetResult();

                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.Name);
                _productIds.Add(product.Id);
            }
        }

        // тестируем добавление продукта
        [Test]
        public void UserIntegrTest_26AddingNewProductsToUser()
        {
            string description = "Tastes like garbage, won't buy it ever again.";

            var user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.ProductDescriptions.Count);

            _userService.AddProductAsync(user.Id, _productIds[0], 1, description).GetAwaiter().GetResult();
            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();

            Assert.AreEqual(1, user.ProductDescriptions.Count);
            Assert.AreEqual(_productIds[0], user.ProductDescriptions[0].Product.Id);
            Assert.AreEqual(1, user.ProductDescriptions[0].Rating);
            Assert.AreEqual(description, user.ProductDescriptions[0].Description);
        }

        // тестируем удаление продуктов
        [Test]
        public void UserIntegrTest_27DeletingProductsFromUser()
        {
            var user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(1, user.ProductDescriptions.Count);

            _userService.DeleteProductAsync(user.Id, user.ProductDescriptions[0].Product.Id).GetAwaiter().GetResult();

            user = _userService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.ProductDescriptions.Count);
        }

        // тестируем поиск роли по айди и имени
        [Test]
        public void UserIntegrTest_28FindRoleByIdAndName()
        {
            UserRole foundByName = _roleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();
            Assert.AreEqual("Simple user", foundByName.Name);

            UserRole foundById = _roleService.FindByIdAsync(foundByName.Id).GetAwaiter().GetResult();
            Assert.AreEqual(foundByName.Name, foundById.Name);
            Assert.AreEqual(foundByName.Id, foundById.Id);
        }

        // тестируем получение всех пользователей определенной роли
        [Test]
        public void UserIntegrTest_29GettingRoleUsers()
        {
            User user1 = _userService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user2 = _userService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();
            UserRole role = _roleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();

            IEnumerable<User> users = _roleService.GetRoleUsers(role).GetAwaiter().GetResult();
            User user1FromGetRoles = users.FirstOrDefault(u => u.Email == user1.Email);
            User user2FromGetRoles = users.FirstOrDefault(u => u.Email == user2.Email);
            Assert.AreEqual(user1.Id, user1FromGetRoles.Id);
            Assert.AreEqual(user2.Id, user2FromGetRoles.Id);
        }

        // тестируем изменение названия роли
        [Test]
        public void UserIntegrTest_30UpdatingRoleName()
        {
            User user1 = _userService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            Assert.AreEqual("Simple user", user1.Roles.FirstOrDefault());

            UserRole role = _roleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();

            _roleService.UpdateRoleNameAsync(role, "Not so simple user").GetAwaiter().GetResult();
            User user2 = _userService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();
            Assert.AreEqual("Not so simple user", user2.Roles.FirstOrDefault());
        }

        // тестируем удаление роли
        [Test]
        public void UserIntegrTest_31DeletingRole()
        {
            User user1 = _userService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            Assert.AreEqual("Not so simple user", user1.Roles.FirstOrDefault());

            UserRole role = _roleService.FindByNameAsync("Not so simple user").GetAwaiter().GetResult();
            _roleService.DeleteAsync(role).GetAwaiter().GetResult();

            User user2 = _userService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();
            Assert.IsNull(user2.Roles.FirstOrDefault());
        }

        // тестируем удаление юзеров, а заодно и чистим базу до изначального состояния
        [Test]
        public void UserIntegrTest_32DeletingUsers()
        {
            foreach (var id in _usersIds)
            {
                _userService.DeleteUserAsync(id).GetAwaiter().GetResult();
            }
        }
    }
}