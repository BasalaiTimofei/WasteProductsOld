using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;

namespace WasteProducts.Logic.Services.Groups
{
    public class GroupCommentService : IGroupCommentService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;

        public GroupCommentService(IGroupRepository dataBase, IMapper mapper)
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public void Create<T>(T item, Guid groupId) where T : class
        {
            var result = _mapper.Map<GroupCommentDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.CommentatorId
                && x.GroupId == groupId);
            var modelBoard = _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId);
            if (modelUser == null && modelBoard == null)
                return;

            result.Modified = DateTime.UtcNow;

            _dataBase.Create(result);
            _dataBase.Save();
        }

        public void Update<T>(T item, Guid groupId) where T : class
        {
            var result = _mapper.Map<GroupCommentDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.CommentatorId
                && x.GroupId == groupId);
            var modelBoard = _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId);
            if (modelUser == null && modelBoard == null)
                return;

            var model = _dataBase.Find<GroupCommentDB>(
                x => x.Id == result.Id
                && x.CommentatorId == result.CommentatorId).First();
            if (model == null)
                return;

            model.Comment = result.Comment;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Delete<T>(T item, Guid groupId) where T : class
        {
            var result = _mapper.Map<GroupCommentDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.CommentatorId
                && x.GroupId == groupId);
            var modelBoard = _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId);
            if (modelUser == null && modelBoard == null)
                return;

            var model = _dataBase.Find<GroupCommentDB>(
                x => x.Id == result.Id
                && x.CommentatorId == result.CommentatorId).First();
            if (model == null)
                return;

            _dataBase.Delete<GroupProductDB>(model.Id);

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public T FindById<T>(Guid id) where T : class
        {
            var model = _dataBase.Find<GroupCommentDB>(
                x => x.Id == id).First();
            var result = _mapper.Map<T>(model);

            return result;
        }

        public IEnumerable<T> FindtBoardComment<T>(Guid boardId) where T : class
        {
            var model = _dataBase.Find<GroupCommentDB>(
                x => x.GroupBoardId == boardId);
            var result = _mapper.Map<IEnumerable<T>>(model);

            return result;
        }
    }
}
