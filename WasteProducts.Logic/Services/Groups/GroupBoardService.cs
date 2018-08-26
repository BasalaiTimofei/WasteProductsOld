using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    public class GroupBoardService : IGroupBoardService
    {
        private IGroupRepository<GroupBoardDB> _dataBase;
        private readonly IMapper _mapper;

        public GroupBoardService(IGroupRepository<GroupBoardDB> dataBase, IMapper mapper) 
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public void Create<GroupBoard>(GroupBoard item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);

            result.TimeCreate = DateTime.UtcNow;
            result.TimeDelete = null;
            result.Bool = true;

            _dataBase.Create(result);
        }

        public void Update<GroupBoard>(GroupBoard item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);

            _dataBase.Update(result);
        }

        public void Delete<GroupBoard>(GroupBoard item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);

            result.TimeDelete = DateTime.UtcNow;
            result.Bool = false;

            _dataBase.Update(result);
        }
    }
}
