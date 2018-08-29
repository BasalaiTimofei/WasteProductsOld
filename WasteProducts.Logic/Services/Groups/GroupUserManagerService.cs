using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Validators;

namespace WasteProducts.Logic.Services
{
    public class GroupUserManagerService : IGroupUserManagerService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;

        public GroupUserManagerService(IGroupRepository dataBase, IMapper mapper) 
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public void Add<T>(T item)
        {
            var result = _mapper.Map<GroupUserDB>(item);

            //_dataBase.Create<GroupUserDB>(result.GroupUserInviteTimeDBs.);
            result.Bool = true;

            _dataBase.Create(result);
        }

        public void Delete<T>(T item)
        {

        }
    }
}
