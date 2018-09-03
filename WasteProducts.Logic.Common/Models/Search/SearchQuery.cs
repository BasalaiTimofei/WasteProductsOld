using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WasteProducts.Logic.Common.Models.Search;

namespace WasteProducts.Logic.Common.Models
{
    [TypeConverter(typeof(SearchQueryConverter))]
    /// <summary>
    /// Model of search query
    /// </summary>
    public class SearchQuery
    {
        /// <summary>
        /// String that contains text to search
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// The list of fields (model properties) to look through
        /// </summary>
        public ReadOnlyCollection<string> SearchableFields
        {
            get
            {
                return _SearchableFields.AsReadOnly();
            }
        }

        private List<string> _SearchableFields;

        public SearchQuery()
        {
            _SearchableFields = new List<string>();
        }

        public SearchQuery(string query)
        {
            _SearchableFields = new List<string>();
            Query = query;
        }

        public virtual SearchQuery AddField(string field)
        {
            _SearchableFields.Add(field);
            return this;
        }

        public SearchQuery SetQueryString(string query)
        {
            Query = query;
            return this;
        }
    }
}
