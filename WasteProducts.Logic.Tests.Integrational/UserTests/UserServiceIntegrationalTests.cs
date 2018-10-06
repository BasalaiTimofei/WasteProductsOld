using FluentValidation;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Diagnostic;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.Groups;
using WasteProducts.Logic.Common.Services.Products;
using WasteProducts.Logic.Common.Services.Users;

namespace WasteProducts.Logic.Tests.UserTests
{
    [TestFixture]
    public class UserServiceIntegrationalTests
    {
        private IUserService _userService;

        private IUserRoleService _roleService;

        private StandardKernel _kernel;

        private readonly List<string> _usersIds = new List<string>();

        private readonly List<string> _productIds = new List<string>();

        private readonly List<string> _groupIds = new List<string>();

        ~UserServiceIntegrationalTests()
        {
            _userService?.Dispose();
            _roleService?.Dispose();
        }

        [OneTimeSetUp]
        public async Task Init()
        {
            _kernel = new StandardKernel();
            _kernel.Load(new DataAccess.InjectorModule(), new Logic.InjectorModule());

            using (var dbService = _kernel.Get<IDiagnosticRepository>())
            {
                await dbService.RecreateAsync().ConfigureAwait(false);
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
            await _userService.RegisterAsync("test49someemail@gmail.com", "Sergei", "qwerty1").ConfigureAwait(false);
            await _userService.RegisterAsync("test50someemail@gmail.com", "Anton", "qwerty2").ConfigureAwait(false);
            await _userService.RegisterAsync("test51someemail@gmail.com", "Alexander", "qwerty3").ConfigureAwait(false);

            var user1 = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").ConfigureAwait(false);
            var user2 = await _userService.LogInByEmailAsync("test50someemail@gmail.com", "qwerty2").ConfigureAwait(false);
            var user3 = await _userService.LogInByEmailAsync("test51someemail@gmail.com", "qwerty3").ConfigureAwait(false);

            Assert.AreEqual("Sergei", user1.UserName);
            Assert.AreEqual("Anton", user2.UserName);
            Assert.IsNotNull(user3.Id);

            _usersIds.Add(user1.Id);
            _usersIds.Add(user2.Id);
            _usersIds.Add(user3.Id);
        }

        // пытаемся зарегистрировать юзера с некорректным емейлом
        [Test]
        public void UserIntegrTest_01AddingUserWithIncorrectEmail()
        {
            Assert.ThrowsAsync<ValidationException>(async () => await _userService.RegisterAsync("Incorrect email", "NewLogin", "qwerty").ConfigureAwait(false));
        }

        // пытаемся зарегистрировать юзера с уже использованным емейлом
        [Test]
        public void UserIntegrTest_02AddingUserWithAlreadyRegisteredEmail()
        {
            Assert.ThrowsAsync<TaskCanceledException>(async () =>  await _userService.RegisterAsync("test49someemail@gmail.com", "NewLogin", "qwerty").ConfigureAwait(false));
        }

        // пытаемся зарегистрировать юзера с неуникальным юзернеймом
        [Test]
        public void UserIntegrTest_03AddingUserWithAlreadyRegisteredNickName()
        {
            Assert.ThrowsAsync<TaskCanceledException>(async () => await _userService.RegisterAsync("test100someemail@gmail.com", "Sergei", "qwerty").ConfigureAwait(false));
        }

        // пытаемся зарегистрировать юзера с null-овыми аргументами, не должно крашить, не должно регистрировать
        [Test]
        public void UserIntegrTest_04RegisteringUserWithNullArguements()
        {
            Assert.ThrowsAsync<ValidationException>(async () => await _userService.RegisterAsync(null, "Sergei1", "qwert1").ConfigureAwait(false));
            Assert.ThrowsAsync<TaskCanceledException>(async () => await _userService.RegisterAsync("test101someemail@gmail.com", null, "qwert2").ConfigureAwait(false));
        }

        // тестируем получение всех зарегистрированных пользователей
        [Test]
        public async Task UserIntegrTest_05GettingAllUsers()
        {
            var users = await _userService.GetAllAsync().ConfigureAwait(false);
            Assert.AreEqual(3, users.Count());
        }

        // проверяем запрос юзера по правильным емейлу и паролю (должно вернуть соответствующего юзера)
        [Test]
        public async Task UserIntegrTest_06CorrectLoggingInByEmail()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").ConfigureAwait(false);
            Assert.AreEqual("Sergei", user.UserName);
        }

