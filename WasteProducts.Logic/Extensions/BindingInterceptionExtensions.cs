using System;
using FluentValidation;
using Ninject;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Syntax;
using WasteProducts.Logic.Interceptors;

namespace WasteProducts.Logic.Extensions
{
    /// <summary>
    /// Ninject binding extension methods for interception
    /// </summary>
    public static class BindingInterceptionExtensions
    {
        /// <summary>
        /// Indicates that binding should be intercepted via the trace interceptor.
        /// The interceptor will be created via the kernel when the method is called.
        /// </summary> 
        /// <returns>The binding builder.</returns>
        public static IBindingOnSyntax<T> Trace<T>(this IBindingOnSyntax<T> syntax)
        {
            syntax.Intercept().With<TraceInterceptor>().InOrder(0);

            return syntax;
        }

        /// <summary>
        /// Indicates that binding should be intercepted via the argument validation interceptor.
        /// The interceptor will be created via the kernel when the method is called.
        /// </summary> 
        /// <returns>The binding builder.</returns>
        public static IBindingOnSyntax<T> ValidateArguments<T>(this IBindingOnSyntax<T> syntax, params Type[] argsTypes)
        {
            foreach (var argsType in argsTypes)
            {
                syntax.Intercept().With(request =>
                {
                    var validatorType = typeof(IValidator<>).MakeGenericType(argsType);
                    var validator = request.Kernel.Get(validatorType) as IValidator;

                    return new ArgumentValidationInterceptor(argsType, validator);
                });
            }

            return syntax;
        }
    }
}