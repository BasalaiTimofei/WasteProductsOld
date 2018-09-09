namespace WasteProducts.Logic.Common.Models.Diagnostic
{
    /// <summary>
    /// Model for database status response
    /// </summary>
    public class DatabaseStatus
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DatabaseStatus() { }

        /// <summary>
        /// Constructor with params
        /// </summary>
        /// <param name="isDbExist"></param>
        /// <param name="isDbCompatibleWithModel"></param>
        public DatabaseStatus(bool isDbExist, bool isDbCompatibleWithModel)
        {
            IsDbExist = isDbExist;
            IsDbCompatibleWithModel = isDbCompatibleWithModel;
        }

        /// <summary>
        /// "True" if database already exist, in otherwise "False".
        /// </summary>
        public bool IsDbExist { get; set; }

        /// <summary>
        /// "True" if database compatible with model in code, in otherwise "False".
        /// </summary>
        public bool IsDbCompatibleWithModel { get; set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is DatabaseStatus databaseStatusObj 
                   && this.IsDbExist == databaseStatusObj.IsDbExist 
                   && this.IsDbCompatibleWithModel == databaseStatusObj.IsDbCompatibleWithModel;
        }
    }
}