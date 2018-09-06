using AutoMapper;
using System;
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

        public void Create<T>(T item, string userId) where T : class
        {
            var result = _mapper.Map<GroupCommentDB>(item);

            result.Modified = DateTime.UtcNow;

            _dataBase.Create(result);
            _dataBase.Save();
        }

        public void Update<T>(T item, string userId) where T : class
        {
            var result = _mapper.Map<GroupCommentDB>(item);
            var model = _dataBase.Find<GroupCommentDB>(
                x => x.Id == result.Id
                && x.CommentatorId == userId).First();

            model.Comment = result.Comment;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Delete<T>(T item, string userId) where T : class
        {
            var result = _mapper.Map<GroupCommentDB>(item);
            var model = _dataBase.Find<GroupCommentDB>(
                x => x.Id == result.Id
                && x.CommentatorId == userId).First();

            _dataBase.Delete<GroupProductDB>(model.Id);

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public T FindById<T>(Guid id) where T : class
        {
            var model = _dataBase.Find<GroupCommentDB>(x => x.Id == id).First();
            var result = _mapper.Map<T>(model);

            return result;
        }

        public T FindAll<T>(Guid id) where T : class
        {
            var model = _dataBase.GetWithInclude<GroupCommentDB>(x => x.GroupBoardId == id).First();
            var result = _mapper.Map<T>(model);

            return result;
        }
    }
}
