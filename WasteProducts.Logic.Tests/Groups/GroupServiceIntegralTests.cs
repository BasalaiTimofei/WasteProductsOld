using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.ModelConfigurations;
using WasteProducts.DataAccess.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;
using WasteProducts.Logic.Mappings.Groups;
using WasteProducts.Logic.Services.Groups;

namespace WasteProducts.Logic.Tests.UserManagementTests
{
    public class GroupServiceIntegralTests
    {
        private Group _group;
        private GroupDB _groupDB;
        private GroupBoard _groupBoard;
        private GroupBoardDB _groupBoardDB;

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
                Id = (new Guid()),
                AdminId = "2",
                Information = "Some product",
                Name = "Best"
            };
            _groupDB = new GroupDB
            {
                Id = (new Guid()),
                AdminId = "2",
                Information = "Some product",
                Name = "Best",
                Created = DateTime.UtcNow,
                Deleted = null,
                IsNotDeleted = true,
                Modified = DateTime.UtcNow,
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
        public void Create_Create_new_Group()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>()));

            _groupService.Create(_group);

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupDB>()), Times.Once);
        }
        [Test]
        public void GroupService_Create_new_Group_whitch_already_created()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupDB, Boolean>>())).Returns(_selectedList);

            _groupService.Create(_group);

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupDB>()), Times.Never);
        }
    }
}