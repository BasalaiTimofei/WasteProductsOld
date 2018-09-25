using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Mappings.Groups;
using WasteProducts.Logic.Services.Groups;

namespace WasteProducts.Logic.Tests.GroupManagementTests
{
    public class GroupCommentServiceITests
    {
        private GroupComment _groupComment;
        private GroupCommentDB _groupCommentDB;
        private GroupBoardDB _groupBoardDB;
        private GroupUserDB _groupUserDB;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private GroupCommentService _groupCommentService;
        private IRuntimeMapper _mapper;
        private List<GroupCommentDB> _selectedCommentList;
        private List<GroupBoardDB> _selectedBoardList;
        private List<GroupUserDB> _selectedUserList;

        [OneTimeSetUp]
        public void TestFixtureSetup()
        {

        }
        [SetUp]
        public void TestCaseSetup()
        {
            _groupComment = new GroupComment
            {
                Id = "00000000-0000-0000-0000-000000000000",
                Comment ="comment",
                CommentatorId ="2",
                GroupBoardId = "00000000-0000-0000-0000-000000000000"
            };
            _groupCommentDB = new GroupCommentDB
            {
                Id = "00000000-0000-0000-0000-000000000000",
                Comment = "comment",
                CommentatorId = "2",
                GroupBoardId = "00000000-0000-0000-0000-000000000000"
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
            _groupCommentService = new GroupCommentService(_groupRepositoryMock.Object, _mapper);
            _selectedCommentList = new List<GroupCommentDB>();
            _selectedBoardList = new List<GroupBoardDB>();
            _selectedUserList = new List<GroupUserDB>();
        }

        [Test]
        public void GroupCommentService_01_Create_01_Create_New_Comment()
        {
            _selectedUserList.Add(_groupUserDB);
            _selectedBoardList.Add(_groupBoardDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);

            var x = Task.Run(()=>_groupCommentService
                .Create(_groupComment, "00000000-0000-0000-0000-000000000001")).Result;

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupCommentDB>()), Times.Once);
        }
        [Test]
        public void GroupCommentService_01_Create_02_Board_Unavalible_or_User_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);

            Assert.ThrowsAsync<ValidationException>(()=>_groupCommentService.Create(_groupComment,
              "00000000-0000-0000-0000-000000000001"));
        }

        [Test]
        public void GroupCommentService_02_Update_01_Update_Your_Comment()
        {
            _selectedUserList.Add(_groupUserDB);
            _selectedBoardList.Add(_groupBoardDB);
            _selectedCommentList.Add(_groupCommentDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .ReturnsAsync(_selectedCommentList);

            Task.Run(() => _groupCommentService.Update(_groupComment, 
                "00000000-0000-0000-0000-000000000001")).Wait();

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupCommentDB>()), Times.Once);
        }
        [Test]
        public void GroupCommentService_02_Update_02_Board_Unavalible_or_User_Unavalible_or_Comment_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .ReturnsAsync(_selectedCommentList);

            Assert.ThrowsAsync<ValidationException>(() => _groupCommentService.Update(_groupComment,
              "00000000-0000-0000-0000-000000000001"));
        }

        [Test]
        public void GroupCommentService_03_Delete_01_Delete_Your_Comment()
        {
            _selectedUserList.Add(_groupUserDB);
            _selectedBoardList.Add(_groupBoardDB);
            _selectedCommentList.Add(_groupCommentDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .ReturnsAsync(_selectedCommentList);

            Task.Run(()=>_groupCommentService.Delete(_groupComment, "00000000-0000-0000-0000-000000000002")).Wait();

            _groupRepositoryMock.Verify(m => m.Delete(It.IsAny<GroupCommentDB>()), Times.Once);
        }
        [Test]
        public void GroupCommentService_03_Delete_02_Board_Unavalible_or_User_Unavalible_or_Comment_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .ReturnsAsync(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .ReturnsAsync(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .ReturnsAsync(_selectedCommentList);

            Assert.ThrowsAsync<ValidationException>(() => _groupCommentService.Delete(_groupComment,
              "00000000-0000-0000-0000-000000000001"));
        }

        [Test]
        public void GroupCommentService_04_FindById_01_Get_Comment_By_Id()
        {
            _selectedCommentList.Add(_groupCommentDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .ReturnsAsync(_selectedCommentList);

            var result = Task.Run(()=>_groupCommentService.FindById("00000000-0000-0000-0000-000000000001")).Result;

            Assert.AreEqual(_groupComment.Id, result.Id);
            Assert.AreEqual(_groupComment.CommentatorId, result.CommentatorId);
            Assert.AreEqual(_groupComment.GroupBoardId, result.GroupBoardId);
        }
        [Test]
        public void GroupCommentService_04_FindById_02_GroupCommentDB_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .ReturnsAsync(_selectedCommentList);

            var result = Task.Run(() => _groupCommentService.FindById("00000000-0000-0000-0000-000000000001")).Result;

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GroupCommentService_04_FindtBoardComment_01_Get_Comments_By_BoardId()
        {
            _selectedCommentList.Add(_groupCommentDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .ReturnsAsync(_selectedCommentList);

            var result = Task.Run(() => _groupCommentService
                .FindtBoardComment("00000000-0000-0000-0000-000000000001")).Result
                .FirstOrDefault();

            Assert.AreEqual(_groupComment.Id, result.Id);
            Assert.AreEqual(_groupComment.CommentatorId, result.CommentatorId);
            Assert.AreEqual(_groupComment.GroupBoardId, result.GroupBoardId);
        }
        [Test]
        public void GroupCommentService_04_FindtBoardComment_02_GroupCommentDB_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .ReturnsAsync(_selectedCommentList);

            var result = Task.Run(() => _groupCommentService
                .FindtBoardComment("00000000-0000-0000-0000-000000000001")).Result;

            Assert.AreEqual(null, result);
        }
    }
}
