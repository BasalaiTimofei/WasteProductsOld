namespace WasteProducts.Logic.Common.Models.Diagnostic
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseState
    {
        /// <summary>
        /// "True" if database already exist, in otherwise "False".
        /// </summary>
        public bool IsDbExist { get; set; }

        /// <summary>
        /// "True" if database compatible with model in code, in otherwise "False".
        /// </summary>
        public bool IsDbCompatibleWithModel { get; set; }
    }
}