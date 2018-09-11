using Ninject;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Repositories.UserManagement;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services;
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
            _userService.RegisterAsync("test49someemail@gmail.com", "Sergei", "qwerty1").GetAwaiter().GetResult();
            _userService.RegisterAsync("test50someemail@gmail.com", "Anton", "qwerty2").GetAwaiter().GetResult();
            _userService.RegisterAsync("test51someemail@gmail.com", "Alexander", "qwerty3").GetAwaiter().GetResult();

            var user1 = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            var user2 = _userService.LogInByEmailAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            var user3 = _userService.LogInByEmailAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

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
            _userService.RegisterAsync("Incorrect email", "NewLogin", "qwerty").GetAwaiter().GetResult();
            User user = _userService.LogInByEmailAsync("Incorrect email", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);

            user = _userService.LogInByEmailAsync("Incorrect email", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);
        }

        // пытаемся зарегистрировать юзера с уже использованным емейлом
        [Test]
        public void UserIntegrTest_02AddingUserWithAlreadyRegisteredEmail()
        {
            _userService.RegisterAsync("test49someemail@gmail.com", "NewLogin", "qwerty").GetAwaiter().GetResult();
            User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);

            user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);
        }

        // пытаемся зарегистрировать юзера с неуникальным юзернеймом
        [Test]
        public void UserIntegrTest_03AddingUserWithAlreadyRegisteredNickName()
        {
            _userService.RegisterAsync("test100someemail@gmail.com", "Sergei", "qwerty").GetAwaiter().GetResult();
            User user = _userService.LogInByEmailAsync("test100someemail@gmail.com", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);

            user = _userService.LogInByEmailAsync("test100someemail@gmail.com", "qwerty").GetAwaiter().GetResult();
            Assert.IsNull(user);
        }

        // пытаемся зарегистрировать юзера с null-овыми аргументами, не должно крашить, не должно регистрировать
        [Test]
        public void UserIntegrTest_04RegisteringUserWithNullArguements()
        {
            _userService.RegisterAsync(null, "Sergei1", "qwert1").GetAwaiter().GetResult();
            _userService.RegisterAsync("test101someemail@gmail.com", null, "qwert2").GetAwaiter().GetResult();

            User user1 = _userService.LogInByNameAsync("Sergei1", "qwert1").GetAwaiter().GetResult();
            User user2 = _userService.LogInByEmailAsync("test101someemail@gmail.com", "qwert2").GetAwaiter().GetResult();

            Assert.IsNull(user1);
            Assert.IsNull(user2);
        }

        // проверяем запрос юзера по правильным емейлу и паролю (должно вернуть соответствующего юзера)
        [Test]
        public void UserIntegrTest_05CorrectLoggingInByEmail()
        {
            User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Email, "test49someemail@gmail.com");
        }

        // проверяем запрос юзера по неверным емейлу и паролю (юзер должен быть null-овым)
        [Test]
        public void UserIntegrTest_06IncorrectQueryingByEmail()
        {
            User user = _userService.LogInByEmailAsync("incorrectEmail", "incorrectPassword").GetAwaiter().GetResult();
            Assert.IsNull(user);
        }

        // пытаемся поменять зарегистрированному юзеру емейл на корректный уникальный емейл (должно поменять)
        [Test]
        public void UserIntegrTest_07ChangingUserEmailToAvailableEmail()
        {
            User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("test49someemail@gmail.com", user.Email);

            bool result = _userService.UpdateEmailAsync(user.Id, "uniqueemail@gmail.com").GetAwaiter().GetResult();

            Assert.IsTrue(result);

            _userService.UpdateEmailAsync(user.Id, "test49someemail@gmail.com").GetAwaiter().GetResult();
        }

        // пытаемся поменять зарегистрированному юзеру емейл на некорректный уникальный емейл (не должно поменять)
        [Test]
        public void UserIntegrTest_08ChangingUserEmailToIncorrectEmail()
        {
            User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("test49someemail@gmail.com", user.Email);

            bool result = _userService.UpdateEmailAsync(user.Id, "uniqueButIncorrectEmail").GetAwaiter().GetResult();

            Assert.IsFalse(result);
            Assert.AreEqual("test49someemail@gmail.com", user.Email);
        }

        // пытаемся поменять зарегистрированному юзеру емейл на корректный неуникальный емейл (не должно поменять)
        [Test]
        public void UserIntegrTest_09ChangingUserEmailToAlreadyRegisteredEmail()
        {
            User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("test49someemail@gmail.com", user.Email);

            bool result = _userService.UpdateEmailAsync(user.Id, "test50someemail@gmail.com").GetAwaiter().GetResult();

            Assert.IsFalse(result);
            Assert.AreEqual("test49someemail@gmail.com", user.Email);

            user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("test49someemail@gmail.com", user.Email);
        }

        // пытаемся передать в метод UpdateEmailAsync null-овые аргументы (не должно поменять емейла, не должно выдать ошибку)
        [Test]
        public void UserIntegrTest_10CallUpdateEmailAsyncWithNulArguements()
        {
            User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
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
            User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual("Sergei", user.UserName);

            bool result = _userService.UpdateUserNameAsync(user.Id, "Anton").GetAwaiter().GetResult();

            user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();

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

        // тестим, правильно ли работает функционал добавления роли и добавления юзера в роль, a так же метод GetRolesAsync IUserService
        [Test]
        public void UserIntegrTest_14AddingToTheUserDBNewRole()
        {
            User user1 = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            User user2 = _userService.LogInByEmailAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = _userService.LogInByEmailAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            _userService.AddToRoleAsync(user1.Id, "Simple user").GetAwaiter().GetResult();
            _userService.AddToRoleAsync(user2.Id, "Simple user").GetAwaiter().GetResult();
            _userService.AddToRoleAsync(user3.Id, "Simple user").GetAwaiter().GetResult();

            user1 = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            var rolesOfUser1 = _userService.GetRolesAsync(user1.Id).GetAwaiter().GetResult();
            Assert.AreEqual("Simple user", rolesOfUser1.FirstOrDefault());
        }

        // тестируем изъятие из роли
        [Test]
        public void UserIntegrTest_15RemovingUserFromRole()
        {
            var userId = _usersIds[0];
            var userRoles = _userService.GetRolesAsync(userId).GetAwaiter().GetResult();
            Assert.AreEqual(userRoles.FirstOrDefault(), "Simple user");
            _userService.RemoveFromRoleAsync(userId, "Simple user").GetAwaiter().GetResult();

            userRoles = _userService.GetRolesAsync(userId).GetAwaiter().GetResult();
            Assert.IsNull(userRoles.FirstOrDefault());
        }

        // Тестируем добавление утверждения (Claim) в юзера
        [Test]
        public void UserIntegrTest_16AddingClaimToUser()
        {
            var userId = _usersIds[0];
            var claim = new Claim("SomeType", "SomeValue");

            _userService.AddClaimAsync(userId, claim).GetAwaiter().GetResult();

            var userClaims = _userService.GetClaimsAsync(userId).GetAwaiter().GetResult();
            var userClaim = userClaims.FirstOrDefault();

            Assert.AreEqual(userClaim.Type, claim.Type);
            Assert.AreEqual(userClaim.Value, claim.Value);
        }

        // тестируем удаление утверждения из юзера
        [Test]
        public void UserIntegrTest_17DeletingClaimFromUser()
        {
            var userId = _usersIds[0];
            var userClaims = _userService.GetClaimsAsync(userId).GetAwaiter().GetResult();
            Assert.AreEqual(1, userClaims.Count);

            _userService.RemoveClaimAsync(userId, userClaims.FirstOrDefault()).GetAwaiter().GetResult();

            userClaims = _userService.GetClaimsAsync(userId).GetAwaiter().GetResult();
            Assert.AreEqual(0, userClaims.Count);
        }

        // тестируем добавление логина в юзера
        [Test]
        public void UserIntegrTest_18AddingLoginToUser()
        {
            var userId = _usersIds[0];
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            _userService.AddLoginAsync(userId, login).GetAwaiter().GetResult();

            var userLogins = _userService.GetLoginsAsync(userId).GetAwaiter().GetResult();
            var userLogin = userLogins.FirstOrDefault();

            Assert.AreEqual(login, userLogin);
        }

        // тестируем удаление логина из юзера
        [Test]
        public void UserIntegrTest_19DeletingLoginFromUser()
        {
            var userId = _usersIds[0];
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            var userLogins = _userService.GetLoginsAsync(userId).GetAwaiter().GetResult();

            Assert.AreEqual(1, userLogins.Count);
            _userService.RemoveLoginAsync(userId, login).GetAwaiter().GetResult();

            userLogins = _userService.GetLoginsAsync(userId).GetAwaiter().GetResult();
            Assert.AreEqual(0, userLogins.Count);
        }

        // тестируем апдейт юзера
        //[Test]
        public void UserIntegrTest_20UserUpdating()
        {
            //User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();

            //string userPhoneNumber = user.PhoneNumber;
            //Assert.AreEqual(userPhoneNumber, null);

            //user.PhoneNumber = "+375172020327";
            //_userService.UpdateAsync(user).GetAwaiter().GetResult();

            //user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            //Assert.AreEqual(user.PhoneNumber, "+375172020327");
        }

        // тестируем изменение пароля пользователя
        [Test]
        public void UserIntegrTest_21ResettingUserPassword()
        {
            User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            _userService.ChangePasswordAsync(user.Id, "qwerty1", "New password").GetAwaiter().GetResult();

            user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "New password").GetAwaiter().GetResult();
            _userService.ChangePasswordAsync(user.Id, "New password", "qwerty1").GetAwaiter().GetResult();
        }

        // тестируем добавление друзей
        [Test]
        public void UserIntegrTest_22AddingNewFriendsToUser()
        {
            User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);

            User user2 = _userService.LogInByEmailAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = _userService.LogInByEmailAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            _userService.AddFriendAsync(user.Id, user2.Id).GetAwaiter().GetResult();
            _userService.AddFriendAsync(user.Id, user3.Id).GetAwaiter().GetResult();

            user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);
        }

        // тестируем удаление друзей
        [Test]
        public void UserIntegrTest_23DeletingFriendsFromUser()
        {
            User user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);

            User user2 = _userService.LogInByEmailAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = _userService.LogInByEmailAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            _userService.DeleteFriendAsync(user.Id, user2.Id).GetAwaiter().GetResult();
            _userService.DeleteFriendAsync(user.Id, user3.Id).GetAwaiter().GetResult();

            user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);
        }

        // тестируем создание продукта (не относится к юзер сервису, но необходимо для следующего теста)
        [Test]
        public void UserIntegrTest_24AddingNewProductsToDB()
        {
            string productName = "Waste product";

            using (var prodService = _kernel.Get<IProductService>())
            {
                prodService.AddByName(productName);
                var product = prodService.GetByNameAsync(productName).GetAwaiter().GetResult();

                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.Name);
                _productIds.Add(product.Id);
            }
        }

        // тестируем добавление продукта
        [Test]
        public void UserIntegrTest_25AddingNewProductsToUser()
        {
            string description = "Tastes like garbage, won't buy it ever again.";

            var user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.ProductDescriptions.Count);

            _userService.AddProductAsync(user.Id, _productIds[0], 1, description).GetAwaiter().GetResult();
            user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();

            Assert.AreEqual(1, user.ProductDescriptions.Count);
            Assert.AreEqual(_productIds[0], user.ProductDescriptions[0].Product.Id);
            Assert.AreEqual(1, user.ProductDescriptions[0].Rating);
            Assert.AreEqual(description, user.ProductDescriptions[0].Description);
        }

        // тестируем удаление продуктов
        [Test]
        public void UserIntegrTest_26DeletingProductsFromUser()
        {
            var user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(1, user.ProductDescriptions.Count);

            _userService.DeleteProductAsync(user.Id, user.ProductDescriptions[0].Product.Id).GetAwaiter().GetResult();

            user = _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.ProductDescriptions.Count);
        }

        // тестируем поиск роли по айди и имени
        [Test]
        public void UserIntegrTest_27FindRoleByIdAndName()
        {
            UserRole foundByName = _roleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();
            Assert.AreEqual("Simple user", foundByName.Name);

            UserRole foundById = _roleService.FindByIdAsync(foundByName.Id).GetAwaiter().GetResult();
            Assert.AreEqual(foundByName.Name, foundById.Name);
            Assert.AreEqual(foundByName.Id, foundById.Id);
        }

        // тестируем получение всех пользователей определенной роли
        [Test]
        public void UserIntegrTest_28GettingRoleUsers()
        {
            User user1 = _userService.LogInByEmailAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user2 = _userService.LogInByEmailAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();
            UserRole role = _roleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();

            IEnumerable<User> users = _roleService.GetRoleUsers(role).GetAwaiter().GetResult();
            User user1FromGetRoles = users.FirstOrDefault(u => u.Id == user1.Id);
            User user2FromGetRoles = users.FirstOrDefault(u => u.Id == user2.Id);
            Assert.AreEqual(user1.Id, user1FromGetRoles.Id);
            Assert.AreEqual(user2.Id, user2FromGetRoles.Id);
        }

        // тестируем изменение названия роли
        [Test]
        public void UserIntegrTest_29UpdatingRoleName()
        {
            var userId = _usersIds[1];

            var rolesOfUser = _userService.GetRolesAsync(userId).GetAwaiter().GetResult();
            Assert.AreEqual("Simple user", rolesOfUser.FirstOrDefault());

            UserRole role = _roleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();
            _roleService.UpdateRoleNameAsync(role, "Not so simple user").GetAwaiter().GetResult();

            rolesOfUser = _userService.GetRolesAsync(userId).GetAwaiter().GetResult();
            Assert.AreEqual("Not so simple user", rolesOfUser.FirstOrDefault());
        }

        // тестируем удаление роли
        [Test]
        public void UserIntegrTest_30DeletingRole()
        {
            var userId = _usersIds[1];

            var rolesOfUser = _userService.GetRolesAsync(userId).GetAwaiter().GetResult();
            Assert.AreEqual("Not so simple user", rolesOfUser.FirstOrDefault());

            UserRole role = _roleService.FindByNameAsync("Not so simple user").GetAwaiter().GetResult();
            _roleService.DeleteAsync(role).GetAwaiter().GetResult();

            rolesOfUser = _userService.GetRolesAsync(userId).GetAwaiter().GetResult();
            Assert.IsNull(rolesOfUser.FirstOrDefault());
        }

        // тестируем удаление юзеров, а заодно и чистим базу до изначального состояния
        [Test]
        public void UserIntegrTest_31DeletingUsers()
        {
            foreach (var id in _usersIds)
            {
                _userService.DeleteUserAsync(id).GetAwaiter().GetResult();
            }
        }
    }
}