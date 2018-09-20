using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;

namespace WasteProducts.Logic.Services.Groups
{
    public class GroupService : IGroupService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository dataBase, IMapper mapper)
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public string Create(Group item)
        {
            var result = _mapper.Map<GroupDB>(item);

            var model = _dataBase.Find<GroupDB>(
                x => x.AdminId == result.AdminId).FirstOrDefault();
            if (model != null)
                throw new ValidationException("Group created");

            result.Created = DateTime.UtcNow;
            result.Deleted = null;
            result.IsNotDeleted = true;
            result.Modified = DateTime.UtcNow;

            result.GroupUsers.Add(new GroupUserDB
            {
                IsInvited = 1,
                RightToCreateBoards = true,
                Modified = DateTime.UtcNow,
            });

            _dataBase.Create(result);
            _dataBase.Save();
            return result.Id.ToString();
        }

        public void Update(Group item)
        {
            var result = _mapper.Map<GroupDB>(item);

            var model = _dataBase.Find<GroupDB>(
                x => x.Id == result.Id
                && x.AdminId == result.AdminId).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Group did not created");

            model.Information = result.Information;
            model.Name = result.Name;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Delete(Group item)
        {
            var result = _mapper.Map<GroupDB>(item);

            var model = _dataBase.GetWithInclude<GroupDB>(
                x => x.Id == result.Id,
                a => a.AdminId == result.AdminId,
                y => y.GroupBoards.Select(z => z.GroupProducts),
                m => m.GroupUsers).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Group did not created");

            model.IsNotDeleted = false;
            model.Deleted = DateTime.UtcNow;
            model.Modified = DateTime.UtcNow;
            foreach (var groupBoard in model.GroupBoards)
            {
                groupBoard.IsNotDeleted = false;
                groupBoard.Deleted = DateTime.UtcNow;
                groupBoard.Modified = DateTime.UtcNow;
                _dataBase.DeleteAll(groupBoard.GroupProducts);
            }
            foreach (var groupUser in model.GroupUsers)
            {
                groupUser.IsInvited = 2;
                groupUser.Modified = DateTime.UtcNow;
            }
            _dataBase.Update(model);
            _dataBase.Save();
        }

        public Group FindById(Guid groupId)
        {
            var model = _dataBase.GetWithInclude<GroupDB>(
                    x => x.Id == groupId,
                    y => y.GroupBoards.Select(z => z.GroupProducts),
                    k => k.GroupBoards.Select(e => e.GroupComments),
                    m => m.GroupUsers).FirstOrDefault();
            if (model == null)
                return null;

            var result = _mapper.Map<Group>(model);

            return result;
        }

        public Group FindByAdmin(string userId)
        {
            var model = _dataBase.GetWithInclude<GroupDB>(
                    x => x.AdminId == userId,
                    y => y.GroupBoards.Select(z => z.GroupProducts),
                    k => k.GroupBoards.Select(e => e.GroupComments),
                    m => m.GroupUsers).FirstOrDefault();
            if (model == null)
                return null;

            var result = _mapper.Map<Group>(model);

            return result;
        }
    }
}
