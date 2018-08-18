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

namespace WasteProducts.Logic.Tests.UserManagementTests
{
    [TestFixture]
    public class UserServiceTest
    {
        private Mock<IUserService> userServicemock;
        private Mock<IUserRepository> userRepository;
        private Mock<IMailService> mailService;
        private UserService userService;

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {
            userServicemock = new Mock<IUserService>();


            var userDB = new UserDB()
            {
                Id = "1",
                Email = "Email",
                UserName = "userName",
                Created = DateTime.Now.AddYears(-1),
                Modified = DateTime.Now.AddMinutes(-1)
            };
            var user = new User()
            {
                Id = "1",
                Email = "Email",
                UserName = "userName"
            };

            userRepository = new Mock<IUserRepository>();
            #region userRepository SetUp
            userRepository.Setup(a => a.Select("Does not exist")).Returns((UserDB) null);
            userRepository.Setup(a => a.Select("Exists")).Returns(userDB);
            #endregion




            mailService = new Mock<IMailService>();
            mailService.Setup(a => a.
            Send(It.IsAny<string>(), 
                 It.IsAny<string>(), 
                 It.IsAny<string>())).
                 Verifiable();

            userService = new UserService(mailService.Object, userRepository.Object);


            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(a => a.Map<User>(It.IsAny<UserDB>())).
                Returns(user);

        }


        [Test]
        public void PasswordRequest_NotExistingEmailReturnsFalse_Test()
        {
            // arrange
            bool expected = false;
            // act
            var actual = userService.PasswordRequest("Does not exist");
            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PasswordRequest_ExistingEmailMailServiceSendMethodIsEvoked_Test()
        {
            // arrange
            
            // act
            userService.PasswordRequest("Exists");

            // assert
            mailService.Verify(a => a.Send(It.IsAny<string>(),
                 It.IsAny<string>(),
                 It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void PasswordRequest_ExistingEmailMailServiceReturnsTrue_Test()
        {
            // arrange
            bool expected = true;

            // act 
            var actual = userService.PasswordRequest("Exists");

            // assert
            Assert.AreEqual(expected, actual);

        }

    }
}
