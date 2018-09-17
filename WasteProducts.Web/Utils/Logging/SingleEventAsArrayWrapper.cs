using System;
using NLog.Common;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace WasteProducts.Web.Utils.Logging
{
    /// <summary>
    /// Wrapper for single log event
    /// </summary>
    [Target("SingleEventAsArray", IsWrapper = true)]
    public sealed class SingleEventAsArrayWrapper : WrapperTargetBase
    {
        /// <inheritdoc/>
        protected override void Write(AsyncLogEventInfo logEvent)
        {
            WrappedTarget.WriteAsyncLogEvents(logEvent);
        }

        /// <inheritdoc/>
        [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
        protected override void Write(AsyncLogEventInfo[] logEvents)
        {
            WrappedTarget.WriteAsyncLogEvents(logEvents);
        }
    }
}