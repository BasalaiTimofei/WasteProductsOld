using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Services.Products;

namespace WasteProducts.Logic.Services.Products
{
    /// <summary>
    /// Implementation of ICategoryService.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public Task<string> Add(string name)
        {
            if (IsCategoryInDB(p =>
                string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return null;

            var newCategory = new Category { Name = name };
            return _categoryRepository.AddAsync(_mapper.Map<CategoryDB>(newCategory))
                .ContinueWith(c => c.Result);
        }

        public Task<IEnumerable<string>> AddRange(IEnumerable<string> nameRange)
        {
            var categoriesDB = _categoryRepository.SelectAllAsync().Result;
            if (nameRange.All(name =>
            {
                return categoriesDB.All(c =>
                    !string.Equals(c.Name, name, StringComparison.CurrentCultureIgnoreCase));
            })) return null;

            var newCategoies = new List<Category>();
            nameRange.Select(c =>
            {
                newCategoies.Add(new Category {Name = c});
                return c;
            });

            return _categoryRepository.AddRangeAsync(_mapper.Map<IEnumerable<CategoryDB>>(newCategoies));
        }

        public Task<IEnumerable<Category>> GetAll()
        {
            return _categoryRepository.SelectAllAsync()
                .ContinueWith(t => _mapper.Map<IEnumerable<Category>>(t.Result));
        }

        public Task<Category> GetById(string id)
        {
            return _categoryRepository.GetByIdAsync(id).ContinueWith(t => _mapper.Map<Category>(t.Result));
        }

        public Task<Category> GetByName(string name)
        {
            return _categoryRepository.GetByNameAsync(name).ContinueWith(t => _mapper.Map<Category>(t.Result));
        }

        public Task Update(Category category)
        {
            if (!IsCategoryInDB(c =>
                    string.Equals(c.Id, category.Id, StringComparison.Ordinal),
                out var categories)) return null;

            return _categoryRepository.UpdateAsync(_mapper.Map<CategoryDB>(category));
        }

        public Task Delete(string id)
        {
            if (!IsCategoryInDB(p =>
                    string.Equals(p.Id, id, StringComparison.Ordinal),
                out var categories)) return null;

            var categoryFromDB = categories.ToList().First();
            return _categoryRepository.DeleteAsync(categoryFromDB);
        }

        private bool IsCategoryInDB(Predicate<CategoryDB> conditionPredicate, out IEnumerable<CategoryDB> categories)
        {
            categories = _categoryRepository.SelectWhereAsync(conditionPredicate).Result;
            return categories.Any();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _categoryRepository.Dispose();
                }

                _disposed = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CategoryService()
        {
            Dispose(false);
        }
    }
}
