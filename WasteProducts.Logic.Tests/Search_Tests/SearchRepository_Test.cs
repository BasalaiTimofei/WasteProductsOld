using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Services;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.DataAccess.Common.Exceptions;
using System.Collections.ObjectModel;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Tests.Search_Tests
{
    public class TestProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class TestUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }

    [TestFixture]
    class SearchRepository_Test
    {
        [OneTimeSetUp]
        public void Setup()
        {
            users = new List<User>
            {
                new User { Id = "1", UserName = "user1", Email = "user1@mail.net" },
                new User { Id = "2", UserName = "user2", Email = "user2@mail.net" },
                new User { Id = "3", UserName = "user3", Email = "user3@mail.net" },
                new User { Id = "4", UserName = "user4 user", Email = "user4@mail.net" },
                new User { Id = "5", UserName = "User5 user", Email = "user5@mail.net" }
            };

            products = new List<TestProduct>
            {
                new TestProduct { Id=1, Name = "Product1 Name1", Description = "Product1 Description1"},
                new TestProduct { Id=2, Name = "Product2 Name2", Description = "Product2 Description2"},
                new TestProduct { Id=3, Name = "Product3 Name3", Description = "Product3 Description3"},
                new TestProduct { Id=4, Name = "Product4 Name4", Description = "Product4 Description4"},
                new TestProduct { Id=5, Name = "Product5 Name5", Description = "Product5 Description5"}
            };

        }

        private IEnumerable<User> users;
        private IEnumerable<TestProduct> products;
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

        #region public void Insert<TEntity>(TEntity obj) where TEntity : class
        [Test]
        public void Insert_InsertNewObject_Return_NoException()
        {
            sut = new LuceneSearchRepository();
            var user = new User();

            sut.Insert(user);
        }
        #endregion

        #region TEntity GetById<TEntity>(int id) where TEntity : class
        [Test]
        public void GetById_GetUser_Return_NoException()
        {
            var user = new User() { Id = "1", UserName = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.GetById<User>("1");
        }

        [Test]
        public void GetById_GetExistingId_Return_ObjectWithCorrectId()
        {
            var user = new User() { Id = "1", UserName = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.GetById<User>("1");

            Assert.AreEqual(user.UserName, userFromRepo.UserName);
        }

        [Test]
        public void GetById_GetWrongId_Return_ObjectAreNotEqual()
        {
            var user = new User() { Id = "1", UserName = "user1" };
            var user2 = new User() { Id = "2", UserName = "user2" };

            sut = new LuceneSearchRepository(true);
            sut.Insert(user);
            sut.Insert(user2);

            var userFromRepo = sut.GetById<User>("2");

            Assert.AreNotEqual(user.UserName, userFromRepo.UserName);
        }

        [Test]
        public void GetById_GetByNotExistingId_Return_NullObject()
        {
            var user = new User() { Id = "1", UserName = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.GetById<User>("2");

            Assert.AreEqual(null, userFromRepo);
        }
        #endregion

        #region TEntity Get<TEntity>(string keyValue, string keyField) where TEntity : class
        [Test]
        public void Get_GetUser_Return_NoException()
        {
            var user = new User() { Id = "1", UserName = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.Get<User>("user1", "login");
        }

        [Test]
        public void Get_GetExistingId_Return_ObjectWithCorrectId()
        {
            var user = new User() { Id = "1", UserName = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.Get<User>("user1", "Login");

            Assert.AreEqual(user.UserName, userFromRepo.UserName);
        }

        [Test]
        public void Get_GetWrongId_Return_ObjectAreNotEqual()
        {
            var user = new User() { Id = "1", UserName = "user1" };
            var user2 = new User() { Id = "2", UserName = "user2" };

            sut = new LuceneSearchRepository(true);
            sut.Insert(user);
            sut.Insert(user2);

            var userFromRepo = sut.Get<User>("user2", "Login");

            Assert.AreNotEqual(user.UserName, userFromRepo.UserName);
        }

        [Test]
        public void Get_GetByNotExistingId_Return_NullObject()
        {
            var user = new User() { Id = "1", UserName = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.Get<User>("user1", "not_existing_filed");

            Assert.AreEqual(null, userFromRepo);
        }
        #endregion

        #region public IEnumerable<TEntity> GetAll<TEntity>() where TEntity  :class
        [Test]
        public void GetAll_GetAll_Return_NoException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);

            var userCollectionFromRepo = sut.GetAll<User>();
        }

        [Test]
        public void GetAll_GetAll_Return_EqualCount()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);

            var userCollectionFromRepo = sut.GetAll<User>();

            Assert.AreEqual(users.Count(), userCollectionFromRepo.Count());
        }
        #endregion

        #region IEnumerable<TEntity> GetAll<TEntity>(string queryString, IEnumerable<string> searchableFields, int numResults) where TEntity : class

        [Test]
        public void GetAllParams_GetAll_Return_ArgumentException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();

            Assert.Throws<ArgumentException>(() => sut.GetAll<User>("user", queryList, -1));
        }

        [Test]
        public void GetAllParams_SearchUser_Return_EqualCount_5()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();
            queryList.Add("Login");
            queryList.Add("Email");

            var userCollectionFromRepo = sut.GetAll<User>("user", queryList, 1000);

            Assert.AreEqual(5, userCollectionFromRepo.Count());
        }

        [Test]
        public void GetAllParams_EmptyQuery_Return_EqualCount_0()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();
            queryList.Add("Login");
            queryList.Add("Email");

            //var userCollectionFromRepo = sut.GetAll<User>(String.Empty, queryList, 1000);

            //Assert.AreEqual(0, userCollectionFromRepo.Count());

            Assert.Throws<ArgumentException>(() => sut.GetAll<User>(String.Empty, queryList, 1000));
        }

        [Test]
        public void GetAllParams_EmptyFields_Return_EqualCount_0()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();

            //var userCollectionFromRepo = sut.GetAll<User>("user", queryList, 1000);

            //Assert.AreEqual(0, userCollectionFromRepo.Count());
            Assert.Throws<ArgumentException>(() => sut.GetAll<User>("user", queryList, 1000));
        }

        //не проходит
        /*[Test]
        public void GetAllParams_GetAllQuery_Return_EqualCount_5()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();
            queryList.Add("Email");

            //такой вариант не проходит по причине как в тесте выше
            var userCollectionFromRepo = sut.GetAll<User>("@mail*", queryList, 1000);

            //такой вариант не проходи, т.к. выбивает ошибку "нельзя что бы поиск начинался с * или ?". Поэтому вопрос как искать
            //слова в середине предложений и тд
            //var userCollectionFromRepo = sut.GetAll<User>("mail*", queryList, 1000);

            Assert.AreEqual(5, userCollectionFromRepo.Count());
        }*/

        [Test]
        public void GetAllParams_SearchUpperCase_Return_EqualCount_1()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();
            queryList.Add("Login");

            var userCollectionFromRepo = sut.GetAll<User>("user5", queryList, 1000);

            Assert.AreEqual(1, userCollectionFromRepo.Count());
        }
        #endregion

        //TODO: IEnumerable<TEntity> GetAll<TEntity>(string queryString, IEnumerable<string> searchableFields, ReadOnlyDictionary<string, float> boosts, int numResults) where TEntity : class;

        #region Update<TEntity>(TEntity obj) where TEntity : class
        [Test]
        public void Update_UpdateObject_Return_NoException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = users.ToList()[1];
            oldUser.UserName = "user1_new";

            sut.Update<User>(oldUser);
        }

        [Test]
        public void Update_UpdateObject_Return_EqualKeyValue()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = users.ToList()[1];
            oldUser.UserName = "user1_new";

            sut.Update<User>(oldUser);
            var updUser = sut.GetById<User>(oldUser.Id);

            Assert.AreEqual("user1_new", updUser.UserName);
        }

        [Test]
        public void Update_NotExistingObject_Return_EqualKeyValue()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);

            var oldUser = new User();

            Assert.Throws<LuceneSearchRepositoryException>(() => sut.Update<User>(oldUser));
        }

        #endregion

        #region Delete<TEntity>(TEntity obj) where TEntity : class;
        [Test]
        public void Delete_UpdateObject_Return_NoException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = users.ToList()[1];

            sut.Delete<User>(oldUser);
        }

        [Test]
        public void Delete_UpdateObject_Return_EqualKeyValue()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = users.ToList()[1];

            sut.Delete<User>(oldUser);
            var updUser = sut.GetById<User>(oldUser.Id);

            Assert.AreEqual(null, updUser);
        }

        [Test]
        public void Delete_NotExistingObject_Return_EqualKeyValue()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = new User();

            Assert.Throws<LuceneSearchRepositoryException>(() => sut.Delete<User>(oldUser));
        }
        #endregion

        #region Full-text search testing
        [Test]
        public void Get_GetAllOneWordQuery_Return_EqualCount_Not_Zero()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var product in products)
            {
                sut.Insert<TestProduct>(product);
            }
            var result = sut.GetAll<TestProduct>("product", new string[] { "Name", "Description" }, 1000);
            Assert.AreEqual(expected: 5, actual: result.Count());

            result = sut.GetAll<TestProduct>("product1", new string[] { "Name", "Description" }, 1000);
            Assert.AreEqual(expected: 1, actual: result.Count());
        }

        public void Get_GetAllOneWordQuery_Return_EqualCount_Zero()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var product in products)
            {
                sut.Insert<TestProduct>(product);
            }

            var result = sut.GetAll<TestProduct>("word1", new string[] { "Name", "Decription" }, 1000);
            Assert.AreEqual(expected: 0, actual: result.Count());

            result = sut.GetAll<TestProduct>("name1", new string[] { "Decription" }, 1000);
            Assert.AreEqual(expected: 0, actual: result.Count());
        }


        [Test]
        public void Get_GetAllMultiplyWordQuery_Return_EqualCount_Not_Zero()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var product in products)
            {
                sut.Insert<TestProduct>(product);
            }

            var result = sut.GetAll<TestProduct>("name1 word1 word2 word3", new string[] { "Name", "Decription" }, 1000);
            Assert.AreEqual(expected: 1, actual: result.Count());

            result = sut.GetAll<TestProduct>("word1 word2 NamE1 word3", new string[] { "Name", "Decription" }, 1000);
            Assert.AreEqual(expected: 1, actual: result.Count());

            result = sut.GetAll<TestProduct>("word1 description1", new string[] { "Name", "Description" }, 1000);
            Assert.AreEqual(expected: 1, actual: result.Count());

            result = sut.GetAll<TestProduct>("name1 description2", new string[] { "Name", "Description" }, 1000);
            Assert.AreEqual(expected: 2, actual: result.Count());

            result = sut.GetAll<TestProduct>("Word1 Word2 NamE1 DescriptioN1 NamE2 ProducT2 Word3 Word4", new string[] { "Description" }, 1000);
            Assert.AreEqual(expected: 2, actual: result.Count());
        }

        [Test]
        public void Get_GetAllMultiplyWordQuery_WrongField_Return_EqualCount_Not_Zero()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var product in products)
            {
                sut.Insert<TestProduct>(product);
            }
            var result = sut.GetAll<TestProduct>("product", new string[] { "NonExistentField" }, 1000);
            Assert.AreEqual(expected: 0, actual: result.Count());

        }

        [Test]
        public void Get_GetAllMultiplyWordQuery_With_Boost_Return_EqualCount_Not_Zero()
        {

            sut = new LuceneSearchRepository(true);
            foreach (var product in products)
            {
                sut.Insert<TestProduct>(product);
            }

            var boostValues = new Dictionary<string, float>();
            boostValues.Add("Name", 1.0f);
            boostValues.Add("Description", 1.0f);
            var result = sut.GetAll<TestProduct>("product", new string[] { "Name", "Description" }, new ReadOnlyDictionary<string, float>(boostValues), 1000);
            Assert.AreEqual(expected: 5, actual: result.Count());
        }

        #endregion

    }

}