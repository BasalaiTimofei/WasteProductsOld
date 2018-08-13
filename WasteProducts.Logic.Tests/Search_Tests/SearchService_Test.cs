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
using System.Threading.Tasks;

namespace WasteProducts.Logic.Tests.Search_Tests
{
    [TestFixture]
    public class SearchService_Test
    {
        [SetUp]
        public void Setup()
        {
            users = new List<User>
            {
                new User { Id = 1, Login = "user1", Email = "user1@mail.net" },
                new User { Id = 2, Login = "user2", Email = "user2@mail.net" },
                new User { Id = 3, Login = "user3", Email = "user3@mail.net" },
                new User { Id = 4, Login = "user4", Email = "user4@mail.net" },
                new User { Id = 5, Login = "user5", Email = "user5@mail.net" }
            };

            mockRepo = new Mock<ISearchRepository>();
            sut = new LuceneSearchService(mockRepo.Object);
        }

        private List<User> users;
        private Mock<ISearchRepository> mockRepo;
        private ISearchService sut;

        #region  SearchResult Search<TEntity>(SearchQuery query);
        [Test]
        public void Search_CheckContainsKey_ReturnTrue()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);
            
            var query = new SearchQuery();

            var result = sut.Search<User>(query);            

            Assert.AreEqual(true, result.Result.ContainsKey(typeof(User)));
        }

        [Test]
        public void Search_EmptyQuery_ReturnAllObjectsInRepository()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);
            
            var query = new SearchQuery();

            var result = sut.Search<User>(query);

            Assert.AreEqual(1, result.Result.Count);
        }

        [Test]
        public void Search_EmptyQuery_ReturnListCount()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);
            
            var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            var result = sut.Search<User>(query);
            List<object> list = result.Result[typeof(User)].ToList();

            Assert.AreEqual(5, list.Count);
        }

        [Test]
        public void SearchId_ByLogin_ReturnUser1()
        {    
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);
            
            var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            var result = sut.Search<User>(query);            
            List<object> list = result.Result[typeof(User)].ToList();
            var user = (User)list[0];

            Assert.AreEqual(users[0].Id, user.Id);
        }
        #endregion

        #region Task<SearchResult> SearchAsync<TEntity>(SearchQuery query);
        [Test]
        public async Task Search_CheckContainsKey_ReturnTrue_Async()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = await sut.SearchAsync<User>(query);

            Assert.AreEqual(true, result.Result.ContainsKey(typeof(User)));
        }

        [Test]
        public async Task Search_EmptyQuery_ReturnAllObjectsInRepository_Async()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = await sut.SearchAsync<User>(query);

            Assert.AreEqual(1, result.Result.Count);
        }

        [Test]
        public async Task Search_EmptyQuery_ReturnListCount_Async()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            var result = await sut.SearchAsync<User>(query);
            List<object> list = result.Result[typeof(User)].ToList();

            Assert.AreEqual(5, list.Count);
        }

        [Test]
        public async Task SearchId_ByLogin_ReturnUser1_Async()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            var result = await sut.SearchAsync<User>(query);
            List<object> list = result.Result[typeof(User)].ToList();
            var user = (User)list[0];

            Assert.AreEqual(users[0].Id, user.Id);
        }
        #endregion

        #region SearchResult SearchAll(SearchQuery query); 
        [Test]
        public void SearchAll_CheckContainsKey_ReturnTrue()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = sut.SearchAll(query);

            Assert.AreEqual(true, result.Result.ContainsKey(typeof(User)));
        }

        [Test]
        public void SearchAll_EmptyQuery_ReturnAllObjectsInRepository()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = sut.SearchAll(query);

            Assert.AreEqual(1, result.Result.Count);
        }
        #endregion

        #region Task<SearchResult> SearchAllAsync(SearchQuery query);
        [Test]
        public async Task SearchAll_CheckContainsKey_ReturnTrueAsync()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = await sut.SearchAllAsync(query);

            Assert.AreEqual(true, result.Result.ContainsKey(typeof(User)));
        }

        [Test]
        public async Task SearchAll_EmptyQuery_ReturnAllObjectsInRepositoryAsync()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = await sut.SearchAllAsync(query);

            Assert.AreEqual(1, result.Result.Count);
        }
        #endregion

        #region SearchResult SearchDefault<TEntity>(SearchQuery query);
        [Test]
        public void SearchDefault_CheckContainsKey_ReturnTrue()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = sut.SearchDefault<User>(query);

            Assert.AreEqual(true, result.Result.ContainsKey(typeof(User)));
        }

        [Test]
        public void SearchDefault_EmptyQuery_ReturnAllObjectsInRepository()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = sut.SearchDefault<User>(query);

            Assert.AreEqual(1, result.Result.Count);
        }

        [Test]
        public void SearchDefault_EmptyQuery_ReturnListCount()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            var result = sut.SearchDefault<User>(query);
            List<object> list = result.Result[typeof(User)].ToList();

            Assert.AreEqual(5, list.Count);
        }

        [Test]
        public void SearchDefaultId_ByLogin_ReturnUser1()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            var result = sut.SearchDefault<User>(query);
            List<object> list = result.Result[typeof(User)].ToList();
            var user = (User)list[0];

            Assert.AreEqual(users[0].Id, user.Id);
        }
        #endregion

        #region Task<SearchResult> SearchDefaultAsync<TEntity>(SearchQuery query);
        [Test]
        public async Task SearchDefault_CheckContainsKey_ReturnTrue_Async()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = await sut.SearchDefaultAsync<User>(query);

            Assert.AreEqual(true, result.Result.ContainsKey(typeof(User)));
        }

        [Test]
        public async Task SearchDefault_EmptyQuery_ReturnAllObjectsInRepository_Async()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = await sut.SearchDefaultAsync<User>(query);

            Assert.AreEqual(1, result.Result.Count);
        }

        [Test]
        public async Task SearchDefault_EmptyQuery_ReturnListCount_Async()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            var result = await sut.SearchDefaultAsync<User>(query);
            List<object> list = result.Result[typeof(User)].ToList();

            Assert.AreEqual(5, list.Count);
        }

        [Test]
        public async Task SearchDefaultId_ByLogin_ReturnUser1_Async()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            var result = await sut.SearchDefaultAsync<User>(query);
            List<object> list = result.Result[typeof(User)].ToList();
            var user = (User)list[0];

            Assert.AreEqual(users[0].Id, user.Id);
        }
        #endregion

        #region SearchResult SearchAllDefault(SearchQuery query);
        [Test]
        public void SearchAllDefault_CheckContainsKey_ReturnTrue()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = sut.SearchAllDefault(query);

            Assert.AreEqual(true, result.Result.ContainsKey(typeof(User)));
        }

        [Test]
        public void SearchAllDefault_EmptyQuery_ReturnAllObjectsInRepository()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = sut.SearchAllDefault(query);

            Assert.AreEqual(1, result.Result.Count);
        }
        #endregion

        #region Task<SearchResult> SearchAllDefaultAsync(SearchQuery query);
        [Test]
        public async Task SearchAllDefault_CheckContainsKey_ReturnTrueAsync()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = await sut.SearchAllDefaultAsync(query);

            Assert.AreEqual(true, result.Result.ContainsKey(typeof(User)));
        }

        [Test]
        public async Task SearchAllDefault_EmptyQuery_ReturnAllObjectsInRepositoryAsync()
        {
            //нужный метод репозитория
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();

            var result = await sut.SearchAllDefaultAsync(query);

            Assert.AreEqual(1, result.Result.Count);
        }
        #endregion

        #region void AddToSearchIndex<TEntity>(TEntity model);        
        [Test]
        public void AddSearchIndex_AddNewEntity_ResultVerify()
        {
            var mockUser = new Mock<User>();

            //нужный метод репозитория
            mockRepo.Setup(x => x.Insert<User>(mockUser.Object)).Verifiable();            

            sut.AddToSearchIndex<User>(mockUser.Object);

            mockRepo.Verify(v => v.Insert<User>(mockUser.Object), Times.Once);
        }
        #endregion

        #region void AddToSearchIndex<TEntity>(IEnumerable<TEntity> model);
        [Test]
        public void AddSearchIndex_AddNewIEnumerable_ResultVerify()
        {
            var mockCollectionUser = new Mock<IEnumerable<User>>();            
            var mockUser = new Mock<User>();
                        
            mockCollectionUser.SetupGet(c => c.Count<User>()).Returns(5);

            //нужный метод репозитория
            mockRepo.Setup(x => x.Insert<IEnumerable<User>>(mockCollectionUser.Object)).Verifiable();

            sut.AddToSearchIndex<User>(mockCollectionUser.Object);

            mockRepo.Verify(v => v.Insert<IEnumerable<User>>(mockCollectionUser.Object), Times.Exactly(5));
        }
        #endregion

        #region void RemoveFromSearchIndex<TEntity>(TEntity model);
        [Test]
        public void RemoveSearchIndex_DeleteEntity_ResultVerify()
        {
            var mockUser = new Mock<User>();

            //нужный метод репозитория
            mockRepo.Setup(x => x.Delete<User>(mockUser.Object)).Verifiable();

            sut.RemoveFromSearchIndex<User>(mockUser.Object);

            mockRepo.Verify(v => v.Delete<User>(mockUser.Object), Times.Once);
        }
        #endregion
        //void RemoveFromSearchIndex<TEntity>(IEnumerable<TEntity> model);

        //void UpdateInSearchIndex<TEntity>(TEntity model);
        //void UpdateInSearchIndex<TEntity>(IEnumerable<TEntity> model);

        //bool ClearSearchIndex();

        //bool OptimizeSearchIndex();
    }
}
