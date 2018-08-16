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

        private IEnumerable<User> users;
        private Mock<ISearchRepository> mockRepo;
        private ISearchService sut;

        #region IEnumerable<TEntity> Search<TEntity>(SearchQuery query) where TEntity : class
        [Test]
        public void Search_EmptyQuery_Return_Exception()
        {
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();
            Assert.Throws<ArgumentException>(()=>sut.Search<User>(query));
        }

        /*[Test]
        public void Search_EmptyQuery_Return_NoException()
        {
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();
            var result = sut.Search<User>(query);
        }

        [Test]
        public void Search_GetAllVerify_Return_Once()
        {
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>())).Verifiable();

            var query = new SearchQuery();            
            var result = sut.Search<User>(query);

            mockRepo.Verify(v => v.GetAll<User>(It.IsAny<string>(), It.IsAny<IEnumerable<string>> (), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Search_GetAll_Return_AllObjectsCount()
        {
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<int>())).Returns(users);

            var query = new SearchQuery();
            var result = sut.Search<User>(query);

            Assert.AreEqual(users.Count(), result.Count());
        }*/
        #endregion

        #region void AddToSearchIndex<TEntity>(TEntity model) where TEntity : class
        [Test]
        public void AddIndex_InsertNewIndex_Return_NoException()
        {
            User user = new User();
            mockRepo.Setup(x => x.Insert<User>(user));

            sut.AddToSearchIndex<User>(user);
        }

        [Test]
        public void AddIndex_InsertNewIndexVerify_Return_Once()
        {
            var user = new User();
            mockRepo.Setup(x => x.Insert<User>(user)).Verifiable();

            sut.AddToSearchIndex<User>(user);

            mockRepo.Verify(v => v.Insert<User>(user), Times.Once);
        }
        #endregion

        #region void AddToSearchIndex<TEntity>(IEnumerable<TEntity> models) where TEntity : class
        [Test]
        public void AddIndex_InsertNewIndexIEnumerable_Return_NoException()
        {
            mockRepo.Setup(x => x.Insert<User>(It.IsAny<User>()));

            sut.AddToSearchIndex<User>(users);
        }

        [Test]
        public void AddIndex_InsertNewIndexIEnumerableVerify_Return_UsersCount()
        {
            mockRepo.Setup(x => x.Insert<User>(It.IsAny<User>())).Verifiable();

            sut.AddToSearchIndex<User>(users);

            mockRepo.Verify(v => v.Insert<User>(It.IsAny<User>()), Times.Exactly(users.Count<User>()));
        }
        #endregion

        #region void RemoveFromSearchIndex<TEntity>(TEntity model) where TEntity : class
        [Test]
        public void RemoveIndex_DeleteIndex_Return_NoException()
        {
            User user = new User();
            mockRepo.Setup(x => x.Delete<User>(user));

            sut.RemoveFromSearchIndex<User>(user);
        }

        [Test]
        public void RemoveIndex_DeleteIndexVerify_Return_Once()
        {
            var user = new User();
            mockRepo.Setup(x => x.Delete<User>(user)).Verifiable();

            sut.RemoveFromSearchIndex<User>(user);

            mockRepo.Verify(v => v.Delete<User>(user), Times.Once);
        }
        #endregion

        #region void RemoveFromSearchIndex<TEntity>(IEnumerable<TEntity> models) where TEntity : class
        [Test]
        public void RemoveIndex_DeleteIndexIEnumerable_Return_NoException()
        {
            mockRepo.Setup(x => x.Delete<User>(It.IsAny<User>()));

            sut.RemoveFromSearchIndex<User>(users);
        }

        [Test]
        public void RemoveIndex_DeleteIndexIEnumerableVerify_Return_UsersCount()
        {
            mockRepo.Setup(x => x.Delete<User>(It.IsAny<User>())).Verifiable();

            sut.RemoveFromSearchIndex<User>(users);

            mockRepo.Verify(v => v.Delete<User>(It.IsAny<User>()), Times.Exactly(users.Count<User>()));
        }
        #endregion

        #region void UpdateInSearchIndex<TEntity>(TEntity model) where TEntity : class
        [Test]
        public void UpdateIndex_UpdateIndex_Return_NoException()
        {
            User user = new User();
            mockRepo.Setup(x => x.Update<User>(user));

            sut.UpdateInSearchIndex<User>(user);
        }

        [Test]
        public void UpdateIndex_UpdateIndexVerify_Return_Once()
        {
            var user = new User();
            mockRepo.Setup(x => x.Update<User>(user)).Verifiable();

            sut.UpdateInSearchIndex<User>(user);

            mockRepo.Verify(v => v.Update<User>(user), Times.Once);
        }
        #endregion

        #region void UpdateInSearchIndex<TEntity>(IEnumerable<TEntity> models) where TEntity : class
        [Test]
        public void UpdateIndex_UpdateIndexIEnumerable_Return_NoException()
        {
            mockRepo.Setup(x => x.Update<User>(It.IsAny<User>()));

            sut.UpdateInSearchIndex<User>(users);
        }

        [Test]
        public void UpdateIndex_UpdateIndexIEnumerableVerify_Return_UsersCount()
        {
            mockRepo.Setup(x => x.Update<User>(It.IsAny<User>())).Verifiable();

            sut.UpdateInSearchIndex<User>(users);

            mockRepo.Verify(v => v.Update<User>(It.IsAny<User>()), Times.Exactly(users.Count<User>()));
        }
        #endregion

        #region void ClearSearchIndex()
        [Test]
        public void ClearIndex_ClearIndex_Return_NoException()
        {
            mockRepo.Setup(x => x.Clear());

            sut.ClearSearchIndex();
        }

        [Test]
        public void ClearIndex_ClearIndexVerify_Return_Once()
        {
            mockRepo.Setup(x => x.Clear()).Verifiable();

            sut.ClearSearchIndex();

            mockRepo.Verify(v => v.Clear(), Times.Once);
        }
        #endregion

        #region void OptimizeSearchIndex()
        [Test]
        public void OptimizeIndex_OptimizeIndex_Return_NoException()
        {
            mockRepo.Setup(x => x.Optimize());

            sut.OptimizeSearchIndex();
        }

        [Test]
        public void OptimizeIndex_OptimizeIndexVerify_Return_Once()
        {
            mockRepo.Setup(x => x.Optimize()).Verifiable();

            sut.OptimizeSearchIndex();

            mockRepo.Verify(v => v.Optimize(), Times.Once);
        }
        #endregion
    }
}
