using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Ninject;
using Ninject.Extensions.Interception;
using WasteProducts.Logic.Common.Models.Search;

namespace WasteProducts.Logic
{
    public class SearchServiceInterceptor : IInterceptor
    {
        private IValidator _validator;
        public SearchServiceInterceptor(IValidator validator)
        {
            _validator = validator;
        }
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}