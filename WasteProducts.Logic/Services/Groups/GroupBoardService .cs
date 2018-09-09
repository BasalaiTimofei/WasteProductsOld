using AutoMapper;
using System;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
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

        public void Create<T>(T item) where T : class
        {
            var result = _mapper.Map<GroupBoardDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.RigtToCreateBoards == true
                && x.UserId == result.CreatorId
                && x.GroupId == result.GroupId);
            if (modelUser == null)
                return;

            result.IsNotDeleted = true;
            result.Created = DateTime.UtcNow;
            result.Deleted = null;
            result.Modified = DateTime.UtcNow;

            _dataBase.Create(result);
            _dataBase.Save();
        }

        public void Update<T>(T item) where T : class
        {
            var result = _mapper.Map<GroupBoardDB>(item);
            var model = _dataBase.Get<GroupBoardDB>(result.Id);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.RigtToCreateBoards == true
                && x.UserId == result.CreatorId
                && x.GroupId == result.GroupId);
            if (modelUser == null)
                return;

            model.Information = result.Information;
            model.Name = result.Name;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Delete<T>(T item) where T : class
        {
            var result = _mapper.Map<GroupBoardDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.RigtToCreateBoards == true
                && x.UserId == result.CreatorId
                && x.GroupId == result.GroupId);
            if (modelUser == null)
                return;

            var model = _dataBase.GetWithInclude<GroupBoardDB>(
                x => x.Id == result.GroupId,
                z => z.GroupProducts).First();
            if (model == null)
                return;

            model.IsNotDeleted = false;
            model.Deleted = DateTime.UtcNow;
            model.Modified = DateTime.UtcNow;
            foreach (var groupProduct in model.GroupProducts)
            {
                _dataBase.Delete<GroupProductDB>(groupProduct.Id);
            }

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public T FindById<T>(Guid id) where T : class
        {
            var model = _dataBase.Find<GroupBoardDB>(x => x.Id == id).First();
            var result = _mapper.Map<T>(model);

            return result;
        }
    }
}
