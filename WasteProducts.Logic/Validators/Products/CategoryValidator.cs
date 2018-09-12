using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WasteProducts.Logic.Common.Models.Products;
using FluentAssertions;

namespace WasteProducts.Logic.Validators.Products
{
    public class CategoryValidator : AbstractValidator<Category>
    {
    }
}
