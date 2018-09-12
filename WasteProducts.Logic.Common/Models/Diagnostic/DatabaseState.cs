using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Models.Diagnostic
{
    /// <summary>
    /// Model for database state response
    /// </summary>
    public class DatabaseState
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DatabaseState() { }

        /// <summary>
        /// Constructor with params
        /// </summary>
        /// <param name="isExist"></param>
        /// <param name="isCompatibleWithModel"></param>
        public DatabaseState(bool isExist, bool isCompatibleWithModel)
        {
            IsExist = isExist;
            IsCompatibleWithModel = isCompatibleWithModel;
        }

        /// <summary>
        /// "True" if database already exist, in otherwise "False".
        /// </summary>
        public bool IsExist { get; set; }

        /// <summary>
        /// "True" if database compatible with model in code, in otherwise "False".
        /// </summary>
        public bool IsCompatibleWithModel { get; set; }
    }
}