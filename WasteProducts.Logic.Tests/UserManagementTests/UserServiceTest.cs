using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.Logic.Common.Services.MailService;
using AutoMapper;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Services.UserService;
using WasteProducts.Logic.Mappings.UserMappings;

namespace WasteProducts.Logic.Tests.UserManagementTests
{
    [TestFixture]
    public class UserServiceTest
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IMailService> _mailServiceMock;
        private Mock<IUserService> _userServiceMock;
        private UserService _userService;
        private IRuntimeMapper _mapper;

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new UserClaimProfile());
                cfg.AddProfile(new UserLoginProfile());
                cfg.AddProfile(new ProductProfile());
            });

            _mapper = (new Mapper(config)).DefaultContext.Mapper;
        }

        [SetUp]
        public void TestCaseSetup()
        {
            _mailServiceMock = new Mock<IMailService>();
            _userRepoMock = new Mock<IUserRepository>();
            _userServiceMock = new Mock<IUserService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<UserClaimProfile>();
                cfg.AddProfile<UserLoginProfile>();
                cfg.AddProfile<Mappings.UserMappings.ProductProfile>();
                cfg.AddProfile<UserProductDescriptionProfile>();
            });
            var mapper = new Mapper(config);

            _userService = new UserService(_userRepoMock.Object, mapper, _mailServiceMock.Object);
        }

        [TearDown]
        public void TestCaseTeatDown()
        {
            _mailServiceMock = null;
            _userRepoMock = null;
            _userServiceMock = null;
            _userService = null;
        }

        [Test]
        public void UserServiceTest_01_RegisterAsync_Password_and_Confirmation_Doesnt_Match_Returns_Null()
        {
            // arrange
            var validEmail = "validEmail@gmail.com";
            _mailServiceMock.Setup(a => a.IsValidEmail(validEmail)).Returns(true);
            var password = "password";
            var passwordConfirmationDoesntMatch = "doesn't match";
            var userNameValid = "validUserName";
            User expected = null;

            // act
            var actual = _userService.RegisterAsync(validEmail, 
                userNameValid, password, 
                passwordConfirmationDoesntMatch)
                .GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UserServiceTest_02_RegisterAsync_Email_Is_Not_Valid_Returns_Null()
        {
            // arrange
            var invalidEmail = "invalidEmail@gmail.com";
            _mailServiceMock.Setup(a => a.IsValidEmail(invalidEmail)).Returns(false);
            var password = "password";
            var passwordConfirmationMatch = "password";
            var userNameValid = "validUserName";
            User expected = null;

            // act
            var actual = _userService.RegisterAsync(invalidEmail,
                userNameValid, password, passwordConfirmationMatch)
                .GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UserServiceTest_03_RegisterAsync_Confirmation_and_Email_Invalid_Returns_Null()
        {
            // arrange
            var invalidEmail = "invalidEmail@gmail.com";
            _mailServiceMock.Setup(a => a.IsValidEmail(invalidEmail)).Returns(false);
            var password = "password";
            var passwordConfirmationDoesntMatch = "doesn't match";
            var userNameValid = "validUserName";
            User expected = null;

            // act
            var actual = _userService.RegisterAsync(invalidEmail,
                userNameValid, password, passwordConfirmationDoesntMatch)
                .GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(expected, actual);
        }

        //[Test]
        public void UserServiceTest_04_RegisterAsync_Successful_Register_Returns_Task_User()
        {
            // arrange
            var validEmail = "validEmail@gmail.com";
            _mailServiceMock.Setup(a => a.IsValidEmail(validEmail)).Returns(true);
            var password = "password";
            var passwordConfirmationMatch = "password";
            var userNameValid = "validUserName";
            var expectedEmail = validEmail;
            var expectedPassword = password;
            var expectedUserName = userNameValid;

            User expectedUser = new User()
            {
                Email = expectedEmail,
                UserName = expectedUserName,
                Claims = new List<System.Security.Claims.Claim>(),
                Friends = new List<User>(),
                Logins = new List<UserLogin>(),
                PhoneNumber = null,
                ProductDescriptions = new List<UserProductDescription>(),
                Roles = new List<string>(),
            };

            _userRepoMock.Setup(a => a.AddAsync(It.IsAny<UserDB>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            _userRepoMock.Setup(b => b.Select(It.Is<string>(c => c == validEmail),
                It.IsAny<bool>())).Returns(_mapper.Map<UserDB>(expectedUser));

            // act
            var actualUser = _userService.RegisterAsync(validEmail,
                userNameValid, password, passwordConfirmationMatch)
                .GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(expectedUser.Claims, actualUser.Claims);
            Assert.AreEqual(expectedUser.Friends, actualUser.Friends);
            Assert.AreEqual(expectedUser.Id, actualUser.Id);
            Assert.AreEqual(expectedUser.Logins, actualUser.Logins);
            Assert.AreEqual(expectedUser.Email, actualUser.Email);
            Assert.AreEqual(expectedUser.PhoneNumber, actualUser.PhoneNumber);
            Assert.AreEqual(expectedUser.ProductDescriptions, actualUser.ProductDescriptions);
            Assert.AreEqual(expectedUser.Roles, actualUser.Roles);
            Assert.AreEqual(expectedUser.UserName, actualUser.UserName);
            _userRepoMock.Verify(m => m.Select(It.Is<string>(c => c == validEmail),
                It.IsAny<bool>()), Times.Once());
        }

        [Test]
        public void UserServiceTest_04_LogInAsync_Not_Existing_Email_Returns_Null()
        {
            // arrange
            var invalidEmail = "invalidEmail@gmail.com";
            var password = "password";
            UserDB userDB = new UserDB
            {
                Email = "validEmail@gmail.com",
                PasswordHash = "password"
            };
            IList<string> roles = new List<string>();

            (UserDB, IList<string>) tuple = (null, roles);

            _userRepoMock.Setup(a => a.
            SelectWithRoles(It.IsAny<Func<UserDB, bool>>(),
                It.IsAny<bool>())).Returns(tuple);

            // act
            var actual = _userService.LogInAsync(invalidEmail, password, true).GetAwaiter().GetResult();

            // assert
            Assert.IsNull(actual);
        }

        [Test] 
        public void UserServiceTest_05_GetRolesAsync_Existing_User_Returns()
        {
            // arrange
            IList<string> expected = new List<string>() {"1", "2" };

            _userRepoMock.Setup(a =>
            a.GetRolesAsync(It.IsAny<UserDB>()))
            .Returns(Task.FromResult((expected)));
            User user = new User();

            // act
            var actual = _userService.GetRolesAsync(user)
                .GetAwaiter().GetResult();

            // arrange
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
        }
    }
}
