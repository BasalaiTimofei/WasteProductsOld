using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<string> Create(GroupComment item, string groupId)
        {
            var result = _mapper.Map<GroupCommentDB>(item);

            var modelUser = (await _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.CommentatorId
                && x.GroupId == groupId)).FirstOrDefault();
            if (modelUser == null)
                throw new ValidationException("User not found");

            var modelBoard = (await _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId)).FirstOrDefault();
            if (modelBoard == null)
                throw new ValidationException("Board not found");

            result.Modified = DateTime.UtcNow;

            _dataBase.Create(result);
            await _dataBase.Save();
            return result.Id;
        }

        public async Task Update(GroupComment item, string groupId)
        {
            var result = _mapper.Map<GroupCommentDB>(item);

            var modelUser = (await _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.CommentatorId
                && x.GroupId == groupId)).FirstOrDefault();
            if (modelUser == null)
                throw new ValidationException("User not found");

            var modelBoard = (await _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId)).FirstOrDefault();
            if (modelBoard == null)
                throw new ValidationException("Board not found");

            var model = (await _dataBase.Find<GroupCommentDB>(
                x => x.Id == result.Id
                && x.CommentatorId == result.CommentatorId)).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Comment not found");

            model.Comment = result.Comment;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            await _dataBase.Save();
        }

        public async Task Delete(GroupComment item, string groupId)
        {
            var result = _mapper.Map<GroupCommentDB>(item);

            var modelUser = (await _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.CommentatorId
                && x.GroupId == groupId)).FirstOrDefault();
            if (modelUser == null)
                throw new ValidationException("User not found");

            var modelBoard = (await _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId)).FirstOrDefault();
            if (modelBoard == null)
                throw new ValidationException("Board not found");

            var model = (await _dataBase.Find<GroupCommentDB>(
                x => x.Id == result.Id
                && x.CommentatorId == result.CommentatorId)).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Comment not found");

            _dataBase.Delete(model);
            await _dataBase.Save();
        }

        public async Task<GroupComment> FindById(string id)
        {
            var model = (await _dataBase.Find<GroupCommentDB>(
                x => x.Id == id)).FirstOrDefault();
            if (model == null)
                return null;

            var result = _mapper.Map<GroupComment>(model);

            return result;
        }

        public async Task<IEnumerable<GroupComment>> FindtBoardComment(string boardId)
        {
            var model = (await _dataBase.Find<GroupCommentDB>(
                x => x.GroupBoardId == boardId));
            if (model.FirstOrDefault() == null)
                return null;

            var result = _mapper.Map<IEnumerable<GroupComment>>(model);

            return result;
        }
    }
}
