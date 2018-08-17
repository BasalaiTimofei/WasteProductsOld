﻿using Moq;
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
using WasteProducts.DataAccess.Common.Exceptions;

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
                new User { Id = 4, Login = "user4 user", Email = "user4@mail.net" },
                new User { Id = 5, Login = "User5 user", Email = "user5@mail.net" }
            };

            mockRepo = new Mock<ISearchRepository>();            
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
            var user = new User() { Id = 1, Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.GetById<User>(1);           
        }

        [Test]
        public void GetById_GetExistingId_Return_ObjectWithCorrectId()
        {
            var user = new User() { Id = 1, Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.GetById<User>(1);

            Assert.AreEqual(user.Login, userFromRepo.Login);            
        }

        [Test]
        public void GetById_GetWrongId_Return_ObjectAreNotEqual()
        {
            var user = new User() { Id = 1, Login = "user1" };
            var user2 = new User() { Id = 2, Login = "user2" };

            sut = new LuceneSearchRepository(true);
            sut.Insert(user);
            sut.Insert(user2);

            var userFromRepo = sut.GetById<User>(2);

            Assert.AreNotEqual(user.Login, userFromRepo.Login);
        }

        [Test]
        public void GetById_GetByNotExistingId_Return_NullObject()
        {
            var user = new User() { Id = 1, Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.GetById<User>(2);

            Assert.AreEqual(null, userFromRepo);            
        }
        #endregion

        #region TEntity Get<TEntity>(string keyValue, string keyField) where TEntity : class
        [Test]
        public void Get_GetUser_Return_NoException()
        {
            var user = new User() { Id = 1, Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.Get<User>("user1", "login");
        }

        [Test]
        public void Get_GetExistingId_Return_ObjectWithCorrectId()
        {
            var user = new User() { Id = 1, Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.Get<User>("user1", "Login");

            Assert.AreEqual(user.Login, userFromRepo.Login);
        }

        [Test]
        public void Get_GetWrongId_Return_ObjectAreNotEqual()
        {
            var user = new User() { Id = 1, Login = "user1" };
            var user2 = new User() { Id = 2, Login = "user2" };

            sut = new LuceneSearchRepository(true);
            sut.Insert(user);
            sut.Insert(user2);

            var userFromRepo = sut.Get<User>("user2", "Login");

            Assert.AreNotEqual(user.Login, userFromRepo.Login);
        }

        [Test]
        public void Get_GetByNotExistingId_Return_NullObject()
        {
            var user = new User() { Id = 1, Login = "user1" };
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
        public void GetAllParams_GetAll_Return_NoException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();

            var userCollectionFromRepo = sut.GetAll<User>("user", queryList, 1000);
        }

        [Test]
        public void GetAllParams_GetAll_Return_LuceneSearchRepositoryException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();           

            Assert.Throws<LuceneSearchRepositoryException>(() => sut.GetAll<User>("user", queryList, -1));
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

            var userCollectionFromRepo = sut.GetAll<User>("user*", queryList, 1000);

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

            Assert.Throws<LuceneSearchRepositoryException>(() => sut.GetAll<User>(String.Empty, queryList, 1000));
        }

        [Test]
        public void GetAllParams_EmptyFields_Return_EqualCount_0()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();            

            var userCollectionFromRepo = sut.GetAll<User>("user", queryList, 1000);

            Assert.AreEqual(0, userCollectionFromRepo.Count());
        }

        //не проходит
        [Test]
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
        }

        //не проходит. нужно убрать зависимость от регистра
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
            oldUser.Login = "user1_new";

            sut.Update<User>(oldUser);
        }

        [Test]
        public void Update_UpdateObject_Return_EqualKeyValue()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = users.ToList()[1];
            oldUser.Login = "user1_new";

            sut.Update<User>(oldUser);
            var updUser = sut.GetById<User>(oldUser.Id);

            Assert.AreEqual("user1_new", updUser.Login);
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

    }
}
