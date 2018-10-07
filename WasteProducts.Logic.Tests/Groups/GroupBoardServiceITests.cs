using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Mappings.Groups;
using WasteProducts.Logic.Services.Groups;

namespace WasteProducts.Logic.Tests.GroupManagementTests
{
    public class GroupBoardServiceTests
    {
        private GroupBoard _groupBoard;
        private GroupBoardDB _groupBoardDB;
        private GroupUserDB _groupUserDB;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private GroupBoardService _groupBoardService;
        private IRuntimeMapper _mapper;
        private List<GroupBoardDB> _selectedBoardList;
        private List<GroupUserDB> _selectedUserList;

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {

        }
        [SetUp]
        public void TestCaseSetup()
        {
            _groupBoard = new GroupBoard
            {
                Id = "00000000-0000-0000-0000-000000000000",
                CreatorId = "2",
                Information = "Some product",
                Name = "Best",
                GroupId = "00000000-0000-0000-0000-000000000001",
                GroupProducts = null
            };
            _groupBoardDB = new GroupBoardDB
            {
                Id = "00000000-0000-0000-0000-000000000000",
                CreatorId = "2",
                Information = "Some product",
                Name = "Best",
                Created = DateTime.UtcNow,
                Deleted = null,
                IsNotDeleted = true,
                Modified = DateTime.UtcNow,
                GroupId = "00000000-0000-0000-0000-000000000001",
                GroupProducts = null
            };
            _groupBoardDB.GroupProducts = new List<GroupProductDB>
            {
                new GroupProductDB
                {
                    Information="Information"
                }
            };
            _groupUserDB = new GroupUserDB
            {
                GroupId = "00000000-0000-0000-0000-000000000001",
                RightToCreateBoards = true
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
            _groupBoardService = new GroupBoardService(_groupRepositoryMock.Object, _mapper);
            _selectedBoardList = new List<GroupBoardDB>();
            _selectedUserList = new List<GroupUserDB>();

        }

        [Test]
        public void GroupBoardService_01_Create_01_Create_New_GroupBoard()
        {
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            string x = Task.Run(()=> _groupBoardService.Create(_groupBoard)).Result;

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupBoardDB>()), Times.Once);
        }
        [Test]
        public void GroupBoardService_01_Create_02_Group_Unavalible_or_UserGroup_Unavalible_or_User_Dose_Not_Have_Access()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            Assert.ThrowsAsync<ValidationException>(()=> _groupBoardService.Create(_groupBoard));
        }

        [Test]
        public void GroupBoardService_02_Update_01_Add_New_Information_In_GroupBoard()
        {
            _selectedBoardList.Add(_groupBoardDB);
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            Task.Run(() => _groupBoardService.Update(_groupBoard)).Wait();

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupBoardDB>()), Times.Once);
        }
        [Test]
        public void GroupBoardService_02_Update_02_GroupBoard_Unavalible_or_UserGroup_Unavalible_or_User_Dose_Not_Have_Access_or_Board_Unavalible_or_UserGroup_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            Assert.ThrowsAsync<ValidationException>(() => _groupBoardService.Update(_groupBoard));
        }

        [Test]
        public void GroupBoardService_03_Delete_01_Remove_GroupBoard()
        {
            _selectedBoardList.Add(_groupBoardDB);
            _selectedUserList.Add(_groupUserDB);
            _groupRepositoryMock.Setup(m => m.GetWithInclude(
                It.IsAny<Func<GroupBoardDB, Boolean>>(),
                It.IsAny<Expression<Func<GroupBoardDB, object>>[]>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            Task.Run(() => _groupBoardService.Delete("00000000-0000-0000-0000-000000000001")).Wait();

            _groupRepositoryMock.Verify(m => m.Delete(It.IsAny<GroupBoardDB>()), Times.Once);
            _groupRepositoryMock.Verify(m => m.DeleteAll(It.IsAny<List<GroupProductDB>>()), Times.Once);
        }
        [Test]
        public void GroupBoardService_03_Delete_02_GroupBoard_Unavalible_or_UserGroup_Unavalible_or_User_Dose_Not_Have_Access_or_Board_Unavalible_or_UserGroup_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.GetWithInclude(
                It.IsAny<Func<GroupBoardDB, Boolean>>(),
                It.IsAny<Expression<Func<GroupBoardDB, object>>[]>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);

            Assert.ThrowsAsync<ValidationException>(() => _groupBoardService.Delete("00000000-0000-0000-0000-000000000001"));
        }

        [Test]
        public void GroupBoardService_04_FindById_01_Obtainment_Avalible_GroupBoard_By_Id()
        {
            _selectedBoardList.Add(_groupBoardDB);
            _groupRepositoryMock.Setup(m => m.Find(
                It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);

            var result = Task.Run(()=> _groupBoardService.FindById("00000000-0000-0000-0000-000000000000")).Result;
            Assert.AreEqual(_groupBoard.Id, result.Id);
            Assert.AreEqual(_groupBoard.Name, result.Name);
            Assert.AreEqual(_groupBoard.Information, result.Information);
            Assert.AreEqual(_groupBoard.CreatorId, result.CreatorId);
        }
        [Test]
        public void GroupBoardService_04_FindById_02_Obtainment_Unavalible_GroupBoard_By_Id()
        {
            _groupRepositoryMock.Setup(m => m.Find(
                It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);

            var result = Task.Run(() => _groupBoardService.FindById("00000000-0000-0000-0000-000000000000")).Result;
            Assert.AreEqual(null, result);
        }
    }
}