using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Extensions.Interception;

namespace WasteProducts.Web.Utils.Interception
{
    public class SearchControllerInterceptor : IInterceptor
    {
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