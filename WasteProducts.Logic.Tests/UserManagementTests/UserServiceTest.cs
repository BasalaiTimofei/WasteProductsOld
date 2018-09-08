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
            var userNameValid = "validUserName";
            User expected = null;

            // act
            var actual = _userService.RegisterAsync(validEmail, userNameValid, password).GetAwaiter().GetResult();

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
            var userNameValid = "validUserName";
            User expected = null;

            // act
            var actual = _userService.RegisterAsync(invalidEmail, userNameValid, password).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UserServiceTest_03_LogInAsync_Not_Existing_Email_Returns_Null()
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

        //[Test]
        public void UserServiceTest_04_PasswordRequestAsync_Not_Existing_Email_Dont_Send_Email()
        {
            // arrange
            var invalidEmail = "incorrectEmail";
            _userRepoMock.Setup(b => b.Select(It.Is<string>(c => 
            c == invalidEmail), It.IsAny<bool>())).Returns((UserDB)null);

            _mailServiceMock.Setup(a => a.SendAsync(It.Is<string>(c => 
            c == invalidEmail), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //act
            _userService.ResetPasswordAsync(invalidEmail).GetAwaiter().GetResult();

            //assert
            _userRepoMock.Verify(m => m.Select(It.Is<string>(c => 
            c == invalidEmail), It.IsAny<bool>()), Times.Once());

            _mailServiceMock.Verify(m => m.SendAsync(It.Is<string>(c => 
            c == invalidEmail), It.IsAny<string>(), It.IsAny<string>()), 
            Times.Never());
        }

        //[Test]
        public void UserServiceTest_05_PasswordRequest_Existing_Email_Sends_Email_Once()
        {
            // arrange
            var existingEmail = "existingEmail";
            UserDB userDB = new UserDB
            {
                PasswordHash = "passwordHash"
            };

            _userRepoMock.Setup(b => b.Select(It.Is<string>(c =>
            c == existingEmail), It.IsAny<bool>())).Returns(userDB);

            _mailServiceMock.Setup(a => a.SendAsync(It.Is<string>(c =>
            c == existingEmail), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            // act
            _userService.ResetPasswordAsync(existingEmail).GetAwaiter().GetResult();

            // assert
            _userRepoMock.Verify(m => m.Select(It.Is<string>(c =>
            c == existingEmail), It.IsAny<bool>()), Times.Once());

            _mailServiceMock.Verify(m => m.SendAsync(It.Is<string>(c =>
            c == existingEmail), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once());
        }

        [Test] 
        public void UserServiceTest_06_GetRolesAsync_Existing_User_Returns()
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
