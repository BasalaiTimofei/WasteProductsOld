using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WasteProducts.Logic.Common.Models.Search
{
    [TypeConverter(typeof(BoostedSearchQueryConverter))]
    /// <summary>
    /// Implements SearchQuery class with term's boosts values
    /// </summary>
    public class BoostedSearchQuery : SearchQuery
    {
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

        public BoostedSearchQuery() : base()
        {
            _BoostValues = new Dictionary<string, float>();
        }

        public BoostedSearchQuery(string query) : base(query)
        {
            _BoostValues = new Dictionary<string, float>();
        }

        public new BoostedSearchQuery SetQueryString(string query)
        {
            Query = query;
            return this;
        }

        public override SearchQuery AddField(string field)
        {
            return AddField(field, 1.0f);
        }

        public BoostedSearchQuery AddField(string field, float boostValue)
        {
            base.AddField(field);
            _BoostValues.Add(field, boostValue);
            return this;
        }
    }
}
