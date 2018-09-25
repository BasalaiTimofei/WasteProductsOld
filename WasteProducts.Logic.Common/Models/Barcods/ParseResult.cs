namespace WasteProducts.Logic.Common.Models.Barcods
{
    /// <summary>
    /// Model for entity ParseResult.
    /// </summary>
    public class ParseResult
    {
        /// <summary>
        /// Success.
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// Value.
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}
