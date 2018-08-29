using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
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

        public void Create<T>(T item)
        {
            var result = _mapper.Map<GroupDB>(item);

            result.TimeCreate = DateTime.UtcNow;
            result.TimeDelete = null;
            result.Bool = true;

            result.GroupUserDBs.Add(new GroupUserDB
            {
                Bool = true,
                UserId = result.Admin,
                GroupUserInviteTimeDBs = new List<GroupUserInviteTimeDB>
                {
                    new GroupUserInviteTimeDB
                    {
                        TimeEntry = DateTime.UtcNow,
                        TimeExit = null,
                        Invite = null
                    }
                }
            });

            _dataBase.Create(result);
            _dataBase.Save();
        }

        public void Update<T>(T item)
        {
            var result = _mapper.Map<GroupDB>(item);
            var model = _dataBase.Get<GroupDB>(result.Id);

            model.Information = result.Information;
            model.Name = result.Name;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Delete<T>(T item)
        {
            var result = _mapper.Map<GroupDB>(item);

            var models = _dataBase.GetWithInclude<GroupDB>(
                x => x.Id == result.Id,
                y => y.GroupBoardDBs.Select(z => z.GroupProductDBs),
                m => m.GroupUserDBs.Select(t => t.GroupUserInviteTimeDBs));

            foreach (var model in models)
            {
                model.Bool = false;
                model.TimeDelete = DateTime.UtcNow;
                foreach (var groupBoard in model.GroupBoardDBs)
                {
                    groupBoard.Bool = false;
                    groupBoard.TimeDelete = DateTime.UtcNow;
                    foreach (var groupProduct in groupBoard.GroupProductDBs)
                    {
                        _dataBase.Delete<GroupProductDB>(groupProduct.Id);
                    }
                }
                foreach (var groupUser in model.GroupUserDBs)
                {
                    groupUser.Bool = false;
                    foreach (var groupUserInviteTime in groupUser.GroupUserInviteTimeDBs)
                    {
                        groupUserInviteTime.TimeExit = DateTime.UtcNow;
                    }
                }
            }
            _dataBase.Update(models);
            _dataBase.Save();
        }
    }
}
