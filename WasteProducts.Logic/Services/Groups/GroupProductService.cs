using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    public class GroupProductService : IGroupProductService
    {
        private IGroupRepository<GroupDB> _dataBase;
        private readonly IMapper _mapper;

        public GroupProductService(IGroupRepository<GroupDB> dataBase, IMapper mapper) 
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public void Create<Group>(Group item)
        {
            var result = _mapper.Map<GroupDB>(item);

            result.TimeCreate = DateTime.UtcNow;
            result.TimeDelete = null;
            result.Bool = true;

            _dataBase.Create(result);
        }

        public void Update<Group>(Group item)
        {
            var result = _mapper.Map<GroupDB>(item);

            _dataBase.Update(result);
        }

        public void Delete<Group>(Group item)
        {
            var result = _mapper.Map<GroupDB>(item);

            result.TimeDelete = DateTime.UtcNow;
            result.Bool = false;

            _dataBase.Update(result);
        }

        public void Delete<T>(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
