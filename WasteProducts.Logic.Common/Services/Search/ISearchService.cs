using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Search;

namespace WasteProducts.Logic.Common.Services.Search
{
    /// <summary>
    /// This interface provides search methods
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// This method provides ability to search by partial words. It replaces all dashes "-" in search requests, and adds "*" (star) after each word.
        /// </summary>
        /// <typeparam name="TEntity">Object model</typeparam>
        /// <param name="query">SearchQuery model</param>
        /// <returns>IEnumerable of data</returns>
        SearchResult Search<TEntity>(SearchQuery query);
        Task<SearchResult> SearchAsync<TEntity>(SearchQuery query);

        SearchResult SearchAll(SearchQuery query);
        SearchResult SearchAllAsync(SearchQuery query);


        /// <summary>
        /// This method provides ability to search without any formatting at search requests
        /// </summary>
        /// <typeparam name="TEntity">Object model</typeparam>
        /// <param name="query">SearchQuery model</param>
        /// <returns>IEnumerable of data</returns>
        SearchResult SearchDefault<TEntity>(SearchQuery query);
        Task<SearchResult> SearchDefaultAsync<TEntity>(SearchQuery query);

        SearchResult SearchAllDefault(SearchQuery query);
        SearchResult SearchAllDefaultAsync(SearchQuery query);

        void AddToSearchIndex<TEntity>(TEntity model);
        void AddToSearchIndex<TEntity>(IEnumerable<TEntity> model);

        void RemoveSearchIndex<TEntity>(TEntity model);
        void RemoveSearchIndex<TEntity>(IEnumerable<TEntity> model);

        void UpdateInSearchIndex<TEntity>(TEntity model);
        void UpdateInSearchIndex<TEntity>(IEnumerable<TEntity> model);

        bool ClearSearchIndex();
        bool OptimizeSearchIndex();

        //Александр,
        //Предполагалось, что данные методы будут создавать\обновлять\удалять индекс в Lucene.
        //Правльно ли я понял, что здесь они не нужны и их можно будет создать когда начнем конфигурировать context?

        //добвление\обновление индекса при первом запуске, а также при добавлении нового объкта в базу
        //void AddUpdateIndex<TEntity>(IEnumerable<TEntity> model);
        //void AddUpdateIndex<TEntity>(TEntity model);
        //удаление индекса целиком и при удалении объекта
        //void ClearIndexRecord<TEntity>(TEntity model);
        //bool ClearIndex();
    }
}
