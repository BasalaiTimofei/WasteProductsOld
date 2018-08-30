using System;

namespace WasteProducts.Logic.Common.Models.Diagnostic
{
    /// <summary>
    /// Result of database operation
    /// </summary>
    public class DatabaseResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isSuccess">boolean success flag</param>
        /// <param name="log">Log string</param>
        public DatabaseResult(bool isSuccess, string log)
        {
            IsSuccess = isSuccess;
            Log = log ?? String.Empty;
        }

        /// <summary>
        /// Return <c>true</c> database operation finished successful, otherwise <c>false</c>
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Log string
        /// </summary>
        public string Log { get; set; }
    }
}