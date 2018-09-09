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
    public class GroupService : IGroupService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository dataBase, IMapper mapper)
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public void Create<T>(T item) where T : class
        {
            var result = _mapper.Map<GroupDB>(item);

            var model = _dataBase.Find<GroupDB>(x => x.AdminId == result.AdminId);
            if (model != null)
                return;

            result.Created = DateTime.UtcNow;
            result.Deleted = null;
            result.IsNotDeleted = true;
            result.Modified = DateTime.UtcNow;

            result.GroupUsers.Add(new GroupUserDB
            {
                IsInvited = 1,
                RigtToCreateBoards = true,
                Modified = DateTime.UtcNow,
            });

            _dataBase.Create(result);
            _dataBase.Save();
        }

        public void Update<T>(T item) where T : class
        {
            var result = _mapper.Map<GroupDB>(item);

            var model = _dataBase.Find<GroupDB>(
                x => x.Id == result.Id
                &&x.AdminId == result.AdminId).First();
            if (model != null)
                return;

            model.Information = result.Information;
            model.Name = result.Name;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Delete<T>(T item) where T : class
        {
            var result = _mapper.Map<GroupDB>(item);

            var model = _dataBase.GetWithInclude<GroupDB>(
                x => x.Id == result.Id, 
                a => a.AdminId == result.AdminId,
                y => y.GroupBoards.Select(z => z.GroupProducts),
                m => m.GroupUsers).First();
            if (model == null)
                return;

            model.IsNotDeleted = false;
            model.Deleted = DateTime.UtcNow;
            model.Modified = DateTime.UtcNow;
            foreach (var groupBoard in model.GroupBoards)
            {
                groupBoard.IsNotDeleted = false;
                groupBoard.Deleted = DateTime.UtcNow;
                groupBoard.Modified = DateTime.UtcNow;
                foreach (var groupProduct in groupBoard.GroupProducts)
                {
                    _dataBase.Delete<GroupProductDB>(groupProduct.Id);
                }
            }
            foreach (var groupUser in model.GroupUsers)
            {
                groupUser.IsInvited = 2;
                groupUser.Modified = DateTime.UtcNow;
            }
            _dataBase.Update(model);
            _dataBase.Save();
        }

        public T FindById<T>(Guid groupId) where T : class
        {
            var model = _dataBase.GetWithInclude<GroupDB>(
                    x => x.Id == groupId,
                    y => y.GroupBoards.Select(z => z.GroupProducts),
                    k => k.GroupBoards.Select(e => e.GroupComments),
                    m => m.GroupUsers).First();

            var result = _mapper.Map<T>(model);

            return result;
        }

        public T FindByAdmin<T>(string userId) where T : class
        {
            var model = _dataBase.GetWithInclude<GroupDB>(
                    x => x.AdminId == userId,
                    y => y.GroupBoards.Select(z => z.GroupProducts),
                    k => k.GroupBoards.Select(e => e.GroupComments),
                    m => m.GroupUsers).First();
            var result = _mapper.Map<T>(model);

            return result;
        }

        public IEnumerable<T> GetIds<T>(string userId) where T : class
        {
            var model = _dataBase.Find<GroupUserDB>(
                    x => x.UserId == userId && x.IsInvited == 1)
                    .Select(y=> new Group { Id=y.GroupId });
            var result = _mapper.Map<IEnumerable<T>>(model);

            return result;
        }

    }
}
