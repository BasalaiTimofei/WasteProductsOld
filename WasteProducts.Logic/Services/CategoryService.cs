using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public bool AddByName(string name)
        {
            if (IsCategoryInDB(p =>
                string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return false;

            var newCategory = new Category { Name = name };
            _categoryRepository.Add(_mapper.Map<CategoryDB>(newCategory));

            return true;
        }

        public bool AddRange(IEnumerable<string> names)
        {
            var result = false;

            foreach(var name in names)
            {
                if (AddByName(name) && !result) result = true;
            }

            return result;
        }

        public Category Get(string name)
        {
            if (!IsCategoryInDB(p =>
                string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return null;

            return _mapper.Map<Category>(categories.ToList().First());
        }

        public void SetDescription(Category category, string description)
        {
            if (!IsCategoryInDB(p =>
                    string.Equals(p.Name, category.Name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return;

            var categoryFromDB = categories.ToList().First();
            categoryFromDB.Description = description;
            _categoryRepository.Update(categoryFromDB);
        }

        public bool Delete(string name)
        {
            if (!IsCategoryInDB(p =>
                    string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return false;

            var categoryFromDB = categories.ToList().First();
            _categoryRepository.Delete(categoryFromDB);

            return true;
        }

        public bool DeleteRange(IEnumerable<string> names)
        {
            var result = false;

            foreach (var name in names)
            {
                if (Delete(name) && !result) result = true;
            }

            return result;
        }

        private bool IsCategoryInDB(Predicate<CategoryDB> conditionPredicate, out IEnumerable<CategoryDB> categories)
        {
            categories = _categoryRepository.SelectWhere(conditionPredicate);
            return categories.Any();
        }
    }
}
