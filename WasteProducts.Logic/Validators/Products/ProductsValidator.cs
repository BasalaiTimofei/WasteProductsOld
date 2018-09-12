using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentAssertions;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Validators.Products
{
    class ProductsValidator : AbstractValidator<Product>
    {
        public ProductsValidator()
        {
        }
    }
}
