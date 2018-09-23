using AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
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

        public string Create(GroupProduct item, string userId, string groupId)
        {
            var result = _mapper.Map<GroupProductDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.UserId == userId
                && x.GroupId == groupId).FirstOrDefault();
            if (modelUser == null)
                throw new ValidationException("User not found");

            var modelBoard = _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId).FirstOrDefault();
            if (modelBoard == null)
                throw new ValidationException("Board not found");

            result.Modified = DateTime.UtcNow;

            _dataBase.Create(result);
            _dataBase.Save();
            return result.Id.ToString();
        }

        public void Update(GroupProduct item, string userId, string groupId)
        {
            var result = _mapper.Map<GroupProductDB>(item);

            var modelUser = _dataBase.Find<GroupUserDB>(
                x => x.UserId == userId
                && x.GroupId == groupId).FirstOrDefault();
            if (modelUser == null)
                throw new ValidationException("User not found");

            var modelBoard = _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId).FirstOrDefault();
            if (modelBoard == null)
                throw new ValidationException("Board not found");

            var model = _dataBase.Find<GroupProductDB>(
                x => x.Id == result.Id).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Product not found");

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
            if (modelUser == null)
                throw new ValidationException("User not found");

            var modelBoard = _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId
                && x.GroupId == groupId).FirstOrDefault();
            if (modelBoard == null)
                throw new ValidationException("Board not found");

            var model = _dataBase.Find<GroupProductDB>(
                x => x.Id == result.Id).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Product not found");

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
