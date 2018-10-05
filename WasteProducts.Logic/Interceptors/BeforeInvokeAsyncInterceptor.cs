using Ninject.Extensions.Interception;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Interceptors
{
    public abstract class BeforeInvokeAsyncInterceptor : IInterceptor
    {
        private static MethodInfo startTaskMethodInfo = typeof(BeforeInvokeAsyncInterceptor).GetMethod("InterceptTaskWithResult", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>Intercepts the specified invocation.</summary>
        /// <param name="invocation">The invocation to intercept.</param>
        public void Intercept(IInvocation invocation)
        {
            Type returnType = invocation.Request.Method.ReturnType;
            if (returnType == typeof(Task))
            {
                this.InterceptTask(invocation);
            }
            else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                Type genericArgument = returnType.GetGenericArguments()[0];
                BeforeInvokeAsyncInterceptor.startTaskMethodInfo.MakeGenericMethod(genericArgument).Invoke((object)this, new object[1]
                {
                    (object) invocation
                });
            }
            else
            {
                this.BeforeInvoke(invocation);
                invocation.Proceed();
                
            }
        }

        /// <summary>Takes some action before the invocation proceeds.</summary>
        /// <param name="invocation">The invocation that is being intercepted.</param>
        protected virtual void BeforeInvoke(IInvocation invocation)
        {
        }

        private void InterceptTask(IInvocation invocation)
        {
            IInvocation invocationClone = invocation.Clone();
            invocation.ReturnValue = (object)Task.Factory.StartNew((Action)(() => this.BeforeInvoke(invocation))).ContinueWith<Task>((Func<Task, Task>)(t =>
            {
                var tcs = new TaskCompletionSource<Task>();
                if (t.IsFaulted)
                {
                    tcs.TrySetException(t.Exception);
                    return tcs.Task;
                }
                else if (t.IsCanceled)
                {
                    tcs.TrySetCanceled();
                    return tcs.Task;
                }
                else // RanToCompletion
                {
                    invocationClone.Proceed();
                    return invocationClone.ReturnValue as Task;
                }
            })).Unwrap();
        }

        private void InterceptTaskWithResult<TResult>(IInvocation invocation)
        {
            IInvocation invocationClone = invocation.Clone();
            invocation.ReturnValue = (object)Task.Factory.StartNew((Action)(() => this.BeforeInvoke(invocation))).ContinueWith<Task<TResult>>((Func<Task, Task<TResult>>)(t =>
            {
                var tcs = new TaskCompletionSource<TResult>();
                if (t.IsFaulted)
                {
                    tcs.TrySetException(t.Exception);
                    return tcs.Task;
                }
                else if (t.IsCanceled)
                {
                    tcs.TrySetCanceled();
                    return tcs.Task;
                }

                invocationClone.Proceed();
                return invocationClone.ReturnValue as Task<TResult>;
            })).Unwrap<TResult>();
        }
    }
}