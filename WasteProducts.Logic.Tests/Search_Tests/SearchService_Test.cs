using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.Search;
using WasteProducts.Logic.Services;
using System.Linq;

namespace WasteProducts.Logic.Tests.Search_Tests
{
    [TestFixture]
    public class SearchService_Test
    {
        public List<User> users = new List<User>
            {
                new User { Id = 1, Login = "user1", Email = "user1@mail.net" },
                new User { Id = 2, Login = "user2", Email = "user2@mail.net" },
                new User { Id = 3, Login = "user3", Email = "user3@mail.net" },
                new User { Id = 4, Login = "user4", Email = "user4@mail.net" },
                new User { Id = 5, Login = "user5", Email = "user5@mail.net" }
            };

        //SearchResult Search<TEntity>(SearchQuery query);
        [Test]
        public void Search_CheckContainsKey_ReturnTrue()
        {
            var repo = new Mock<ISearchRepository>();

            //нужный метод репозитория
            repo.Setup(x => x.GetAll<User>()).Returns(users);

            var service = new LuceneSearchService(repo.Object);
            var query = new SearchQuery();

            var result = service.Search<User>(query);            

            Assert.AreEqual(true, result.Result.ContainsKey(typeof(User)));
        }

        [Test]
        public void Search_EmptyQuery_ReturnAllObjectsInRepository()
        {
            var repo = new Mock<ISearchRepository>();

            //нужный метод репозитория
            repo.Setup(x => x.GetAll<User>()).Returns(users);

            var service = new LuceneSearchService(repo.Object);
            var query = new SearchQuery();            

            var result = service.Search<User>(query);
            Assert.AreEqual(1, result.Result.Count);
        }

        [Test]
        public void Search_EmptyQuery_ReturnCount5()
        {
            var repo = new Mock<ISearchRepository>();

            //нужный метод репозитория
            repo.Setup(x => x.GetAll<User>()).Returns(users);

            var service = new LuceneSearchService(repo.Object);
            var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            var result = service.Search<User>(query);
            List<object> list = result.Result[typeof(User)].ToList();

            Assert.AreEqual(5, list.Count);
        }

        [Test]
        public void SearchId_ByLogin_ReturnUser1()
        {
            var repo = new Mock<ISearchRepository>();

            //нужный метод репозитория
            repo.Setup(x => x.GetAll<User>()).Returns(users);

            var service = new LuceneSearchService(repo.Object);
            var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            var result = service.Search<User>(query);            
            List<object> list = result.Result[typeof(User)].ToList();
            var user = (User)list[0];

            Assert.AreEqual(users[0].Id, user.Id);
        }



        //Task<SearchResult> SearchAsync<TEntity>(SearchQuery query);        
        //SearchResult SearchAll(SearchQuery query);        
        //Task<SearchResult> SearchAllAsync(SearchQuery query);        
        //SearchResult SearchDefault<TEntity>(SearchQuery query);        
        //Task<SearchResult> SearchDefaultAsync<TEntity>(SearchQuery query);        
        //SearchResult SearchAllDefault(SearchQuery query);
        //Task<SearchResult> SearchAllDefaultAsync(SearchQuery query);
        //void AddToSearchIndex<TEntity>(TEntity model);
        //void AddToSearchIndex<TEntity>(IEnumerable<TEntity> model);
        //void RemoveFromSearchIndex<TEntity>(TEntity model);
        //void RemoveFromSearchIndex<TEntity>(IEnumerable<TEntity> model);
        //void UpdateInSearchIndex<TEntity>(TEntity model);
        //void UpdateInSearchIndex<TEntity>(IEnumerable<TEntity> model);
        //bool ClearSearchIndex();
        //bool OptimizeSearchIndex();
    }
}
