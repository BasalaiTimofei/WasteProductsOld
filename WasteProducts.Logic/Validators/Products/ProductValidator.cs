using FluentValidation;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Validators.Products
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Category.Name).NotEmpty().When(x => x.Category != null);
            RuleFor(x => x.Barcode.Code).NotEmpty().Matches(@"\d{13}").When(x => x.Barcode != null);
        }
    }
}
