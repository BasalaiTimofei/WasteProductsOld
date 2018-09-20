using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Mappings.Groups;
using WasteProducts.Logic.Services.Groups;

namespace WasteProducts.Logic.Tests.GroupManagementTests
{
    public class GroupUserServiceITests
    {
        private GroupUser _groupUser;
        private GroupUserDB _groupUserDB;
        private GroupDB _groupDB;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private GroupUserService _groupUserService;
        private IRuntimeMapper _mapper;

        private List<GroupUserDB> _selectedUserList;
        private List<GroupDB> _selectedList;

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {

        }
        [SetUp]
        public void TestCaseSetup()
        {
            _groupUser = new GroupUser
            {
                Id = "00000000-0000-0000-0000-000000000002",
                GroupId = "00000000-0000-0000-0000-000000000001",
                UserId = "2"
            };
            _groupUserDB = new GroupUserDB
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                GroupId = new Guid("00000000-0000-0000-0000-000000000001"),
                RightToCreateBoards = true,
                UserId = "2",
                IsInvited = 0
            };

            _groupDB = new GroupDB
            {
                Id = (new Guid("00000000-0000-0000-0000-000000000001")),
                AdminId = "2",
                Information = "Some product",
                Name = "Best",
                Created = DateTime.UtcNow,
                Deleted = null,
                IsNotDeleted = true,
                Modified = DateTime.UtcNow,
                GroupBoards = null,
                GroupUsers = null
            };

            _groupRepositoryMock = new Mock<IGroupRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GroupProfile());
                cfg.AddProfile(new GroupBoardProfile());
                cfg.AddProfile(new GroupProductProfile());
                cfg.AddProfile(new GroupUserProfile());
                cfg.AddProfile(new GroupCommentProfile());
            });

            _mapper = (new Mapper(config)).DefaultContext.Mapper;
            _groupUserService = new GroupUserService(_groupRepositoryMock.Object, _mapper);
            _selectedUserList = new List<GroupUserDB>();
            _selectedList = new List<GroupDB>();
        }

        [Test]
        public void SendInvite_01_SendInvite_01_Send_Invite_New_User()
        {
            _selectedList.Add(_groupDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            _groupUserService.SendInvite(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupUserDB>()), Times.Once);
        }
        [Test]
        public void SendInvite_01_SendInvite_02_Send_Invite_Not_New_User()
        {
            _selectedList.Add(_groupDB);
            _groupUserDB.IsInvited = 2;
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            _groupUserService.SendInvite(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupUserDB>()), Times.Once);
        }
        [Test]
        public void SendInvite_01_SendInvite_03_Send_Invite_Where_Group_Unavalible_or_User_In_Group()
        {
            _groupUserDB.IsInvited = 1;
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            Assert.Throws(typeof(ValidationException),
                    delegate () { _groupUserService.SendInvite(_groupUser, "2"); });
        }

        [Test]
        public void SendInvite_02_DismissUser_01_Dismiss_User()
        {
            _selectedList.Add(_groupDB);
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            _groupUserService.DismissUser(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupUserDB>()), Times.Once);
        }
        [Test]
        public void SendInvite_02_DismissUser_02_User_Unavalible_or_Group_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            Assert.Throws(typeof(ValidationException),
                    delegate () { _groupUserService.DismissUser(_groupUser, "2"); });
        }

        [Test]
        public void SendInvite_03_Enter_01_Enter_In_The_Group()
        {
            _selectedList.Add(_groupDB);
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            _groupUserService.Enter(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupUserDB>()), Times.Once);
        }
        [Test]
        public void SendInvite_03_Enter_02_Group_Unavalible_or_User_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            Assert.Throws(typeof(ValidationException),
                    delegate () { _groupUserService.Enter(_groupUser, "2"); });
        }

        [Test]
        public void SendInvite_04_Leave_01_Leave_Group()
        {
            _selectedList.Add(_groupDB);
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            _groupUserService.Leave(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupUserDB>()), Times.Once);
        }
        [Test]
        public void SendInvite_04_Leave_02_Group_Unavalible_or_User_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            Assert.Throws(typeof(ValidationException),
                    delegate () { _groupUserService.Leave(_groupUser, "2"); });
        }

        [Test]
        public void SendInvite_05_GetEntitle_01_Get_Entitle()
        {
            _selectedList.Add(_groupDB);
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            _groupUserService.GetEntitle(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupUserDB>()), Times.Once);
        }
        [Test]
        public void SendInvite_05_GetEntitle_02_Group_Unavalible_or_User_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            Assert.Throws(typeof(ValidationException),
                    delegate () { _groupUserService.GetEntitle(_groupUser, "2"); });
        }

        [Test]
        public void SendInvite_06_FindReceivedInvites_01_Get_User_Invites()
        {
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            var result = _groupUserService.FindReceivedInvites("2").FirstOrDefault();
            Assert.AreEqual(_groupUser.Id, result.Id);
            Assert.AreEqual(_groupUser.GroupId, result.GroupId);
            Assert.AreEqual(_groupUser.UserId, result.UserId);
        }
        [Test]
        public void SendInvite_06_FindReceivedInvites_02_User_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            var result = _groupUserService.FindReceivedInvites("2");
            Assert.AreEqual(null, result);
        }

        [Test]
        public void SendInvite_07_FindGroupsById_01_Get_GroupsId_Where_User_Entered()
        {
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            var result = _groupUserService.FindGroupsById("2").FirstOrDefault();
            Assert.AreEqual(_groupUser.Id, result.Id);
            Assert.AreEqual(_groupUser.GroupId, result.GroupId);
            Assert.AreEqual(_groupUser.UserId, result.UserId);
        }
        [Test]
        public void SendInvite_07_FindGroupsById_02_UserGroup_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            var result = _groupUserService.FindGroupsById("2");
            Assert.AreEqual(null, result);
        }

        [Test]
        public void SendInvite_08_FindUsersByGroupId_01_Get_Groups_Where_User_Entered()
        {
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            var result = _groupUserService.FindUsersByGroupId(new Guid("00000000-0000-0000-0000-000000000001")).FirstOrDefault();
            Assert.AreEqual(_groupUser.Id, result.Id);
            Assert.AreEqual(_groupUser.GroupId, result.GroupId);
            Assert.AreEqual(_groupUser.UserId, result.UserId);
        }
        [Test]
        public void SendInvite_08_FindUsersByGroupId_02_UserGroup_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);

            var result = _groupUserService.FindUsersByGroupId(new Guid("00000000-0000-0000-0000-000000000001"));
            Assert.AreEqual(null, result);
        }
    }
}