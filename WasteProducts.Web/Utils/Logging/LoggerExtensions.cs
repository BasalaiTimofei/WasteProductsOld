using System;
using NLog;

namespace WasteProducts.Web.Utils
{
    public static class LoggerExtensions
    {
        public static void ActionError(this ILogger logger, Exception exception, string controllerName,
            string actionName)
        {
            logger.Error(exception, $"Action \"{controllerName}.{actionName}\" throw exception:");
        }
    }
}