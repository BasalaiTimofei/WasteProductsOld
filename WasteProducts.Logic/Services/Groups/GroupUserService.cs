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

        public void SendInvite<T>(T item, string adminId) where T : class
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var modelGroupDB = _dataBase.Find<GroupDB>(
                x => x.Id == result.GroupId
                && x.AdminId == adminId);
            if (modelGroupDB != null)
                return;

            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.GroupId == result.GroupId).First();

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

        public void DismissUser<T>(T item, string adminId) where T : class
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var modelGroupDB = _dataBase.Find<GroupDB>(
                x => x.Id == result.GroupId
                && x.AdminId == adminId);
            if (modelGroupDB != null)
                return;

            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.IsInvited == 1
                && x.GroupId == result.GroupId).First();
            if (model == null)
                return;

            model.IsInvited = 2;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Enter<T>(T item) where T : class
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.IsInvited == 0
                && x.GroupId == result.GroupId).First();
            if (model == null)
                return;

            model.IsInvited = 1;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Leave<T>(T item) where T : class
        {
            var result = _mapper.Map<GroupUserDB>(item);

            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == result.UserId
                && x.IsInvited == 1
                && x.GroupId == result.GroupId).First();
            if (model == null)
                return;

            model.IsInvited = 2;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public IEnumerable<T> FindReceivedInvites<T>(string userId) where T : class
        {
            var model = _dataBase.Find<GroupUserDB>(
                x => x.UserId == userId 
                && x.IsInvited == 0);
            var result = _mapper.Map<IEnumerable<T>>(model);

            return result;
        }

        public IEnumerable<T> FindUsersByGroupId<T>(Guid groupId) where T : class
        {
            var model = _dataBase.Find<GroupUserDB>(
                x => x.GroupId == groupId);
            var result = _mapper.Map<IEnumerable<T>>(model);

            return result;
        }
    }
}
