namespace WasteProducts.Logic.Common.Models.Barcods
{
    /// <summary>
    /// Model for entity HttpQueryResult.
    /// </summary>
    public class HttpQueryResult
    {
        /// <summary>
        /// StatusCode.
        /// </summary>
        public int StatusCode { get; set; } = -1;

        /// <summary>
        /// Page uri.
        /// </summary>
        public string Page { get; set; } = string.Empty;
    }
}
