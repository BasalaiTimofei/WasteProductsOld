using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WasteProducts.Web.Models.Users;

namespace WasteProducts.Web.Validators.Users
{
    public class ProductDescriptionValidator : AbstractValidator<ProductDescription>
    {
        private const int _minProductRating = 1;
        private const int _maxProductRating = 5;
        private const int _maxProductDescriptionLength = 400;

        public ProductDescriptionValidator()
        {
            RuleFor(x => x.Rating).NotNull().NotEmpty().GreaterThanOrEqualTo(_minProductRating)
                .LessThanOrEqualTo(_maxProductRating)
                .WithMessage("Product rating is required and should be between " +
                $"{_minProductRating} and {_maxProductRating}.");
            RuleFor(x => x.Description).MaximumLength(_maxProductDescriptionLength)
                .WithMessage("Product description cannot be more than " +
                $"{_maxProductDescriptionLength}");
        }
    }
}