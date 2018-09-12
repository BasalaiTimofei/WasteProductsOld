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

        /// <summary>
        /// Tries to add a new category by name and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="name">The name of the category to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        public bool Add(string name)
        {
            if (IsCategoryInDB(p =>
                string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return false;

            var newCategory = new Category { Name = name };
            _categoryRepository.Add(_mapper.Map<CategoryDB>(newCategory));

            return true;
        }

        /// <summary>
        /// Tries to add a list of new categories by names and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="names">The list of names of the categories to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        public bool AddRange(IEnumerable<string> names)
        {
            var result = false;

            foreach(var name in names)
            {
                if (Add(name) && !result) result = true;
            }

            return result;
        }

        /// <summary>
        /// Returns a spicific category by its name.
        /// </summary>
        /// <param name="name">The name of the category to be gotten.</param>
        /// <returns>The specific category to be returned.</returns>
        public Category Get(string name)
        {
            if (!IsCategoryInDB(p =>
                string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return null;

            return _mapper.Map<Category>(categories.ToList().First());
        }

        /// <summary>
        /// Adds the description for specific category.
        /// </summary>
        /// <param name="category">The specific category for which a description is added.</param>
        /// <param name="description">The specific description for the specfic category.</param>
        public void SetDescription(Category category, string description)
        {
            if (!IsCategoryInDB(p =>
                    string.Equals(p.Name, category.Name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return;

            var categoryFromDB = categories.ToList().First();
            categoryFromDB.Description = description;
            _categoryRepository.Update(categoryFromDB);
        }

        /// <summary>
        /// Tries to delete the specific category.
        /// </summary>
        /// <param name="name">The name of the category to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
        public bool Delete(string name)
        {
            if (!IsCategoryInDB(p =>
                    string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var categories)) return false;

            var categoryFromDB = categories.ToList().First();
            _categoryRepository.Delete(categoryFromDB);

            return true;
        }

        /// <summary>
        /// Tries to delete the list of specific categories.
        /// </summary>
        /// <param name="names">The list of names of the categories to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
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
