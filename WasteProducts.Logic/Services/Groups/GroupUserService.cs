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
    public class GroupUserService : IGroupUserService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;
        private bool _disposed;

        public GroupUserService(IGroupRepository dataBase, IMapper mapper)
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public async Task Invite(GroupUser item, string adminId)
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var modelGroupDB = (await _dataBase.Find<GroupDB>(
                x => x.Id == result.GroupId
                && x.AdminId == adminId
                && x.IsNotDeleted == true)).FirstOrDefault();
            if (modelGroupDB == null)
                throw new ValidationException("Group not found");

            var model = (await _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.GroupId == result.GroupId)).FirstOrDefault();

            result.IsConfirmed = false;
            result.Created = DateTime.UtcNow;
            if (model == null)
            {
                _dataBase.Create(result);
            }
            else
            {
                _dataBase.Dispose();
                return;
            }
            await _dataBase.Save();
        }

        public async Task Kick(GroupUser item, string adminId)
        {
            var modelGroupDB = (await _dataBase.Find<GroupDB>(
                x => x.Id == item.GroupId
                && x.AdminId == adminId
                && x.IsNotDeleted == true)).FirstOrDefault();

            if (modelGroupDB == null)
                throw new ValidationException("Group not found");

            var model = (await _dataBase.Find<GroupUserDB>(
                x => x.UserId == item.UserId
                && x.GroupId == item.GroupId)).FirstOrDefault();

            if (model == null)
                throw new ValidationException("User not found");

            _dataBase.Delete(model);
            await _dataBase.Save();
        }

        public async Task GiveRightToCreateBoards(GroupUser item, string adminId)
        {
            var modelGroupDB = (await _dataBase.Find<GroupDB>(
                x => x.Id == item.GroupId
                && x.AdminId == adminId
                && x.IsNotDeleted == true)).FirstOrDefault();

            if (modelGroupDB == null)
              throw new ValidationException("Group not found");

            var model = (await _dataBase.Find<GroupUserDB>(
                x => x.UserId == item.UserId
                && x.GroupId == item.GroupId)).FirstOrDefault();

            if (model == null || model.RightToCreateBoards)
              throw new ValidationException("User not found");

            model.RightToCreateBoards = true;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            await _dataBase.Save();
        }

        public async Task TakeAwayRightToCreateBoards(GroupUser item, string adminId)
        {
            var modelGroupDB = (await _dataBase.Find<GroupDB>(
                x => x.Id == item.GroupId
                && x.AdminId == adminId
                && x.IsNotDeleted == true)).FirstOrDefault();

            if (modelGroupDB == null)
                throw new ValidationException("Group not found");

            var model = (await _dataBase.Find<GroupUserDB>(
                x => x.UserId == item.UserId
                && x.GroupId == item.GroupId)).FirstOrDefault();

            if (model == null || !model.RightToCreateBoards)
                throw new ValidationException("User not found");

            model.RightToCreateBoards = false;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            await _dataBase.Save();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _dataBase.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~GroupUserService()
        {
            Dispose();
        }
    }
}
