using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
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

        public async Task<string> Create(GroupProduct item)
        {
            var result = _mapper.Map<GroupProductDB>(item);

            var modelBoard = (await _dataBase.Find<GroupBoardDB>(
                x => x.Id == result.GroupBoardId).ConfigureAwait(false)).FirstOrDefault();
            if (modelBoard == null)
                throw new ValidationException("Board not found");

            var modelProduct = await _dataBase.Find<GroupProductDB>(x => x.ProductId == result.ProductId).ConfigureAwait(false);
            if (modelProduct.Any())
                throw new ValidationException("Product already added");

            result.Id = Guid.NewGuid().ToString();
            result.Modified = DateTime.UtcNow;

            _dataBase.Create(result);
            await _dataBase.Save();
            return result.Id;
        }

        public async Task Update(GroupProduct item)
        {
            var result = _mapper.Map<GroupProductDB>(item);

            var model = (await _dataBase.Find<GroupProductDB>(
                x => x.Id == result.Id).ConfigureAwait(false)).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Product not found");

            model.ProductId = result.ProductId;
            model.Information = result.Information;
            model.Modified = DateTime.UtcNow;

            _dataBase.Update(model);
            await _dataBase.Save();
        }

        public async Task Delete(string groupProductId)
        {
            var model = (await _dataBase.Find<GroupProductDB>(
                x => x.Id == groupProductId).ConfigureAwait(false)).FirstOrDefault();
            if (model == null)
                throw new ValidationException("Product not found");

            _dataBase.Delete(model);
            await _dataBase.Save();
        }

        public async Task<GroupProduct> FindById(string groupProductId)
        {
            var model = (await _dataBase.Find<GroupProductDB>(
                x => x.Id == groupProductId)).FirstOrDefault();
            if (model == null)
                return null;

            var result = _mapper.Map<GroupProduct>(model);

            return result;
        }
    }
}