        // проверяем запрос юзера по неверным емейлу и паролю (юзер должен быть null-овым)
        [Test]
        public void UserIntegrTest_07IncorrectQueryingByEmail()
        {
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _userService.LogInByEmailAsync("incorrectEmail", "incorrectPassword").ConfigureAwait(false));
        }

        // пытаемся поменять зарегистрированному юзеру емейл на корректный уникальный емейл (должно поменять)
        [Test]
        public async Task UserIntegrTest_08ChangingUserEmailToAvailableEmail()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").ConfigureAwait(false);
            Assert.AreEqual("Sergei", user.UserName);

            bool result = await _userService.UpdateEmailAsync(user.Id, "uniqueemail@gmail.com").ConfigureAwait(false);

            Assert.IsTrue(result);

            await _userService.UpdateEmailAsync(user.Id, "test49someemail@gmail.com").ConfigureAwait(false);
        }

        // пытаемся поменять зарегистрированному юзеру емейл на некорректный уникальный емейл (не должно поменять)
        [Test]
        public async Task UserIntegrTest_09ChangingUserEmailToIncorrectEmail()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").ConfigureAwait(false);
            Assert.AreEqual("Sergei", user.UserName);

            Assert.ThrowsAsync<ValidationException>(async () => await _userService.UpdateEmailAsync(user.Id, "uniqueButIncorrectEmail").ConfigureAwait(false));

            //Assert.IsFalse(result);
            //Assert.AreEqual("Sergei", user.UserName);
        }

        // пытаемся поменять зарегистрированному юзеру емейл на корректный неуникальный емейл (не должно поменять)
        [Test]
        public async Task UserIntegrTest_10ChangingUserEmailToAlreadyRegisteredEmail()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").ConfigureAwait(false);
            Assert.AreEqual("Sergei", user.UserName);

            Assert.ThrowsAsync<TaskCanceledException>(async () => await _userService.UpdateEmailAsync(user.Id, "test50someemail@gmail.com").ConfigureAwait(false));

            //Assert.IsFalse(result);
            //Assert.AreEqual("Sergei", user.UserName);

            user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").ConfigureAwait(false);
            Assert.AreEqual("Sergei", user.UserName);
        }

        // пытаемся передать в метод UpdateEmailAsync null-овые аргументы (не должно поменять емейла, не должно выдать ошибку)
        [Test]
        public async Task UserIntegrTest_11CallUpdateEmailAsyncWithNulArguements()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").ConfigureAwait(false);
            Assert.IsNotNull(user);

            Assert.ThrowsAsync<ValidationException>(async () => await _userService.UpdateEmailAsync(user.Id, null).ConfigureAwait(false));
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _userService.UpdateEmailAsync(null, "correctuniqueemail@gmail.com").ConfigureAwait(false));

            Assert.AreEqual("Sergei", user.UserName);
        }

        // пытаемся изменить юзеру юзернейм на юзернейм, уже имеющийся в системе (не должно поменять)
        [Test]
        public async Task UserIntegrTest_12ChangingUserNameToAlreadyExistingUserName()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").ConfigureAwait(false);
            Assert.AreEqual("Sergei", user.UserName);

            Assert.ThrowsAsync<TaskCanceledException>(async () => await _userService.UpdateUserNameAsync(user.Id, "Anton").ConfigureAwait(false));

            user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").ConfigureAwait(false);

            Assert.AreEqual("Sergei", user.UserName);
        }

        // пытаемся зарегистрировать юзера так, как он будет регистрироваться на самом деле,
        // т.е. с "отправкой" письма на почту (по факту, если использовать настоящий ящик, оно отправляется),
        // где в тестовых целях из методов возвращаются айди и токен, необходимые для подтверждения емейла
        // так же тут тестируется аналогичная "отправка" запроса на изменение пароля (в if statement)
        [Test]
        public async Task UserIntegrTest_13TryingToRegisterUserPropperlyAndResetPassword()
        {
            string email = "test52someemail@gmail.com";
            var (id, token) = await _userService.RegisterAsync(email, "TestName", "TestPassword123").ConfigureAwait(false);
            if (await _userService.ConfirmEmailAsync(id, token).ConfigureAwait(false))
            {
                var resetResult = await _userService.ResetPasswordRequestAsync(email).ConfigureAwait(false);
                await _userService.ResetPasswordAsync(resetResult.UserId, resetResult.Token, "newPassword").ConfigureAwait(false);
                var user = await _userService.LogInByNameAsync("TestName", "newPassword").ConfigureAwait(false);
                Assert.IsNotNull(user);
                Assert.AreEqual(resetResult.UserId, user.Id);
            }
            else
            {
                throw new Exception("Email wasn't confirmed!");
            }
        }

        // тестируем создание роли, а так же проверяем, действительно ли роль создается в базе данных
        [Test]
        public async Task UserIntegrTest_14FindingRoleByCorrectRoleName()
        {
            UserRole roleToCreate = new UserRole() { Name = "Simple user" };
            await _roleService.CreateAsync(roleToCreate).ConfigureAwait(false);

            UserRole role = await _roleService.FindByNameAsync("Simple user").ConfigureAwait(false);
            Assert.AreEqual(role.Name, "Simple user");
        }

        // проверяем запрос роли по несуществующему названию
        [Test]
        public async Task UserIntegrTest_15FindingRoleByIncorrectRoleName()
        {
            UserRole role = await _roleService.FindByNameAsync("Not existing role name").ConfigureAwait(false);
            Assert.IsNull(role);
        }

        // тестим, правильно ли работает функционал добавления роли и добавления юзера в роль, a так же метод GetRolesAsync IUserService
        [Test]
        public async Task UserIntegrTest_16AddingToTheUserDBNewRole()
        {
            await _userService.AddToRoleAsync(_usersIds[0], "Simple user").ConfigureAwait(false);
            await _userService.AddToRoleAsync(_usersIds[1], "Simple user").ConfigureAwait(false);
            await _userService.AddToRoleAsync(_usersIds[2], "Simple user").ConfigureAwait(false);

            var rolesOfUser1 = await _userService.GetRolesAsync(_usersIds[0]).ConfigureAwait(false);
            Assert.AreEqual("Simple user", rolesOfUser1.FirstOrDefault());
        }

        // тестируем изъятие из роли
        [Test]
        public async Task UserIntegrTest_17RemovingUserFromRole()
        {
            var userId = _usersIds[0];
            var userRoles = await _userService.GetRolesAsync(userId).ConfigureAwait(false);
            Assert.AreEqual(userRoles.FirstOrDefault(), "Simple user");
            await _userService.RemoveFromRoleAsync(userId, "Simple user").ConfigureAwait(false);

            userRoles = await _userService.GetRolesAsync(userId).ConfigureAwait(false);
            Assert.IsNull(userRoles.FirstOrDefault());
        }

        // Тестируем добавление утверждения (Claim) в юзера + получение Claims через GetClaimsAsync
        [Test]
        public async Task UserIntegrTest_18AddingClaimToUser()
        {
            var userId = _usersIds[0];
            var claim = new Claim("SomeType", "SomeValue");

            await _userService.AddClaimAsync(userId, claim).ConfigureAwait(false);

            var userClaims = await _userService.GetClaimsAsync(userId).ConfigureAwait(false);
            var userClaim = userClaims.FirstOrDefault();

            Assert.AreEqual(userClaim.Type, claim.Type);
            Assert.AreEqual(userClaim.Value, claim.Value);
        }

        // тестируем удаление утверждения из юзера
        [Test]
        public async Task UserIntegrTest_19DeletingClaimFromUser()
        {
            var userId = _usersIds[0];
            var userClaims = await _userService.GetClaimsAsync(userId).ConfigureAwait(false);
            Assert.AreEqual(1, userClaims.Count);

            await _userService.RemoveClaimAsync(userId, userClaims.FirstOrDefault()).ConfigureAwait(false);

            userClaims = await _userService.GetClaimsAsync(userId).ConfigureAwait(false);
            Assert.IsEmpty(userClaims);
        }

        // тестируем добавление логина в юзера+ получение Loginss через GetLoginsAsync
        [Test]
        public async Task UserIntegrTest_20AddingLoginToUser()
        {
            var userId = _usersIds[0];
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            await _userService.AddLoginAsync(userId, login).ConfigureAwait(false);

            var userLogins = await _userService.GetLoginsAsync(userId).ConfigureAwait(false);
            var userLogin = userLogins.FirstOrDefault();

            Assert.AreEqual(login, userLogin);
        }

        // тестируем удаление логина из юзера
        [Test]
        public async Task UserIntegrTest_21DeletingLoginFromUser()
        {
            var userId = _usersIds[0];
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            var userLogins = await _userService.GetLoginsAsync(userId).ConfigureAwait(false);

            Assert.AreEqual(1, userLogins.Count);
            await _userService.RemoveLoginAsync(userId, login).ConfigureAwait(false);

            userLogins = await _userService.GetLoginsAsync(userId).ConfigureAwait(false);
            Assert.IsEmpty(userLogins);
        }

        // тестируем изменение пароля пользователя
        [Test]
        public async Task UserIntegrTest_22ResettingUserPassword()
        {
            User user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "qwerty1").ConfigureAwait(false);
            await _userService.ChangePasswordAsync(user.Id, "qwerty1", "New password").ConfigureAwait(false);

            user = await _userService.LogInByEmailAsync("test49someemail@gmail.com", "New password").ConfigureAwait(false);
            await _userService.ChangePasswordAsync(user.Id, "New password", "qwerty1").ConfigureAwait(false);
        }

        // тестируем добавление друзей + метод получения списка друзей GetFriendsAsync
        [Test]
        public async Task UserIntegrTest_23AddingNewFriendsToUser()
        {
            var friends = await _userService.GetFriendsAsync(_usersIds[0]).ConfigureAwait(false);
            Assert.IsEmpty(friends);

            await _userService.AddFriendAsync(_usersIds[0], _usersIds[1]).ConfigureAwait(false);
            await _userService.AddFriendAsync(_usersIds[0], _usersIds[2]).ConfigureAwait(false);

            friends = await _userService.GetFriendsAsync(_usersIds[0]).ConfigureAwait(false);
            Assert.AreEqual(2, friends.Count);
            Assert.IsTrue(friends.Any(u => u.Id == _usersIds[1]) && friends.Any(u => u.Id == _usersIds[2]));
            Assert.IsTrue(friends.Any(u => u.UserName == "Anton") && friends.Any(u => u.UserName == "Alexander"));
        }

        // тестируем удаление друзей
        [Test]
        public async Task UserIntegrTest_24DeletingFriendsFromUser()
        {
            var friends = await _userService.GetFriendsAsync(_usersIds[0]).ConfigureAwait(false);
            Assert.AreEqual(2, friends.Count);

            await _userService.DeleteFriendAsync(_usersIds[0], _usersIds[1]).ConfigureAwait(false);
            await _userService.DeleteFriendAsync(_usersIds[0], _usersIds[2]).ConfigureAwait(false);

            friends = await _userService.GetFriendsAsync(_usersIds[0]).ConfigureAwait(false);
            Assert.IsEmpty(friends);
        }

        // тестируем создание продукта (не относится к юзер сервису, но необходимо для следующего теста)
        [Test]
        public async Task UserIntegrTest_25AddingNewProductsToDB()
        {
            using (var prodService = _kernel.Get<IProductService>())
            {
                string name = "SomeProduct";

                string id = await prodService.AddAsync(name).ConfigureAwait(false);

                var product = await prodService.GetByIdAsync(id).ConfigureAwait(false);

                Assert.IsNotNull(product);
                Assert.AreEqual(name, product.Name);

                _productIds.Add(product.Id);
            }
        }


        // тестируем добавление продукта + метод получения списка продуктов GetProductDescriptionsAsync
        [Test]
        public async Task UserIntegrTest_26AddingNewProductsToUser()
        {
            string description = "Tastes like garbage, won't buy it ever again.";

            var products = await _userService.GetProductsAsync(_usersIds[0]).ConfigureAwait(false);
            Assert.IsEmpty(products);

            await _userService.AddProductAsync(_usersIds[0], _productIds[0], 1, description).ConfigureAwait(false);
            products = await _userService.GetProductsAsync(_usersIds[0]).ConfigureAwait(false);

            Assert.AreEqual(1, products.Count);
            Assert.AreEqual(_productIds[0], products[0].Product.Id);
            Assert.AreEqual(1, products[0].Rating);
            Assert.AreEqual(description, products[0].Description);
        }

        // тестируем удаление продуктов
        [Test]
        public async Task UserIntegrTest_27DeletingProductsFromUser()
        {
            var products = await _userService.GetProductsAsync(_usersIds[0]).ConfigureAwait(false);
            Assert.AreEqual(1, products.Count);

            await _userService.DeleteProductAsync(_usersIds[0], _productIds[0]).ConfigureAwait(false);

            products = await _userService.GetProductsAsync(_usersIds[0]).ConfigureAwait(false);
            Assert.IsEmpty(products);
        }

        // тестируем создание группы и приглашение человека в группу (не относится к юзер сервису, но необходимо для следующего теста)
        [Test]
        public async Task UserIntegrTest_28AddingNewGroupToDB()
        {
            var name = "Some group";
            var info = "Info about the group";
            var group = new Group
            {
                Name = name,
                Information = info,
                AdminId = _usersIds[0]
            };
            using (var gService = _kernel.Get<IGroupService>())
            {
                var groupFromDB = await gService.Create(group);
                Assert.AreEqual(info, groupFromDB.Information);
                _groupIds.Add(groupFromDB.Id);
            }

            // группа создана, теперь приглашаем людей
            var groupUser1 = new GroupUser()
            {
                GroupId = _groupIds[0],
                UserId = _usersIds[1]
            };
            var groupUser2 = new GroupUser()
            {
                GroupId = _groupIds[0],
                UserId = _usersIds[2]
            };
            using (var guService = _kernel.Get<IGroupUserService>())
            {
               await guService.Invite(groupUser1, _usersIds[0]);
               await guService.Invite(groupUser2, _usersIds[0]);
            }
        }

        // тестируем ответ на приглашение (один согласится, другой откажется,
        // приглашение отказавшегося должно быть удалено) + получение списка групп
        // при помощи метода GetGroupsAsync
        [Test]
        public async Task UserIntegrTest_29TestingRespondToInvitationToGroup()
        {
            await _userService.RespondToGroupInvitationAsync(_usersIds[1], _groupIds[0], true).ConfigureAwait(false);
            await _userService.RespondToGroupInvitationAsync(_usersIds[2], _groupIds[0], false).ConfigureAwait(false);

            var groups1 = await _userService.GetGroupsAsync(_usersIds[1]).ConfigureAwait(false);
            var groups2 = await _userService.GetGroupsAsync(_usersIds[2]).ConfigureAwait(false);

            Assert.IsEmpty(groups2);

            Assert.AreEqual("Some group", groups1.First().Name);
            Assert.IsFalse(groups1.First().RightToCreateBoards);
        }

        // тестим выход из группы
        [Test]
        public async Task UserIntegrTest_30TestingLeavingAGroup()
        {
            var groups1 = await _userService.GetGroupsAsync(_usersIds[1]).ConfigureAwait(false);
            Assert.AreEqual("Some group", groups1.First().Name);

            await _userService.LeaveGroupAsync(_usersIds[1], _groupIds[0]).ConfigureAwait(false);
            var groups = await _userService.GetGroupsAsync(_usersIds[1]).ConfigureAwait(false);
            Assert.IsEmpty(groups);
        }

        // тестируем поиск роли по айди и имени
        [Test]
        public async Task UserIntegrTest_31FindRoleByIdAndName()
        {
            UserRole foundByName = await _roleService.FindByNameAsync("Simple user").ConfigureAwait(false);
            Assert.AreEqual("Simple user", foundByName.Name);

            UserRole foundById = await _roleService.FindByIdAsync(foundByName.Id).ConfigureAwait(false);
            Assert.AreEqual(foundByName.Name, foundById.Name);
            Assert.AreEqual(foundByName.Id, foundById.Id);
        }

        // тестируем получение всех пользователей определенной роли
        [Test]
        public async Task UserIntegrTest_32GettingRoleUsers()
        {
            User user1 = await _userService.LogInByEmailAsync("test50someemail@gmail.com", "qwerty2").ConfigureAwait(false);
            User user2 = await _userService.LogInByEmailAsync("test51someemail@gmail.com", "qwerty3").ConfigureAwait(false);
            UserRole role = await _roleService.FindByNameAsync("Simple user").ConfigureAwait(false);

            IEnumerable<User> users = await _roleService.GetRoleUsers(role).ConfigureAwait(false);
            User user1FromGetRoles = users.FirstOrDefault(u => u.Id == user1.Id);
            User user2FromGetRoles = users.FirstOrDefault(u => u.Id == user2.Id);
            Assert.AreEqual(user1.Id, user1FromGetRoles.Id);
            Assert.AreEqual(user2.Id, user2FromGetRoles.Id);
        }

        // тестируем изменение названия роли
        [Test]
        public async Task UserIntegrTest_33UpdatingRoleName()
        {
            var userId = _usersIds[1];

            var rolesOfUser = await _userService.GetRolesAsync(userId).ConfigureAwait(false);
            Assert.AreEqual("Simple user", rolesOfUser.FirstOrDefault());

            UserRole role = await _roleService.FindByNameAsync("Simple user").ConfigureAwait(false);
            await _roleService.UpdateRoleNameAsync(role, "Not so simple user").ConfigureAwait(false);

            rolesOfUser = await _userService.GetRolesAsync(userId).ConfigureAwait(false);
            Assert.AreEqual("Not so simple user", rolesOfUser.FirstOrDefault());
        }

        // тестируем удаление роли
        [Test]
        public async Task UserIntegrTest_34DeletingRole()
        {
            var userId = _usersIds[1];

            var rolesOfUser = await _userService.GetRolesAsync(userId).ConfigureAwait(false);
            Assert.AreEqual("Not so simple user", rolesOfUser.FirstOrDefault());

            UserRole role = await _roleService.FindByNameAsync("Not so simple user").ConfigureAwait(false);
            await _roleService.DeleteAsync(role).ConfigureAwait(false);

            rolesOfUser = await _userService.GetRolesAsync(userId).ConfigureAwait(false);
            Assert.IsNull(rolesOfUser.FirstOrDefault());
        }

        // тестируем удаление юзеров, а заодно и чистим базу до изначального состояния
        [Test]
        public async Task UserIntegrTest_35DeletingUsers()
        {
            foreach (var id in _usersIds)
            {
                await _userService.DeleteUserAsync(id).ConfigureAwait(false);
            }
        }
    }
}
