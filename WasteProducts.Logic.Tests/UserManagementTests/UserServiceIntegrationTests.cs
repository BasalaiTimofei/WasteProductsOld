using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Users;
using WasteProducts.DataAccess.Repositories.Users;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.Products;
using WasteProducts.Logic.Common.Services.Users;

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
        public async Task UserIntegrTest_00AddingUsers()
        {
            await _userService.RegisterAsync("test49someemail@gmail.com", "Sergei", "qwerty1", null);
            await _userService.RegisterAsync("test50someemail@gmail.com", "Anton", "qwerty2", null);
            await _userService.RegisterAsync("test51someemail@gmail.com", "Alexander", "qwerty3", null);

            var user1 = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            var user2 = await _userService.LogInByEmailAsync("test50someemail@gmail.com", "qwerty2");
            var user3 = await _userService.LogInByEmailAsync("test51someemail@gmail.com", "qwerty3");

            Assert.AreEqual("Sergei", user1.UserName);
            Assert.AreEqual("Anton", user2.UserName);
            Assert.IsNotNull(user3.Id);

            _usersIds.Add(user1.Id);
            _usersIds.Add(user2.Id);
            _usersIds.Add(user3.Id);
        }

        

        // пытаемся зарегистрировать юзера с некорректным емейлом
        [Test]
        public async Task UserIntegrTest_01AddingUserWithIncorrectEmail()
        {
            await _userService.RegisterAsync("Incorrect email", "NewLogin", "qwerty", null);
            User user = await _userService.LogInByEmailAsync("Incorrect email", "qwerty");
            Assert.IsNull(user);

            user = await _userService.LogInByEmailAsync("Incorrect email", "qwerty");
            Assert.IsNull(user);
        }

        // пытаемся зарегистрировать юзера с уже использованным емейлом
        [Test]
        public async Task UserIntegrTest_02AddingUserWithAlreadyRegisteredEmail()
        {
            await _userService.RegisterAsync("test49someemail@gmail.com", "NewLogin", "qwerty", null);
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty");
            Assert.IsNull(user);

            user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty");
            Assert.IsNull(user);
        }

        // пытаемся зарегистрировать юзера с неуникальным юзернеймом
        [Test]
        public async Task UserIntegrTest_03AddingUserWithAlreadyRegisteredNickName()
        {
            await _userService.RegisterAsync("test100someemail@gmail.com", "Sergei", "qwerty", null);
            User user = await _userService.LogInByEmailAsync("test100someemail@gmail.com", "qwerty");
            Assert.IsNull(user);

            user = await _userService.LogInByEmailAsync("test100someemail@gmail.com", "qwerty");
            Assert.IsNull(user);
        }

        // пытаемся зарегистрировать юзера с null-овыми аргументами, не должно крашить, не должно регистрировать
        [Test]
        public async Task UserIntegrTest_04RegisteringUserWithNullArguements()
        {
            await _userService.RegisterAsync(null, "Sergei1", "qwert1", null);
            await _userService.RegisterAsync("test101someemail@gmail.com", null, "qwert2", null);

            User user1 = await _userService.LogInByNameAsync("Sergei1", "qwert1");
            User user2 = await _userService.LogInByEmailAsync("test101someemail@gmail.com", "qwert2");

            Assert.IsNull(user1);
            Assert.IsNull(user2);
        }

        // проверяем запрос юзера по правильным емейлу и паролю (должно вернуть соответствующего юзера)
        [Test]
        public async Task UserIntegrTest_05CorrectLoggingInByEmail()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            Assert.AreEqual("Sergei", user.UserName);
        }

        // проверяем запрос юзера по неверным емейлу и паролю (юзер должен быть null-овым)
        [Test]
        public async Task UserIntegrTest_06IncorrectQueryingByEmail()
        {
            User user = await _userService.LogInByEmailAsync("incorrectEmail", "incorrectPassword");
            Assert.IsNull(user);
        }

        // пытаемся поменять зарегистрированному юзеру емейл на корректный уникальный емейл (должно поменять)
        [Test]
        public async Task UserIntegrTest_07ChangingUserEmailToAvailableEmail()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            Assert.AreEqual("Sergei", user.UserName);

            bool result = await _userService.UpdateEmailAsync(user.Id, "uniqueemail@gmail.com");

            Assert.IsTrue(result);

            await _userService.UpdateEmailAsync(user.Id, "test49someemail@gmail.com");
        }

        // пытаемся поменять зарегистрированному юзеру емейл на некорректный уникальный емейл (не должно поменять)
        [Test]
        public async Task UserIntegrTest_08ChangingUserEmailToIncorrectEmail()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            Assert.AreEqual("Sergei", user.UserName);

            bool result = await _userService.UpdateEmailAsync(user.Id, "uniqueButIncorrectEmail");

            Assert.IsFalse(result);
            Assert.AreEqual("Sergei", user.UserName);
        }

        // пытаемся поменять зарегистрированному юзеру емейл на корректный неуникальный емейл (не должно поменять)
        [Test]
        public async Task UserIntegrTest_09ChangingUserEmailToAlreadyRegisteredEmail()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            Assert.AreEqual("Sergei", user.UserName);

            bool result = await _userService.UpdateEmailAsync(user.Id, "test50someemail@gmail.com");

            Assert.IsFalse(result);
            Assert.AreEqual("Sergei", user.UserName);

            user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            Assert.AreEqual("Sergei", user.UserName);
        }

        // пытаемся передать в метод UpdateEmailAsync null-овые аргументы (не должно поменять емейла, не должно выдать ошибку)
        [Test]
        public async Task UserIntegrTest_10CallUpdateEmailAsyncWithNulArguements()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            Assert.IsNotNull(user);

            bool result1 = await _userService.UpdateEmailAsync(user.Id, null);
            bool result2 = await _userService.UpdateEmailAsync(null, "correctuniqueemail@gmail.com");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.AreEqual("Sergei", user.UserName);
        }

        // пытаемся изменить юзеру юзернейм на юзернейм, уже имеющийся в системе (не должно поменять)
        [Test]
        public async Task UserIntegrTest_11ChangingUserNameToAlreadyExistingUserName()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            Assert.AreEqual("Sergei", user.UserName);

            bool result = await _userService.UpdateUserNameAsync(user.Id, "Anton");

            user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");

            Assert.IsFalse(result);
            Assert.AreEqual("Sergei", user.UserName);
        }

        // пытаемся зарегистрировать юзера так, как он будет регистрироваться на самом деле,
        // т.е. с "отправкой" письма на почту (по факту, если использовать настоящий ящик, оно отправляется),
        // где в тестовых целях из методов возвращаются айди и токен, необходимые для подтверждения емейла
        // так же тут тестируется аналогичная "отправка" запроса на изменение пароля (в if statement)
        [Test]
        public async Task UserIntegrTest_12TryingToRegisterUserPropperlyAndResetPassword()
        {
            string email = "test52someemail@gmail.com";
            var (id, token) = await _userService.RegisterAsync(email, "TestName", "TestPassword123", "Айди юзера: {0} и токен: {1}");
            if (await _userService.ConfirmEmailAsync(id, token))
            {
                (id, token) = await _userService.ResetPasswordRequestAsync(email, "Айди юзера: {0} и токен: {1}");
                await _userService.ResetPasswordAsync(id, token, "newPassword");
                var user = await _userService.LogInByNameAsync("TestName", "newPassword");
                Assert.IsNotNull(user);
                Assert.AreEqual(id, user.Id);
            }
            else
            {
                throw new Exception("Email wasn't confirmed!");
            }
        }

        // тестируем создание роли, а так же проверяем, действительно ли роль создается в базе данных
        [Test]
        public async Task UserIntegrTest_13FindingRoleByCorrectRoleName()
        {
            UserRole roleToCreate = new UserRole() { Name = "Simple user" };
            await _roleService.CreateAsync(roleToCreate);

            UserRole role = await _roleService.FindByNameAsync("Simple user");
            Assert.AreEqual(role.Name, "Simple user");
        }

        // проверяем запрос роли по несуществующему названию
        [Test]
        public async Task UserIntegrTest_14FindingRoleByIncorrectRoleName()
        {
            UserRole role = await _roleService.FindByNameAsync("Not existing role name");
            Assert.IsNull(role);
        }

        // тестим, правильно ли работает функционал добавления роли и добавления юзера в роль, a так же метод GetRolesAsync IUserService
        [Test]
        public async Task UserIntegrTest_15AddingToTheUserDBNewRole()
        {
            User user1 = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            User user2 = await _userService.LogInByEmailAsync("test50someemail@gmail.com", "qwerty2");
            User user3 = await _userService.LogInByEmailAsync("test51someemail@gmail.com", "qwerty3");

            await _userService.AddToRoleAsync(user1.Id, "Simple user");
            await _userService.AddToRoleAsync(user2.Id, "Simple user");
            await _userService.AddToRoleAsync(user3.Id, "Simple user");

            user1 = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            var rolesOfUser1 = await _userService.GetRolesAsync(user1.Id);
            Assert.AreEqual("Simple user", rolesOfUser1.FirstOrDefault());
        }

        // тестируем изъятие из роли
        [Test]
        public async Task UserIntegrTest_16RemovingUserFromRole()
        {
            var userId = _usersIds[0];
            var userRoles = await _userService.GetRolesAsync(userId);
            Assert.AreEqual(userRoles.FirstOrDefault(), "Simple user");
            await _userService.RemoveFromRoleAsync(userId, "Simple user");

            userRoles = await _userService.GetRolesAsync(userId);
            Assert.IsNull(userRoles.FirstOrDefault());
        }

        // Тестируем добавление утверждения (Claim) в юзера
        [Test]
        public async Task UserIntegrTest_17AddingClaimToUser()
        {
            var userId = _usersIds[0];
            var claim = new Claim("SomeType", "SomeValue");

            await _userService.AddClaimAsync(userId, claim);

            var userClaims = await _userService.GetClaimsAsync(userId);
            var userClaim = userClaims.FirstOrDefault();

            Assert.AreEqual(userClaim.Type, claim.Type);
            Assert.AreEqual(userClaim.Value, claim.Value);
        }

        // тестируем удаление утверждения из юзера
        [Test]
        public async Task UserIntegrTest_18DeletingClaimFromUser()
        {
            var userId = _usersIds[0];
            var userClaims = await _userService.GetClaimsAsync(userId);
            Assert.AreEqual(1, userClaims.Count);

            await _userService.RemoveClaimAsync(userId, userClaims.FirstOrDefault());

            userClaims = await _userService.GetClaimsAsync(userId);
            Assert.AreEqual(0, userClaims.Count);
        }

        // тестируем добавление логина в юзера
        [Test]
        public async Task UserIntegrTest_19AddingLoginToUser()
        {
            var userId = _usersIds[0];
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            await _userService.AddLoginAsync(userId, login);

            var userLogins = await _userService.GetLoginsAsync(userId);
            var userLogin = userLogins.FirstOrDefault();

            Assert.AreEqual(login, userLogin);
        }

        // тестируем удаление логина из юзера
        [Test]
        public async Task UserIntegrTest_20DeletingLoginFromUser()
        {
            var userId = _usersIds[0];
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            var userLogins = await _userService.GetLoginsAsync(userId);

            Assert.AreEqual(1, userLogins.Count);
            await _userService.RemoveLoginAsync(userId, login);

            userLogins = await _userService.GetLoginsAsync(userId);
            Assert.AreEqual(0, userLogins.Count);
        }
                
        // тестируем изменение пароля пользователя
        [Test]
        public async Task UserIntegrTest_21ResettingUserPassword()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1");
            await _userService.ChangePasswordAsync(user.Id, "qwerty1", "New password");

            user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "New password");
            await _userService.ChangePasswordAsync(user.Id, "New password", "qwerty1");
        }

        // тестируем добавление друзей
        [Test]
        public async Task UserIntegrTest_22AddingNewFriendsToUser()
        {
            var friends = await _userService.GetFriendsAsync(_usersIds[0]);
            Assert.AreEqual(0, friends.Count);

            await _userService.AddFriendAsync(_usersIds[0], _usersIds[1]);
            await _userService.AddFriendAsync(_usersIds[0], _usersIds[2]);

            friends = await _userService.GetFriendsAsync(_usersIds[0]);
            Assert.AreEqual(2, friends.Count);
            Assert.IsTrue(friends.Any(u => u.Id == _usersIds[1]) && friends.Any(u => u.Id == _usersIds[2]));
            Assert.IsTrue(friends.Any(u => u.UserName == "Anton") && friends.Any(u => u.UserName == "Alexander"));
        }

        // тестируем удаление друзей
        [Test]
        public async Task UserIntegrTest_23DeletingFriendsFromUser()
        {
            var friends = await _userService.GetFriendsAsync(_usersIds[0]);
            Assert.AreEqual(2, friends.Count);

            await _userService.DeleteFriendAsync(_usersIds[0], _usersIds[1]);
            await _userService.DeleteFriendAsync(_usersIds[0], _usersIds[2]);

            friends = await _userService.GetFriendsAsync(_usersIds[0]);
            Assert.AreEqual(0, friends.Count);
        }

        // тестируем создание продукта (не относится к юзер сервису, но необходимо для следующего теста)
        [Test]
        public async Task UserIntegrTest_24AddingNewProductsToDB()
        {
            string productName = "Waste product";

            using (var prodService = _kernel.Get<IProductService>())
            {
                prodService.Add(productName, out var addedProduct);
                var product = await prodService.GetByNameAsync(productName);

                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.Name);
                _productIds.Add(product.Id);
            }
        }

        // тестируем добавление продукта
        [Test]
        public async Task UserIntegrTest_25AddingNewProductsToUser()
        {
            string description = "Tastes like garbage, won't buy it ever again.";

            var products = await _userService.GetProductDescriptionsAsync(_usersIds[0]);
            Assert.AreEqual(0, products.Count);

            await _userService.AddProductAsync(_usersIds[0], _productIds[0], 1, description);
            products = await _userService.GetProductDescriptionsAsync(_usersIds[0]);

            Assert.AreEqual(1, products.Count);
            Assert.AreEqual(_productIds[0], products[0].Product.Id);
            Assert.AreEqual(1, products[0].Rating);
            Assert.AreEqual(description, products[0].Description);
        }

        // тестируем удаление продуктов
        [Test]
        public async Task UserIntegrTest_26DeletingProductsFromUser()
        {
            var products = await _userService.GetProductDescriptionsAsync(_usersIds[0]);
            Assert.AreEqual(1, products.Count);

            await _userService.DeleteProductAsync(_usersIds[0], _productIds[0]);

            products = await _userService.GetProductDescriptionsAsync(_usersIds[0]);
            Assert.AreEqual(0, products.Count);
        }

        // тестируем поиск роли по айди и имени
        [Test]
        public async Task UserIntegrTest_27FindRoleByIdAndName()
        {
            UserRole foundByName = await _roleService.FindByNameAsync("Simple user");
            Assert.AreEqual("Simple user", foundByName.Name);

            UserRole foundById = await _roleService.FindByIdAsync(foundByName.Id);
            Assert.AreEqual(foundByName.Name, foundById.Name);
            Assert.AreEqual(foundByName.Id, foundById.Id);
        }

        // тестируем получение всех пользователей определенной роли
        [Test]
        public async Task UserIntegrTest_28GettingRoleUsers()
        {
            User user1 = await _userService.LogInByEmailAsync("test50someemail@gmail.com", "qwerty2");
            User user2 = await _userService.LogInByEmailAsync("test51someemail@gmail.com", "qwerty3");
            UserRole role = await _roleService.FindByNameAsync("Simple user");

            IEnumerable<User> users = await _roleService.GetRoleUsers(role);
            User user1FromGetRoles = users.FirstOrDefault(u => u.Id == user1.Id);
            User user2FromGetRoles = users.FirstOrDefault(u => u.Id == user2.Id);
            Assert.AreEqual(user1.Id, user1FromGetRoles.Id);
            Assert.AreEqual(user2.Id, user2FromGetRoles.Id);
        }

        // тестируем изменение названия роли
        [Test]
        public async Task UserIntegrTest_29UpdatingRoleName()
        {
            var userId = _usersIds[1];

            var rolesOfUser = await _userService.GetRolesAsync(userId);
            Assert.AreEqual("Simple user", rolesOfUser.FirstOrDefault());

            UserRole role = await _roleService.FindByNameAsync("Simple user");
            await _roleService.UpdateRoleNameAsync(role, "Not so simple user");

            rolesOfUser = await _userService.GetRolesAsync(userId);
            Assert.AreEqual("Not so simple user", rolesOfUser.FirstOrDefault());
        }

        // тестируем удаление роли
        [Test]
        public async Task UserIntegrTest_30DeletingRole()
        {
            var userId = _usersIds[1];

            var rolesOfUser = await _userService.GetRolesAsync(userId);
            Assert.AreEqual("Not so simple user", rolesOfUser.FirstOrDefault());

            UserRole role = await _roleService.FindByNameAsync("Not so simple user");
            await _roleService.DeleteAsync(role);

            rolesOfUser = await _userService.GetRolesAsync(userId);
            Assert.IsNull(rolesOfUser.FirstOrDefault());
        }

        // тестируем удаление юзеров, а заодно и чистим базу до изначального состояния
        [Test]
        public async Task UserIntegrTest_31DeletingUsers()
        {
            foreach (var id in _usersIds)
            {
                await _userService.DeleteUserAsync(id);
            }
        }
    }
}
