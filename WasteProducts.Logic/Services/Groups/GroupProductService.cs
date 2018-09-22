using AutoMapper;
using System;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;

namespace WasteProducts.Logic.Services.Groups
{
    public class GroupProductService : IGroupProductService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;

        public GroupProductService(IGroupRepository dataBase, IMapper mapper)
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public void Create(GroupProduct item, string userId, string groupId)
        {
            var result = _mapper.Map<GroupProductDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.UserId == userId
                && x.GroupId == groupId).FirstOrDefault();
            var modelBoard = _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId).FirstOrDefault();
            if (modelUser == null || modelBoard == null)
                return;

            result.Modified = DateTime.UtcNow;

            _dataBase.Create(result);
            _dataBase.Save();
        }

        public void Update(GroupProduct item, string userId, string groupId)
        {
            var result = _mapper.Map<GroupProductDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.UserId == userId
                && x.GroupId == groupId).FirstOrDefault();
            var modelBoard = _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId).FirstOrDefault();
            var model = _dataBase.Find<GroupProductDB>(
                x => x.Id == result.Id).FirstOrDefault();
            if (modelUser == null || modelBoard == null || model == null)
                return;

            model.ProductId = result.ProductId;
            model.Information = result.Information;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Delete(GroupProduct item, string userId, string groupId)
        {
            var result = _mapper.Map<GroupProductDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.UserId == userId
                && x.GroupId == groupId).FirstOrDefault();
            var modelBoard = _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId).FirstOrDefault();
            var model = _dataBase.Find<GroupProductDB>(
                x => x.Id == result.Id).FirstOrDefault();
            if (modelUser == null || modelBoard == null || model == null)
                return;

            _dataBase.Delete(model);
            _dataBase.Save();
        }

        public GroupProduct FindById(string id)
        {
            var model = _dataBase.Find<GroupProductDB>(
                x => x.Id == id).FirstOrDefault();
            if (model == null)
                return null;

            var result = _mapper.Map<GroupProduct>(model);

            return result;
        }
    }
}
