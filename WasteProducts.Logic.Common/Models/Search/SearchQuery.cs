namespace WasteProducts.Logic.Common.Models
{
    /// <summary>
    /// Model uses for search
    /// </summary>
    public class SearchQuery
    {
        /// <summary>
        /// The query that contains text to search
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// The array of fields (model properties) of search
        /// </summary>
        public string[] SearchableFields { get; set; }

    }
}
