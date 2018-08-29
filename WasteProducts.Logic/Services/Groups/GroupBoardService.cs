using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    public class GroupBoardService : IGroupBoardService
    {
        private IGroupRepository _dataBase;
        private readonly IMapper _mapper;

        public GroupBoardService(IGroupRepository dataBase, IMapper mapper)
        {
            _dataBase = dataBase;
            _mapper = mapper;
        }

        public void Create<T>(T item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);

            result.TimeCreate = DateTime.UtcNow;
            result.TimeDelete = null;
            result.Bool = true;

            _dataBase.Create(result);
        }

        public void Update<T>(T item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);
            var model = _dataBase.Get<GroupBoardDB>(result.Id);

            model.Information = result.Information;
            model.Name = result.Name;

            _dataBase.Update(model);
        }

        public void Delete<T>(T item)
        {
            var result = _mapper.Map<GroupBoardDB>(item);
            var models = _dataBase.Find<GroupBoardDB>(x => x.Id == result.Id);

            foreach (var model in models)
            {
                model.Bool = false;
                model.TimeDelete = DateTime.UtcNow;
                foreach (var groupProduct in model.GroupProductDBs)
                {
                    _dataBase.Delete<GroupProductDB>(model.Id);
                }
            }
        }
    }
}
