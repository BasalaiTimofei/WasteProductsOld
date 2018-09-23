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
    public class GroupService : IGroupService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;
        private bool _disposed;

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
                throw new ValidationException("Group already exists");

            result.Id = Guid.NewGuid().ToString();
            result.Created = DateTime.UtcNow;
            result.Deleted = null;
            result.IsNotDeleted = true;

            result.GroupUsers.Add(new GroupUserDB
            {
                GroupId = result.Id,
                UserId = result.AdminId,
                IsConfirmed = true,
                RightToCreateBoards = true,
                Created = DateTime.UtcNow,
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
                && x.AdminId == result.AdminId
                && x.IsNotDeleted == true).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Group not found");

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
                b=>b.IsNotDeleted == true,
                y => y.GroupBoards.Select(z => z.GroupProducts),
                m => m.GroupUsers).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Group not found");

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
            _dataBase.DeleteAll(model.GroupUsers);
            _dataBase.Update(model);
            _dataBase.Save();
        }

        public Group FindById(string groupId)
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

        public Group FindByName(string name)
        {
            var model = _dataBase.GetWithInclude<GroupDB>(
                    x => x.Name == name,
                    y => y.GroupBoards.Select(z => z.GroupProducts),
                    k => k.GroupBoards.Select(e => e.GroupComments),
                    m => m.GroupUsers).FirstOrDefault();
            if (model == null)
                return null;

            var result = _mapper.Map<Group>(model);

            return result;
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

        ~GroupService()
        {
            Dispose();
        }
    }
}
