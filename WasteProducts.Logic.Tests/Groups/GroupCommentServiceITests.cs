using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Id = new Guid("00000000-0000-0000-0000-000000000000"),
                Comment = "comment",
                CommentatorId = "2",
                GroupBoardId = new Guid("00000000-0000-0000-0000-000000000000")
            };
            _groupBoardDB = new GroupBoardDB
            {
                Id = new Guid("00000000-0000-0000-0000-000000000000"),
                CreatorId = "2",
                Information = "Some product",
                Name = "Best",
                Created = DateTime.UtcNow,
                Deleted = null,
                IsNotDeleted = true,
                Modified = DateTime.UtcNow,
                GroupId = new Guid("00000000-0000-0000-0000-000000000001"),
                GroupProducts = null
            };
            _groupUserDB = new GroupUserDB
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                GroupId = new Guid("00000000-0000-0000-0000-000000000001"),
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
                .Returns(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .Returns(_selectedBoardList);

            _groupCommentService.Create(_groupComment, new Guid("00000000-0000-0000-0000-000000000001"));

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupCommentDB>()), Times.Once);
        }
        [Test]
        public void GroupCommentService_01_Create_02_Board_Unavalible_or_User_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .Returns(_selectedBoardList);

            _groupCommentService.Create(_groupComment, new Guid("00000000-0000-0000-0000-000000000001"));

            _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupCommentDB>()), Times.Never);
        }

        [Test]
        public void GroupCommentService_02_Update_01_Update_Your_Comment()
        {
            _selectedUserList.Add(_groupUserDB);
            _selectedBoardList.Add(_groupBoardDB);
            _selectedCommentList.Add(_groupCommentDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .Returns(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .Returns(_selectedCommentList);

            _groupCommentService.Update(_groupComment, new Guid("00000000-0000-0000-0000-000000000001"));

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupCommentDB>()), Times.Once);
        }
        [Test]
        public void GroupCommentService_02_Update_02_Board_Unavalible_or_User_Unavalible_or_Comment_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .Returns(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .Returns(_selectedCommentList);

            _groupCommentService.Update(_groupComment, new Guid("00000000-0000-0000-0000-000000000001"));

            _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupCommentDB>()), Times.Never);
        }

        [Test]
        public void GroupCommentService_03_Delete_01_Delete_Your_Comment()
        {
            _selectedUserList.Add(_groupUserDB);
            _selectedBoardList.Add(_groupBoardDB);
            _selectedCommentList.Add(_groupCommentDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .Returns(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .Returns(_selectedCommentList);

            _groupCommentService.Delete(_groupComment, new Guid("00000000-0000-0000-0000-000000000002"));

            _groupRepositoryMock.Verify(m => m.Delete(It.IsAny<GroupCommentDB>()), Times.Once);
        }
        [Test]
        public void GroupCommentService_03_Delete_02_Board_Unavalible_or_User_Unavalible_or_Comment_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
                .Returns(_selectedUserList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
                .Returns(_selectedBoardList);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .Returns(_selectedCommentList);

            _groupCommentService.Delete(_groupComment, new Guid("00000000-0000-0000-0000-000000000001"));

            _groupRepositoryMock.Verify(m => m.Delete(It.IsAny<GroupCommentDB>()), Times.Never);
        }

        [Test]
        public void GroupCommentService_04_FindById_01_Get_Comment_By_Id()
        {
            _selectedCommentList.Add(_groupCommentDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .Returns(_selectedCommentList);

            var result = _groupCommentService.FindById(new Guid("00000000-0000-0000-0000-000000000001"));

            Assert.AreEqual(_groupComment.Id, result.Id);
            Assert.AreEqual(_groupComment.CommentatorId, result.CommentatorId);
            Assert.AreEqual(_groupComment.GroupBoardId, result.GroupBoardId);
        }
        [Test]
        public void GroupCommentService_04_FindById_02_GroupCommentDB_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .Returns(_selectedCommentList);

            var result = _groupCommentService.FindById(new Guid("00000000-0000-0000-0000-000000000001"));

            Assert.AreEqual(null, result);
        }

        [Test]
        public void GroupCommentService_04_FindtBoardComment_01_Get_Comments_By_BoardId()
        {
            _selectedCommentList.Add(_groupCommentDB);
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .Returns(_selectedCommentList);

            var result = _groupCommentService.FindtBoardComment(new Guid("00000000-0000-0000-0000-000000000001"))
                .FirstOrDefault();

            Assert.AreEqual(_groupComment.Id, result.Id);
            Assert.AreEqual(_groupComment.CommentatorId, result.CommentatorId);
            Assert.AreEqual(_groupComment.GroupBoardId, result.GroupBoardId);
        }
        [Test]
        public void GroupCommentService_04_FindtBoardComment_02_GroupCommentDB_Unavalible()
        {
            _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupCommentDB, Boolean>>()))
                .Returns(_selectedCommentList);

            var result = _groupCommentService.FindtBoardComment(new Guid("00000000-0000-0000-0000-000000000001"));

            Assert.AreEqual(null, result);
        }

        //[Test]
        //public void GroupBoardService_01_Create_02_Group_Unavalible_or_UserGroup_Unavalible_or_User_Dose_Not_Have_Access()
        //{
        //    _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
        //        .Returns(_selectedUserList);

        //    _groupBoardService.Create(_groupBoard);

        //    _groupRepositoryMock.Verify(m => m.Create(It.IsAny<GroupBoardDB>()), Times.Never);
        //}

        //[Test]
        //public void GroupBoardService_02_Update_01_Add_New_Information_In_GroupBoard()
        //{
        //    _selectedBoardList.Add(_groupBoardDB);
        //    _selectedUserList.Add(_groupUserDB);
        //    _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
        //        .Returns(_selectedBoardList);
        //    _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
        //        .Returns(_selectedUserList);

        //    _groupBoardService.Update(_groupBoard);

        //    _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupBoardDB>()), Times.Once);
        //}
        //[Test]
        //public void GroupBoardService_02_Update_02_GroupBoard_Unavalible_or_UserGroup_Unavalible_or_User_Dose_Not_Have_Access_or_Board_Unavalible_or_UserGroup_Unavalible()
        //{
        //    _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
        //        .Returns(_selectedBoardList);
        //    _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
        //        .Returns(_selectedUserList);

        //    _groupBoardService.Update(_groupBoard);

        //    _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupBoardDB>()), Times.Never);
        //}

        //[Test]
        //public void GroupBoardService_03_Delete_01_Remove_GroupBoard()
        //{
        //    _selectedBoardList.Add(_groupBoardDB);
        //    _selectedUserList.Add(_groupUserDB);
        //    _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
        //        .Returns(_selectedBoardList);
        //    _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
        //        .Returns(_selectedUserList);

        //    _groupBoardService.Update(_groupBoard);

        //    _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupBoardDB>()), Times.Once);
        //    _groupRepositoryMock.Verify(m => m.Delete(It.IsAny<GroupProductDB>()), Times.AtMostOnce);
        //}
        //[Test]
        //public void GroupBoardService_03_Delete_02_GroupBoard_Unavalible_or_UserGroup_Unavalible_or_User_Dose_Not_Have_Access_or_Board_Unavalible_or_UserGroup_Unavalible()
        //{
        //    _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupBoardDB, Boolean>>()))
        //        .Returns(_selectedBoardList);
        //    _groupRepositoryMock.Setup(m => m.Find(It.IsAny<Func<GroupUserDB, Boolean>>()))
        //        .Returns(_selectedUserList);

        //    _groupBoardService.Update(_groupBoard);

        //    _groupRepositoryMock.Verify(m => m.Update(It.IsAny<GroupBoardDB>()), Times.Never);
        //    _groupRepositoryMock.Verify(m => m.Delete(It.IsAny<GroupProductDB>()), Times.Never);
        //}

        //[Test]
        //public void GroupBoardService_04_FindById_01_Obtainment_Avalible_GroupBoard_By_Id()
        //{
        //    _selectedBoardList.Add(_groupBoardDB);
        //    _groupRepositoryMock.Setup(m => m.Find(
        //        It.IsAny<Func<GroupBoardDB, Boolean>>()))
        //        .Returns(_selectedBoardList);

        //    var result = _groupBoardService.FindById(new Guid("00000000-0000-0000-0000-000000000000"));
        //    Assert.AreEqual(_groupBoard.Id, result.Id);
        //    Assert.AreEqual(_groupBoard.Name, result.Name);
        //    Assert.AreEqual(_groupBoard.Information, result.Information);
        //    Assert.AreEqual(_groupBoard.CreatorId, result.CreatorId);
        //}
        //[Test]
        //public void GroupBoardService_04_FindById_02_Obtainment_Unavalible_GroupBoard_By_Id()
        //{
        //    _groupRepositoryMock.Setup(m => m.Find(
        //        It.IsAny<Func<GroupBoardDB, Boolean>>()))
        //        .Returns(_selectedBoardList);

        //    var result = _groupBoardService.FindById(new Guid("00000000-0000-0000-0000-000000000000"));
        //    Assert.AreEqual(null, result);
        //}
    }
}