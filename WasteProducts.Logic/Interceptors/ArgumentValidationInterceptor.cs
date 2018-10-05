using FluentValidation;
using Ninject.Extensions.Interception;
using System;
using System.Linq;
using WasteProducts.Logic.Resources;

namespace WasteProducts.Logic.Interceptors
{
    /// <summary>
    /// Interceptor for trace validating method arguments
    /// </summary>
    public class ArgumentValidationInterceptor : BeforeInvokeAsyncInterceptor
    {
        private readonly IValidator _validator;
        private readonly Type _entityType;

        /// <summary>
        /// Constructor with arguments
        /// </summary>
        /// <param name="entityType">type to be validated</param>
        /// <param name="validator">FluentValidator for <c>entityType</c></param>
        public ArgumentValidationInterceptor(Type entityType, IValidator validator)
        {
            _validator = validator;
            _entityType = entityType;
        }

        /// <inheritdoc />
        protected override void BeforeInvoke(IInvocation invocation)
        {
            var methodArguments = invocation.Request.Arguments;

            foreach (var arg in methodArguments.Where(arg => arg?.GetType() == _entityType))
            {
                var validationResult = _validator.Validate(arg);

                if (!validationResult.IsValid)
                {
                    var msg = string.Format(InterceptorResources.ValidationMessageFormat, _entityType.Name, invocation.Request.Method.Name);
                    throw new ValidationException(msg, validationResult.Errors);
                }
            }
        }
    }
}