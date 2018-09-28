using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Mappings.Groups;
using WasteProducts.Logic.Services.Groups;

namespace WasteProducts.Logic.Tests.GroupManagementTests
{
    public class GroupServiceTests
    {
        private Group _group;
        private GroupDB _groupDB;

        private Mock<IGroupRepository> _groupRepositoryMock;
        private GroupService _groupService;
        private IRuntimeMapper _mapper;
        private List<GroupDB> _selectedList;

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {

        }
        [SetUp]
        public void TestCaseSetup()
        {
            _group = new Group
            {
                Id = "00000000-0000-0000-0000-000000000000",
                AdminId = "2",
                Information = "Some product",
                Name = "Best",
                GroupBoards = null,
                GroupUsers = null
            };
            _groupDB = new GroupDB
            {
                Id = "00000000-0000-0000-0000-000000000000",
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
            _groupDB.GroupBoards = new List<GroupBoardDB>
            {
                new GroupBoardDB
                {
                    Name="Name",
                    GroupProducts = new List<GroupProductDB>
                    {
                        new GroupProductDB
                        {
                            Information="Information"
                        }
                    }
                }
            };
            _groupDB.GroupUsers = new List<GroupUserDB>
            {
                new GroupUserDB
                {
                    RightToCreateBoards = true
                }
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
            _groupService = new GroupService(_groupRepositoryMock.Object, _mapper);
            _selectedList = new List<GroupDB>();

        }

        [Test]
        public void GroupService_01_Create_01_Create_New_Group()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .ReturnsAsync(_selectedList);

            var x = Task.Run(()=>_groupService.Create(_group)).Result;

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupDB>()), Times.Once);
        }
        [Test]
        public void GroupService_01_Create_02_User_Already_Created_Group()
        {
            _selectedList.Add(_groupDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .ReturnsAsync(_selectedList);

            Assert.ThrowsAsync<ValidationException>(()=> _groupService.Create(_group));
        }

        [Test]
        public void GroupService_02_Update_01_Add_New_Information_In_Group()
        {
            _selectedList.Add(_groupDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .ReturnsAsync(_selectedList);

            Task.Run(()=>_groupService.Update(_group)).Wait();

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupDB>()), Times.Once);
        }
        [Test]
        public void GroupService_02_Update_02_Group_Did_Not_Created_or_User_Is_Not_Admin()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, bool>>()))
                .ReturnsAsync(_selectedList);

            Assert.ThrowsAsync<ValidationException>(() => _groupService.Update(_group));
        }

        [Test]
        public void GroupService_03_Delete_01_Remove_Information_With_Group()
        {
            _selectedList.Add(_groupDB);
            _groupRepositoryMock.Setup(m => m.GetWithInclude(
                It.IsAny<Func<GroupDB, bool>>(),
                It.IsAny<Expression<Func<GroupDB, object>>[]>()))
                .ReturnsAsync(_selectedList);

            Task.Run(() => _groupService.Delete(_group)).Wait();

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupDB>()), Times.Once);
            _groupRepositoryMock.Verify(m => m.DeleteAll(It.IsAny<List<GroupProductDB>>()), Times.Once);
        }
        [Test]
        public void GroupService_03_Delete_02_Group_Did_Not_Created_or_User_Is_Not_Admin()
        {
            _groupRepositoryMock.Setup(m => m.GetWithInclude(
                It.IsAny<Func<GroupDB, bool>>(),
                It.IsAny<Expression<Func<GroupDB, object>>[]>()))
                .ReturnsAsync(_selectedList);

            Assert.ThrowsAsync<ValidationException>(() => _groupService.Delete(_group));
        }

        [Test]
        public void GroupService_04_FindById_01_Obtainment_Avalible_Group_By_Id()
        {
            _selectedList.Add(_groupDB);
            _groupRepositoryMock.Setup(m => m.GetWithInclude(
                It.IsAny<Func<GroupDB, bool>>(),
                It.IsAny<Expression<Func<GroupDB, object>>[]>()))
                .ReturnsAsync(_selectedList);

            var result = Task.Run(()=>_groupService.FindById("00000000-0000-0000-0000-000000000000")).Result;
            Assert.AreEqual(_group.Id, result.Id);
            Assert.AreEqual(_group.Name, result.Name);
            Assert.AreEqual(_group.Information, result.Information);
            Assert.AreEqual(_group.AdminId, result.AdminId);
        }
        [Test]
        public void GroupService_04_FindById_02_Obtainment_Unavalible_Group_By_Id()
        {
            _groupRepositoryMock.Setup(m => m.GetWithInclude(
                It.IsAny<Func<GroupDB, bool>>(),
                It.IsAny<Expression<Func<GroupDB, object>>[]>()))
                .ReturnsAsync(_selectedList);

            var result = Task.Run(() => _groupService.FindById("00000000-0000-0000-0000-000000000002")).Result;
            Assert.AreEqual(null, result);
        }

        [Test]
        public void GroupService_05_FindByAdmin_01_Input_Id_User()
        {
            _selectedList.Add(_groupDB);
            _groupRepositoryMock.Setup(m => m.GetWithInclude(
                It.IsAny<Func<GroupDB, bool>>(),
                It.IsAny<Expression<Func<GroupDB, object>>[]>()))
                .ReturnsAsync(_selectedList);

            var result = Task.Run(() => _groupService.FindByAdmin("1")).Result;
            Assert.AreEqual(_group.Id, result.Id);
            Assert.AreEqual(_group.Name, result.Name);
            Assert.AreEqual(_group.Information, result.Information);
            Assert.AreEqual(_group.AdminId, result.AdminId);
        }
        [Test]
        public void GroupService_05_FindByAdmin_02_Input_Id_User_But_Group_Is_Unavailable()
        {
            _groupRepositoryMock.Setup(m => m.GetWithInclude(
                It.IsAny<Func<GroupDB, bool>>(),
                It.IsAny<Expression<Func<GroupDB, object>>[]>()))
                .ReturnsAsync(_selectedList);

            var result = Task.Run(() => _groupService.FindByAdmin("2")).Result; ;
            Assert.AreEqual(null, result);
        }
    }
}