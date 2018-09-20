using AutoMapper;
using System;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;

namespace WasteProducts.Logic.Services.Groups
{
    public class GroupBoardService : IGroupBoardService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;

        public GroupBoardService(IGroupRepository dataBase, IMapper mapper)
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public void Create(GroupBoard item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.RightToCreateBoards == true
                && x.UserId == result.CreatorId
                && x.GroupId == result.GroupId).FirstOrDefault();
            if (modelUser == null)
                return;

            result.IsNotDeleted = true;
            result.Created = DateTime.UtcNow;
            result.Deleted = null;
            result.Modified = DateTime.UtcNow;

            _dataBase.Create(result);
            _dataBase.Save();
        }

        public void Update(GroupBoard item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);
            var model = _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.Id).FirstOrDefault();

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.RightToCreateBoards == true
                && x.UserId == result.CreatorId
                && x.GroupId == result.GroupId).FirstOrDefault();
            if (modelUser == null || model == null)
                return;

            model.Information = result.Information;
            model.Name = result.Name;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Delete(GroupBoard item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);

            var model = _dataBase.GetWithInclude<GroupBoardDB>(
                x => x.Id == result.Id
                &&x.GroupId == result.GroupId,
                z => z.GroupProducts).FirstOrDefault();
            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.RightToCreateBoards == true
                && x.UserId == result.CreatorId
                && x.GroupId == result.GroupId).FirstOrDefault();
            if (model == null || modelUser == null)
                return;

            model.IsNotDeleted = false;
            model.Deleted = DateTime.UtcNow;
            model.Modified = DateTime.UtcNow;

            _dataBase.DeleteAll(model.GroupProducts);
            _dataBase.Update(model);
            _dataBase.Save();
        }

        public GroupBoard FindById(string id)
        {
            var model = _dataBase.Find<GroupBoardDB>(
                x => x.Id == id).FirstOrDefault();
            if (model == null)
                return null;

            var result = _mapper.Map<GroupBoard>(model);

            return result;
        }
    }
}
