using AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<string> Create(GroupBoard item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);
            result.GroupProducts = null;
            result.GroupComments = null;

            var modelUser = (await _dataBase.Find<GroupUserDB>(
                x => x.RightToCreateBoards == true
                && x.UserId == result.CreatorId
                && x.GroupId == result.GroupId).ConfigureAwait(false)).FirstOrDefault();
            if (modelUser == null)
                throw new ValidationException("User not found");

            result.Id = Guid.NewGuid().ToString();
            result.IsNotDeleted = true;
            result.Created = DateTime.UtcNow;
            result.Deleted = null;
            result.Modified = DateTime.UtcNow;

            _dataBase.Create(result);
            await _dataBase.Save();
            return result.Id;
        }

        public async Task Update(GroupBoard item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);

            var model = (await _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.Id).ConfigureAwait(false)).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Board not found");

            model.Information = result.Information;
            model.Name = result.Name;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            await _dataBase.Save();
        }

        public async Task Delete(string boardId)
        {
            var model = (await _dataBase.GetWithInclude<GroupBoardDB>(
                x => x.Id == boardId,
                z => z.GroupProducts).ConfigureAwait(false)).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Board not found");

            model.IsNotDeleted = false;
            model.Deleted = DateTime.UtcNow;
            model.Modified = DateTime.UtcNow;

            _dataBase.DeleteAll(model.GroupProducts);
            _dataBase.Update(model);
            await _dataBase.Save();
        }

        public async Task<GroupBoard> FindById(string boardId)
        {
            var model =(await _dataBase.Find<GroupBoardDB>(
                x => x.Id == boardId)).FirstOrDefault();
            if (model == null)
                return null;

            var result = _mapper.Map<GroupBoard>(model);

            return result;
        }
    }
}
