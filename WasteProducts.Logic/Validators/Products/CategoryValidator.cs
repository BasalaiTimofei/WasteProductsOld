using FluentValidation;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Validators.Products
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
        }
    }
}
