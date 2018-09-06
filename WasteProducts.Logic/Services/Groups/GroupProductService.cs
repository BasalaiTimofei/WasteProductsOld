using AutoMapper;
using System;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;

namespace WasteProducts.Logic.Services.Groups
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

        public void Create<T>(T item, string userId) where T : class
        {
            var result = _mapper.Map<GroupProductDB>(item);

            result.Modified = DateTime.UtcNow;

            _dataBase.Create(result);
            _dataBase.Save();
        }

        public void Update<T>(T item, string userId) where T : class
        {
            var result = _mapper.Map<GroupProductDB>(item);
            var model = _dataBase.Get<GroupProductDB>(result.Id);

            model.ProductId = result.ProductId;
            model.Information = result.Information;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            _dataBase.Save();
        }

        public void Delete<T>(T item, string userId) where T : class
        {
            var result = _mapper.Map<GroupProductDB>(item);

            _dataBase.Delete<GroupProductDB>(result.Id);
            _dataBase.Save();
        }

        public T FindById<T>(Guid id) where T : class
        {
            var model = _dataBase.Find<GroupProductDB>(x => x.Id == id).First();
            var result = _mapper.Map<T>(model);

            return result;
        }

    }
}
