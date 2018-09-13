using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ninject.Extensions.Interception;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Services.Products;

namespace WasteProducts.Logic.Interceptors.Products
{
    class ProductServiceInterceptor : IInterceptor
    {
        private IValidator _validator;

        public ProductServiceInterceptor(IValidator validator)
        {
            this._validator = validator;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Request.Method.Name == "Add")
                ValidateProduct((ProductService)invocation.Request.Target
                );

            invocation.Proceed();
        }

        private void ValidateProduct(ProductService product)
        {
            var result = _validator.Validate(product);
            if (!result.IsValid)
                throw new ValidationException(
                    result.Errors.Select(x => x.ErrorMessage)
                        .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}"));
        }
    }
}
