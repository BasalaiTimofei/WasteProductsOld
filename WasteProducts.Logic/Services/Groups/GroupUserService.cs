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
    public class GroupUserService : IGroupUserService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;

        public GroupUserService(IGroupRepository dataBase, IMapper mapper)
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public void SendInvite(GroupUser item, string adminId)
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var modelGroupDB = _dataBase.Find<GroupDB>(
                x => x.Id == result.GroupId
                && x.AdminId == adminId
                && x.IsNotDeleted == true).FirstOrDefault();
            if (modelGroupDB == null)
                return;

            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.GroupId == result.GroupId).FirstOrDefault();

            result.IsInvited = 0;
            result.Modified = DateTime.UtcNow;
            if (model == null)
            {
                _dataBase.Create(result);
            }
            else if (model.IsInvited == 2)
            {
                _dataBase.Update(result);
            }
            else
            {
                _dataBase.Dispose();
                return;
            }
            _dataBase.Save();
        }

        public void DismissUser(GroupUser item, string adminId)
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var modelGroupDB = _dataBase.Find<GroupDB>(
                x => x.Id == result.GroupId
                && x.AdminId == adminId
                && x.IsNotDeleted == true).FirstOrDefault();
            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.IsInvited == 1
                && x.GroupId == result.GroupId).FirstOrDefault();
            if (modelGroupDB == null || model == null)
                return;

            model.IsInvited = 2;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Enter(GroupUser item, string adminId)
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var modelGroupDB = _dataBase.Find<GroupDB>(
                x => x.Id == result.GroupId
                && x.AdminId == adminId
                && x.IsNotDeleted == true).FirstOrDefault();
            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.IsInvited == 0
                && x.GroupId == result.GroupId).FirstOrDefault();
            if (modelGroupDB == null || model == null)
                return;

            model.IsInvited = 1;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Leave(GroupUser item, string adminId)
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var modelGroupDB = _dataBase.Find<GroupDB>(
                x => x.Id == result.GroupId
                && x.AdminId == adminId
                && x.IsNotDeleted == true).FirstOrDefault();
            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.IsInvited == 1
                && x.GroupId == result.GroupId).FirstOrDefault();
            if (modelGroupDB == null || model == null)
                return;

            model.IsInvited = 2;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void GetEntitle(GroupUser item, string adminId)
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var modelGroupDB = _dataBase.Find<GroupDB>(
                x => x.Id == result.GroupId
                && x.AdminId == adminId).FirstOrDefault();
            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.GroupId == result.GroupId).FirstOrDefault();
            if (modelGroupDB == null || model == null)
                return;

            if(result.RightToCreateBoards)
            {
                model.RightToCreateBoards = false;
            }
            else
            {
                model.RightToCreateBoards = true;
            }
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public IEnumerable<GroupUser> FindReceivedInvites(string userId)
        {
            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == userId 
                && x.IsInvited == 0);
            if (model == null)
                return null;

            var result = _mapper.Map<IEnumerable<GroupUser>>(model);

            return result;
        }

        public IEnumerable<GroupUser> FindGroupsById(string userId)
        {
            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == userId
                && x.IsInvited == 1);
            if (model == null)
                return null;

            var result = _mapper.Map<IEnumerable<GroupUser>>(model);

            return result;
        }

        public IEnumerable<GroupUser> FindUsersByGroupId(Guid groupId)
        {
            var model = _dataBase.Find<GroupUserDB>(
                x => x.GroupId == groupId);
            if (model == null)
                return null;

            var result = _mapper.Map<IEnumerable<GroupUser>>(model);

            return result;
        }
    }
}
