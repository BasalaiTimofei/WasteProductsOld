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

namespace WasteProducts.Logic.Tests.UserManagementTests
{
    [TestFixture]
    public class UserServiceTest
    {
        //private Mock<IUserService> userServicemock;
        private Mock<IUserRepository> userRepoMock;
        private Mock<IMailService> mailServiceMock;
        private IMapper mapper;
        private UserService userService;

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {
            //    userServicemock = new Mock<IUserService>();


            //    var userDB = new UserDB()
            //    {
            //        Id = "1",
            //        Email = "Email",
            //        UserName = "userName",
            //        Created = DateTime.Now.AddYears(-1),
            //        Modified = DateTime.Now.AddMinutes(-1)
            //    };
            //    var user = new User()
            //    {
            //        Id = "1",
            //        Email = "Email",
            //        UserName = "userName"
            //    };

            //    userRepository = new Mock<IUserRepository>();
            //    #region userRepository SetUp
            //    userRepository.Setup(a => a.Select("Does not exist")).Returns((UserDB) null);
            //    userRepository.Setup(a => a.Select("Exists")).Returns(userDB);
            //    #endregion




            //    mailService = new Mock<IMailService>();
            //    mailService.Setup(a => a.
            //    Send(It.IsAny<string>(), 
            //         It.IsAny<string>(), 
            //         It.IsAny<string>())).
            //         Verifiable();

            //    userService = new UserService(mailService.Object, userRepository.Object);


            //    
            mailServiceMock = new Mock<IMailService>();
            userRepoMock = new Mock<IUserRepository>();
            userService = new UserService(mailServiceMock.Object, userRepoMock.Object);
        }

        [Test]
        public void UserServiceTest_01_RegisterAsync_Password_and_Confirmation_Doesnt_Match_Returns_Null()
        {
            // arrange
            var validEmail = "validEmail@gmail.com";
            mailServiceMock.Setup(a => a.IsValidEmail(validEmail)).Returns(true);
            var password = "password";
            var passwordConfirmationDoesntMatch = "doesn't match";
            var userNameValid = "validUserName";
            User expected = null;

            // act
            var actual = userService.RegisterAsync(validEmail, 
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
            mailServiceMock.Setup(a => a.IsValidEmail(invalidEmail)).Returns(false);
            var password = "password";
            var passwordConfirmationMatch = "password";
            var userNameValid = "validUserName";
            User expected = null;

            // act
            var actual = userService.RegisterAsync(invalidEmail,
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
            mailServiceMock.Setup(a => a.IsValidEmail(invalidEmail)).Returns(false);
            var password = "password";
            var passwordConfirmationDoesntMatch = "doesn't match";
            var userNameValid = "validUserName";
            User expected = null;

            // act
            var actual = userService.RegisterAsync(invalidEmail,
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
            mailServiceMock.Setup(a => a.IsValidEmail(validEmail)).Returns(true);
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
                UserName = expectedUserName
            };

            userRepoMock.Setup(a => a.AddAsync(It.IsAny<UserDB>())).Returns(Task.CompletedTask);
            userRepoMock.Setup(b => b.Select(It.Is<string>(c => c == validEmail),
                It.IsAny<bool>())).Returns(Mapper.Map<UserDB>(expectedUser));

            // act
            var actualUser = userService.RegisterAsync(validEmail,
                userNameValid, password, passwordConfirmationMatch)
                .GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(expectedUser.Id, actualUser.Id);
            Assert.AreEqual(expectedEmail, actualUser.Email);
            Assert.AreEqual(expectedPassword, actualUser.Password);
            Assert.AreEqual(expectedUserName, actualUser.UserName);
        }



        //[Test]
        //public void PasswordRequest_NotExistingEmailReturnsFalse_Test()
        //{
        //    // arrange
        //    bool expected = false;
        //    // act
        //    var actual = userService.PasswordRequest("Does not exist");
        //    // assert
        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void PasswordRequest_ExistingEmailMailServiceSendMethodIsEvoked_Test()
        //{
        //    // arrange
            
        //    // act
        //    userService.PasswordRequest("Exists");

        //    // assert
        //    mailService.Verify(a => a.Send(It.IsAny<string>(),
        //         It.IsAny<string>(),
        //         It.IsAny<string>()), Times.Once());
        //}

        //[Test]
        //public void PasswordRequest_ExistingEmailMailServiceReturnsTrue_Test()
        //{
        //    // arrange
        //    bool expected = true;

        //    // act 
        //    var actual = userService.PasswordRequest("Exists");

        //    // assert
        //    Assert.AreEqual(expected, actual);

        //}

    }
}
