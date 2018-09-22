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

        public void Invite(GroupUser item, string adminId)
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var modelGroupDB = _dataBase.Find<GroupDB>(
                x => x.Id == result.GroupId
                && x.AdminId == adminId
                && x.IsNotDeleted == true).FirstOrDefault();
            if (modelGroupDB == null)
                throw new ValidationException("Group not create");

            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.GroupId == result.GroupId).FirstOrDefault();

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
            _dataBase.Save();
        }

        public async void Kick(GroupUser groupUser, string adminId)
        {
            var group = _dataBase.Find<GroupDB>(
                x => x.Id == groupUser.GroupId
                && x.AdminId == adminId
                && x.IsNotDeleted == true).FirstOrDefault();

            if (group == null)
                throw new ValidationException("Group not create");

            var groupUserDB = _dataBase.Find<GroupUserDB>(x =>
                x.UserId == groupUser.UserId &&
                x.GroupId == groupUser.GroupId).FirstOrDefault();

            if (groupUser == null)
                throw new ValidationException("User not found");

            await _dataBase.DeleteUserFromGroupAsync(groupUser.GroupId, groupUser.UserId);
            _dataBase.Save();
        }

        public void GiveRightToCreateBoards(GroupUser item, string adminId)
        {
            var modelGroupDB = _dataBase.Find<GroupDB>(
                x => x.Id == item.GroupId
                && x.AdminId == adminId).FirstOrDefault();

            if (modelGroupDB == null)
              throw new ValidationException("Group not create");

            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == item.UserId
                && x.GroupId == item.GroupId).FirstOrDefault();

            if (model == null || model.RightToCreateBoards)
              throw new ValidationException("User not found");

            model.RightToCreateBoards = true;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void TakeAwayRightToCreateBoards(GroupUser item, string adminId)
        {
            var modelGroupDB = _dataBase.Find<GroupDB>(
                x => x.Id == item.GroupId
                && x.AdminId == adminId).FirstOrDefault();

            if (modelGroupDB == null)
              throw new ValidationException("User not found");

            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == item.UserId
                && x.GroupId == item.GroupId).FirstOrDefault();

            if (model == null || !model.RightToCreateBoards)
              throw new ValidationException("User not found");

            model.RightToCreateBoards = false;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
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
