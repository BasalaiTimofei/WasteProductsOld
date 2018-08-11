using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models.Search
{
    /// <summary>
    /// Model for returning results of full-text search
    /// </summary>
    public class SearchResult
    {

        /// <summary>
        /// Dictionary containing IEnumerable list of founded objects and their type
        /// </summary>
        public Dictionary<Type, IEnumerable<object>> Result { get; } = new Dictionary<Type, IEnumerable<object>>();
    }
}
