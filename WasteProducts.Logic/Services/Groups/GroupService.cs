using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;

namespace WasteProducts.Logic.Services.Groups
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _dataBase;
        private readonly IMapper _mapper;
        private bool _disposed;

        public GroupService(IGroupRepository dataBase, IMapper mapper)
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public async Task<Group> Create(Group item)
        {
            var result = _mapper.Map<GroupDB>(item);
            result.GroupBoards = null;
            result.GroupUsers = null;

            var searchResult = await _dataBase.Find<GroupDB>(x => x.AdminId == result.AdminId).ConfigureAwait(false);

            var model = searchResult.FirstOrDefault();
            if (model != null)
            {
                throw new ValidationException("Group already exists");
            }

            result.Id = Guid.NewGuid().ToString();
            result.Created = DateTime.UtcNow;
            result.Deleted = null;
            result.IsNotDeleted = true;

            result.GroupUsers = new List<GroupUserDB>
            {
                new GroupUserDB
                {
                    GroupId = result.Id,
                    UserId = result.AdminId,
                    IsConfirmed = true,
                    RightToCreateBoards = true,
                    Created = DateTime.UtcNow,
                }
            };

            _dataBase.Create(result);
            await _dataBase.Save().ConfigureAwait(false);

            return _mapper.Map<Group>(result);
        }

        public async Task Update(Group item)
        {
            var result = _mapper.Map<GroupDB>(item);

            var searchResult = await _dataBase.Find<GroupDB>(x => x.Id == result.Id && x.IsNotDeleted)
                .ConfigureAwait(false);

            var model = searchResult.FirstOrDefault();
            if (model == null)
            {
                throw new ValidationException("Group not found");
            }

            model.Information = result.Information;
            model.Name = result.Name;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);

            await _dataBase.Save().ConfigureAwait(false);
        }

        public async Task Delete(string groupId)
        {
            var searchResult = await _dataBase.GetWithInclude<GroupDB>(x => x.Id == groupId && x.IsNotDeleted,
                y => y.GroupBoards.Select(z => z.GroupProducts),
                m => m.GroupUsers
            ).ConfigureAwait(false);

            var model = searchResult.FirstOrDefault();
            if (model == null)
            {
                throw new ValidationException("Group not found");
            }

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

            await _dataBase.Save().ConfigureAwait(false);
        }

        public Task<Group> FindById(string groupId)
        {
            return FindBy(g => g.Id == groupId).ContinueWith(r=> r.Result.FirstOrDefault());
        }

        public Task<IEnumerable<Group>> FindByAdmin(string userId)
        {
            return FindBy(g => g.AdminId == userId);
        }

        public Task<IEnumerable<Group>> FindByName(string name)
        {
            return FindBy(g => g.Name == name);
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

        private Task<IEnumerable<Group>> FindBy(Func<GroupDB, bool> predicate)
        {
            return _dataBase.GetWithInclude(
                predicate,
                y => y.GroupBoards.Select(z => z.GroupProducts),
                k => k.GroupBoards.Select(e => e.GroupComments),
                m => m.GroupUsers).ContinueWith(result => _mapper.Map<IEnumerable<Group>>(result));
        }

        ~GroupService()
        {
            Dispose();
        }
    }
}
