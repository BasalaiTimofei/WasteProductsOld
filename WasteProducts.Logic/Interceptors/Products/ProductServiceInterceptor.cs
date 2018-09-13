using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ninject.Extensions.Interception;

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
            var r = invocation.Request.Kernel.Components.Get(_validator.GetType());
            Console.WriteLine(r);
        }
    }
}
