using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    public class GroupProductService : IGroupProductService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;

        public GroupProductService(IGroupRepository dataBase, IMapper mapper)
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public void Create<T>(T item)
        {
            var result = _mapper.Map<GroupProductDB>(item);

            _dataBase.Create(result);
        }

        public void Update<T>(T item)
        {
            var result = _mapper.Map<GroupProductDB>(item);
            var model = _dataBase.Get<GroupProductDB>(result.Id);

            model.ProductId = result.ProductId;
            model.Information = result.Information;

            _dataBase.Update(model);
        }

        public void Delete<T>(T item)
        {
            var result = _mapper.Map<GroupProductDB>(item);

            _dataBase.Delete<GroupProductDB>(result.Id);
        }
    }
}
