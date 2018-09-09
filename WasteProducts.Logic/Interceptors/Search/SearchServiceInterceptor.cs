using FluentValidation;
using Ninject.Extensions.Interception;

namespace WasteProducts.Logic.Interceptors
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
            invocation.Proceed();
        }
    }
}