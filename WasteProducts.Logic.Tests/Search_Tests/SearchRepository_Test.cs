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
using WasteProducts.DataAccess.Repositories;

namespace WasteProducts.Logic.Tests.Search_Tests
{
    [TestFixture]
    class SearchRepository_Test
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
            //sut = new LuceneSearchRepository(mockRepo.Object);
        }

        private IEnumerable<User> users;
        private Mock<ISearchRepository> mockRepo;
        private ISearchRepository sut;

        #region public LuceneSearchRepository()
        [Test]
        public void Ctr_NewRepository_Return_NoException()
        {
            sut = new LuceneSearchRepository();
        }

        [Test]
        public void Ctr_NewRepositoryWithTrue_Return_NoException()
        {
            sut = new LuceneSearchRepository(true);
        }

        [Test]
        public void Ctr_NewRepositoryWithFalse_Return_NoException()
        {
            sut = new LuceneSearchRepository(false);
        }
        #endregion
    }
}
