using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
                GroupId = "00000000-0000-0000-0000-000000000001",
                UserId = "2"
            };
            _groupUserDB = new GroupUserDB
            {
                GroupId = "00000000-0000-0000-0000-000000000001",
                RightToCreateBoards = true,
                UserId = "2",
                IsConfirmed = false
            };

            _groupDB = new GroupDB
            {
                Id = "00000000-0000-0000-0000-000000000001",
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
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, bool>>()))
                .Returns(_selectedUserList);

            _groupUserService.Invite(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupUserDB>()), Times.Once);
        }
        [Test]
        public void SendInvite_02_SendInvite_02_Send_Invite_Not_New_User()
        {
            _selectedList.Add(_groupDB);
            _groupUserDB.IsConfirmed = true;
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, bool>>()))
                .Returns(_selectedUserList);

            _groupUserService.Invite(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupUserDB>()), Times.Never);
        }
        [Test]
        public void SendInvite_03_SendInvite_03_Send_Invite_Where_Group_Unavalible_or_User_In_Group()
        {
            _groupUserDB.IsConfirmed = true;
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, bool>>()))
                .Returns(_selectedUserList);

            _groupUserService.Invite(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupUserDB>()), Times.Never);
            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupUserDB>()), Times.Never);
        }

        [Test]
        public void SendInvite_04_DismissUser_01_Dismiss_User()
        {
            _selectedList.Add(_groupDB);
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, bool>>()))
                .Returns(_selectedUserList);

            _groupUserService.Kick(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.DeleteUserFromGroupAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        [Test]
        public void SendInvite_05_DismissUser_02_User_Unavalible_or_Group_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, bool>>()))
                .Returns(_selectedUserList);

            _groupUserService.Kick(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupUserDB>()), Times.Never);
        }

        [Test]
        public void SendInvite_06_GetEntitle_01_Get_Entitle()
        {
            _selectedList.Add(_groupDB);
            _groupUserDB.RightToCreateBoards = false;
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, bool>>()))
                .Returns(_selectedUserList);

            _groupUserService.GiveRightToCreateBoards(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupUserDB>()), Times.Once);
        }
        [Test]
        public void SendInvite_07_GetEntitle_02_Group_Unavalible_or_User_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .Returns(_selectedList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, bool>>()))
                .Returns(_selectedUserList);

            _groupUserService.GiveRightToCreateBoards(_groupUser, "2");

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupUserDB>()), Times.Never);
        }
    }
}