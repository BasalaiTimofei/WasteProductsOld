using System;
using System.Linq;
using FluentValidation;
using Ninject.Extensions.Interception;
using WasteProducts.Logic.Common.Models.Search;

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
            string methodName = invocation.Request.Method.Name;
            bool errorsPresent = false;

            switch (methodName)
            {
                case "SearchProduct":
                    ValidateQuery((BoostedSearchQuery)invocation.Request.Arguments[0]);
                    break;
            }

            invocation.Proceed();
        }

        private void ValidateQuery(BoostedSearchQuery query)
        {
            if (query != null)
            {
                var result = _validator.Validate(query);
                if (!result.IsValid)
                {
                    throw new ValidationException(
                        result.Errors.Select(x => x.ErrorMessage)
                            .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}")
                    );
                }
            }
            else
            {
                throw new ValidationException(Resources.SearchService.IncorrectQueryStr);
            }
        }
    }
}