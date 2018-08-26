using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Repositories.UserManagement;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Services.MailService;
using WasteProducts.Logic.Services;
using WasteProducts.Logic.Services.MailService;
using AutoMapper;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Services.UserService;
using System.Net.Mail;
using WasteProducts.Logic.Mappings.UserMappings;
using WasteProducts.Logic.Common.Models.Products;
using System.Linq.Expressions;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Barcods;
using System.Threading;

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
            _mailServiceMock = new Mock<IMailService>();
            _userRepoMock = new Mock<IUserRepository>();
            _userServiceMock = new Mock<IUserService>();
            _userService = new UserService(_mailServiceMock.Object, _userRepoMock.Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new UserClaimProfile());
                cfg.AddProfile(new UserLoginProfile());
                cfg.AddProfile(new ProductProfile());
            });

            _mapper = (new Mapper(config)).DefaultContext.Mapper;
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

        [Test]
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
                Password = expectedPassword,
                UserName = expectedUserName,
                AccessFailedCount = 0,
                Claims = new List<System.Security.Claims.Claim>(),
                EmailConfirmed = false,
                Friends = new List<User>(),
                LockoutEnabled = false,
                LockoutEndDateUtc = null,
                Logins = new List<UserLogin>(),
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                Products = new List<Product>(),
                Roles = new List<string>(),
                SecurityStamp = null,
                TwoFactorEnabled = false
            };

            _userRepoMock.Setup(a => a.AddAsync(It.IsAny<UserDB>())).Returns(Task.CompletedTask);
            _userRepoMock.Setup(b => b.Select(It.Is<string>(c => c == validEmail),
                It.IsAny<bool>())).Returns(_mapper.Map<UserDB>(expectedUser));

            // act
            var actualUser = _userService.RegisterAsync(validEmail,
                userNameValid, password, passwordConfirmationMatch)
                .GetAwaiter().GetResult();


            // assert
            Assert.AreEqual(expectedUser.AccessFailedCount, actualUser.AccessFailedCount);
            Assert.AreEqual(expectedUser.Claims, actualUser.Claims);
            Assert.AreEqual(expectedUser.EmailConfirmed, actualUser.EmailConfirmed);
            Assert.AreEqual(expectedUser.Friends, actualUser.Friends);
            Assert.AreEqual(expectedUser.Id, actualUser.Id);
            Assert.AreEqual(expectedUser.LockoutEnabled, actualUser.LockoutEnabled);
            Assert.AreEqual(expectedUser.LockoutEndDateUtc, actualUser.LockoutEndDateUtc);
            Assert.AreEqual(expectedUser.Logins, actualUser.Logins);
            Assert.AreEqual(expectedUser.Password, actualUser.Password);
            Assert.AreEqual(expectedUser.Email, actualUser.Email);
            Assert.AreEqual(expectedUser.PhoneNumber, actualUser.PhoneNumber);
            Assert.AreEqual(expectedUser.PhoneNumberConfirmed, actualUser.PhoneNumberConfirmed);
            Assert.AreEqual(expectedUser.Products, actualUser.Products);
            Assert.AreEqual(expectedUser.Roles, actualUser.Roles);
            Assert.AreEqual(expectedUser.SecurityStamp, actualUser.SecurityStamp);
            Assert.AreEqual(expectedUser.TwoFactorEnabled, actualUser.TwoFactorEnabled);
            Assert.AreEqual(expectedUser.UserName, actualUser.UserName);
            _userRepoMock.Verify(m => m.Select(It.Is<string>(c => c == validEmail),
                It.IsAny<bool>()), Times.Once());
        }

        [Test]
        public void UserServiceTest_05_LogInAsync_Not_Existing_Email_Returns_Null()
        {
            // arrange
            var invalidEmail = "invalidEmail@gmail.com";
            var password = "password";
            UserDB userDB = new UserDB();
            userDB.Email = "validEmail@gmail.com";
            userDB.PasswordHash = "password";
            var userName = "UserName";
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
        public void UserServiceTest_06_PasswordRequestAsync_Not_Existing_Email_Dont_Send_Email()
        {
            // arrange
            var invalidEmail = "incorrectEmail";
            _userRepoMock.Setup(b => b.Select(It.Is<string>(c => 
            c == invalidEmail), It.IsAny<bool>())).Returns((UserDB)null);

            _mailServiceMock.Setup(a => a.SendAsync(It.Is<string>(c => 
            c == invalidEmail), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //act
            _userService.PasswordRequestAsync(invalidEmail).GetAwaiter().GetResult();

            //assert
            _userRepoMock.Verify(m => m.Select(It.Is<string>(c => 
            c == invalidEmail), It.IsAny<bool>()), Times.Once());

            _mailServiceMock.Verify(m => m.SendAsync(It.Is<string>(c => 
            c == invalidEmail), It.IsAny<string>(), It.IsAny<string>()), 
            Times.Never());
        }

        [Test]
        public void UserServiceTest_07_PasswordRequest_Existing_Email_Sends_Email_Once()
        {
            // arrange
            var existingEmail = "existingEmail";
            UserDB userDB = new UserDB();
            userDB.PasswordHash = "passwordHash";
            _userRepoMock.Setup(b => b.Select(It.Is<string>(c =>
            c == existingEmail), It.IsAny<bool>())).Returns(userDB);
            _mailServiceMock.Setup(a => a.SendAsync(It.Is<string>(c =>
            c == existingEmail), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            // act
            _userService.PasswordRequestAsync(existingEmail).GetAwaiter().GetResult();

            // assert
            _userRepoMock.Verify(m => m.Select(It.Is<string>(c =>
            c == existingEmail), It.IsAny<bool>()), Times.Once());

            _mailServiceMock.Verify(m => m.SendAsync(It.Is<string>(c =>
            c == existingEmail), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once());
        }

        [Test] 
        public void UserServiceTest_08_GetRolesAsync_Existing_User_Returns()
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

        [Test]
        public void UserServiceTest_09_AddProductAsync_User_Already_Has_The_Product_With_Same_Barcode_Code_Dont_Add()
        {
            // arrange
            User user = new User();
            Product product = new Product();
            product.Barcode = new Barcode();
            user.Products = new List<Product>();
            product.Barcode.Code = "code";
            user.Products.Add(product);
            _userServiceMock.Setup(a => a.UpdateAsync(It.IsAny<User>())).Verifiable();


            // act
            _userService.AddProductAsync(user, product).GetAwaiter().GetResult();


            // assert
            _userServiceMock.Verify(a => a.UpdateAsync(It.IsAny<User>()), 
                Times.Never());

        }

        [Test]
        public void UserServiceTest_10_AddProductAsync_User_Already_Has_The_Product_With_Same_Barcode_ID_Dont_Add()
        {
            // arrange
            User user = new User();
            Product product = new Product();
            product.Barcode = new Barcode() { Id = "identification" };
            user.Products = new List<Product>();
            user.Products.Add(product);
            _userServiceMock.Setup(a => a.UpdateAsync(It.IsAny<User>())).Verifiable();

            // act
            _userService.AddProductAsync(user, product).GetAwaiter().GetResult();

            // assert
            _userServiceMock.Verify(a => a.UpdateAsync(It.IsAny<User>()),
                Times.Never());
        }

        [Test]
        public void UserServiceTest_11_AddProductAsync_User_Doesnt_Have_The_Product_With_Same_Barcode_ID_or_Code_New_Product_Added()
        {
            // arrange
            User user = new User();
            user.Id = "asdas";

            Product hasProduct = new Product
            {
                Barcode = new Barcode()
                {
                    Id = "identification",
                    Code = "code"
                }
            };
            user.Products = new List<Product>
            {
                hasProduct
            };

            Product newProduct = new Product
            {
                Barcode = new Barcode()
                {
                    Id = "anotherIdentification",
                    Code = "anotherCode"

                }
            };
            _userRepoMock.Setup(a => a.UpdateAsync(It.Is<UserDB>(u => u.Id == "asdas"))).Verifiable();

            // act
            _userService.AddProductAsync(user, newProduct);
            Thread.Sleep(20);
            // assert
            _userRepoMock.Verify(a => a.UpdateAsync(It.Is<UserDB>(u => u.Id == "asdas")),
                Times.Once());

        }

        // AddProductAsync
        // DeleteProductAsync
    }
}
