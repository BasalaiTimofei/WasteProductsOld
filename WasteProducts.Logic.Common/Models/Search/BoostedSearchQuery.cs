using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WasteProducts.Logic.Common.Models.Search
{
    [TypeConverter(typeof(BoostedSearchQueryConverter))]
    /// <summary>
    /// Implements SearchQuery class with term's boosts values
    /// </summary>
    public class BoostedSearchQuery
    {
        public string Query { get; set; }

        private List<string> _SearchableFields;

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

        /// <summary>
        /// Dictionary with boost values for searchable fields
        /// </summary>
        public ReadOnlyDictionary<string, float> BoostValues
        {
            get
            {
                return new ReadOnlyDictionary<string, float>(_BoostValues);
            }
        }
        private Dictionary<string, float> _BoostValues { get; }

        public BoostedSearchQuery()
        {
            _BoostValues = new Dictionary<string, float>();
            _SearchableFields = new List<string>();
        }

        public BoostedSearchQuery(string query)
        {
            Query = query;
            _BoostValues = new Dictionary<string, float>();
            _SearchableFields = new List<string>();
        }

        public BoostedSearchQuery SetQueryString(string query)
        {
            Query = query;
            return this;
        }

        public BoostedSearchQuery AddField(string field)
        {
            return AddField(field, 1.0f);
        }

        public BoostedSearchQuery AddField(string field, float boostValue)
        {
            _SearchableFields.Add(field);
            _BoostValues.Add(field, boostValue);
            return this;
        }
    }
}
