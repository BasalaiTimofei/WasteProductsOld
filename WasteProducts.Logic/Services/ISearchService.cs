using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common;
using WasteProducts.Logic.Common.Models;

namespace WasteProducts.Logic.Services
{
    public interface ISearchService
    {
        IEnumerable<T> Search<T>(ISearchQuery query);
        Task<IEnumerable<T>> SearchAll<T>(ISearchQuery query);
        void AddToSearchIndex<T>(T obj);
        void UpdateInSearchIndex<T>(T obj);
        void DeleteFromSearchIndex<T>(T obj);
    }
}
