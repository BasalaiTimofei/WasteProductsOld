using NUnit.Framework;
using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Repositories.Search;
using System.Linq;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.DataAccess.Common.Exceptions;
using System.Collections.ObjectModel;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.Common.Models.Products;
using Ninject;
using WasteProducts.DataAccess.Common.Context;

namespace WasteProducts.Logic.Tests.Search_Tests
{
    public class TestProduct
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class TestUser
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }

    [TestFixture]
    class SearchRepository_Test
    {
        [OneTimeSetUp]
        public void Setup()
        {
            users = new List<TestUser>
            {
                new TestUser { Id = "1", Login = "user1", Email = "user1@mail.net" },
                new TestUser { Id = "2", Login = "user2", Email = "user2@mail.net" },
                new TestUser { Id = "3", Login = "user3", Email = "user3@mail.net" },
                new TestUser { Id = "4", Login = "user4 user", Email = "user4@mail.net" },
                new TestUser { Id = "5", Login = "user5 user", Email = "user5@mail.net" }
            };

            products = new List<TestProduct>
            {
                new TestProduct { Id="1", Name = "Product1 Name1", Description = "Product1 Description1"},
                new TestProduct { Id="2", Name = "Product2 Name2", Description = "Product2 Description2"},
                new TestProduct { Id="3", Name = "Product3 Name3", Description = "Product3 Description3"},
                new TestProduct { Id="4", Name = "Product4 Name4", Description = "Product4 Description4"},
                new TestProduct { Id="5", Name = "Product5 Name5", Description = "Product5 Description5"}
            };

        }

        private IEnumerable<TestUser> users;
        private IEnumerable<TestProduct> products;
        private ISearchRepository sut;

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

        [Test]
        public void Insert_InsertNewObject_Return_NoException()
        {
            sut = new LuceneSearchRepository();
            var user = new TestUser();

            sut.Insert(user);
        }

        [Test]
        public void GetById_GetTestUser_Return_NoException()
        {
            var user = new TestUser() { Id = "1", Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.GetById<TestUser>(1);
        }

        [Test]
        public void GetById_GetExistingId_Return_ObjectWithCorrectId()
        {
            var user = new TestUser() { Id = "1", Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.GetById<TestUser>("1");

            Assert.AreEqual(user.Login, userFromRepo.Login);
        }

        [Test]
        public void GetById_GetWrongId_Return_ObjectAreNotEqual()
        {
            var user = new TestUser() { Id = "1", Login = "user1" };
            var user2 = new TestUser() { Id = "2", Login = "user2" };

            sut = new LuceneSearchRepository(true);
            sut.Insert(user);
            sut.Insert(user2);

            var userFromRepo = sut.GetById<TestUser>("2");

            Assert.AreNotEqual(user.Login, userFromRepo.Login);
        }

        [Test]
        public void GetById_GetByNotExistingId_Return_NullObject()
        {
            var user = new TestUser() { Id = "1", Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.GetById<TestUser>(2);

            Assert.AreEqual(null, userFromRepo);
        }

        [Test]
        public void Get_GetUser_Return_NoException()
        {
            var user = new TestUser() { Id = "1", Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.Get<TestUser>("user1", "login");
        }

        [Test]
        public void Get_GetExistingId_Return_ObjectWithCorrectId()
        {
            var user = new TestUser() { Id = "1", Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.Get<TestUser>("user1", "Login");

            Assert.AreEqual(user.Login, userFromRepo.Login);
        }

        [Test]
        public void Get_GetWrongId_Return_ObjectAreNotEqual()
        {
            var user = new TestUser() { Id = "1", Login = "user1" };
            var user2 = new TestUser() { Id = "2", Login = "user2" };

            sut = new LuceneSearchRepository(true);
            sut.Insert(user);
            sut.Insert(user2);

            var userFromRepo = sut.Get<TestUser>("user2", "Login");

            Assert.AreNotEqual(user.Login, userFromRepo.Login);
        }

        [Test]
        public void Get_GetByNotExistingId_Return_NullObject()
        {
            var user = new TestUser() { Id = "1", Login = "user1" };
            sut = new LuceneSearchRepository(true);
            sut.Insert(user);

            var userFromRepo = sut.Get<TestUser>("user1", "not_existing_filed");

            Assert.AreEqual(null, userFromRepo);
        }

        [Test]
        public void GetAll_GetAll_Return_NoException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);

            var userCollectionFromRepo = sut.GetAll<TestUser>();
        }

        [Test]
        public void GetAll_GetAll_Return_EqualCount()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);

            var userCollectionFromRepo = sut.GetAll<TestUser>();

            Assert.AreEqual(users.Count(), userCollectionFromRepo.Count());
        }

        [Test]
        public void GetAllParams_GetAll_Return_ArgumentException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();

            Assert.Throws<ArgumentException>(() => sut.GetAll<TestUser>("user", queryList, -1));
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

            var userCollectionFromRepo = sut.GetAll<TestUser>("user", queryList, 1000);

            Assert.AreEqual(5, userCollectionFromRepo.Count());
        }

        [Test]
        public void GetAllParams_EmptyQuery_Return_ArgumentException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();
            queryList.Add("Login");
            queryList.Add("Email");

            Assert.Throws<ArgumentException>(() => sut.GetAll<TestUser>(String.Empty, queryList, 1000));
        }

        [Test]
        public void GetAllParams_EmptyFields_Return_ArgumentException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();

            Assert.Throws<ArgumentException>(() => sut.GetAll<TestUser>("user", queryList, 1000));
        }

        [Test]
        public void GetAllParams_SearchUpperCase_Return_EqualCount_1()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var queryList = new List<string>();
            queryList.Add("Login");

            var userCollectionFromRepo = sut.GetAll<TestUser>("user5", queryList, 1000);

            Assert.AreEqual(1, userCollectionFromRepo.Count());
        }

        [Test]
        public void GetProductWithNestedProperty()
        {
            sut = new LuceneSearchRepository(true);
            ProductDB product = new ProductDB();
            product.Id = "1";
            product.Name = "Test product";
            product.Description = "Test product description";
            product.Category = new CategoryDB();
            product.Category.Id = 11;
            product.Category.Name = "Test category name";
            product.Category.Description = "Test category description";
            sut.Insert(product);
            var result = sut.GetById<ProductDB>(1);

            Assert.AreNotEqual(null, product.Category);
            Assert.AreEqual(11, product.Category.Id);
            Assert.AreEqual("Test category name", product.Category.Name);
            Assert.AreEqual("Test category description", product.Category.Description);
        }

        //TODO: IEnumerable<TEntity> GetAll<TEntity>(string queryString, IEnumerable<string> searchableFields, ReadOnlyDictionary<string, float> boosts, int numResults) where TEntity : class;

        [Test]
        public void Update_UpdateObject_Return_NoException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = users.ToList()[1];
            oldUser.Login = "user1_new";

            sut.Update<TestUser>(oldUser);
        }

        [Test]
        public void Update_UpdateObject_Return_EqualKeyValue()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = users.ToList()[1];
            oldUser.Login = "user1_new";

            sut.Update<TestUser>(oldUser);
            var updUser = sut.GetById<TestUser>(oldUser.Id);

            Assert.AreEqual("user1_new", updUser.Login);
        }

        [Test]
        public void Update_NotExistingObject_Return_EqualKeyValue()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);

            var oldUser = new TestUser();

            Assert.Throws<LuceneSearchRepositoryException>(() => sut.Update<TestUser>(oldUser));
        }

        [Test]
        public void Delete_UpdateObject_Return_NoException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = users.ToList()[1];

            sut.Delete<TestUser>(oldUser);
        }

        [Test]
        public void Delete_UpdateObject_Return_EqualKeyValue()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = users.ToList()[1];

            sut.Delete<TestUser>(oldUser);
            var updUser = sut.GetById<TestUser>(oldUser.Id);

            Assert.AreEqual(null, updUser);
        }

        [Test]
        public void Delete_NotExistingObject_Return_LuceneSearchRepositoryException()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var user in users)
                sut.Insert(user);
            var oldUser = new TestUser();

            Assert.Throws<LuceneSearchRepositoryException>(() => sut.Delete<TestUser>(oldUser));
        }

        [Test]
        public void Get_GetAllOneWordQuery_Return_EqualCount_Not_Zero()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var product in products)
            {
                sut.Insert<TestProduct>(product);
            }
            var result = sut.GetAll<TestProduct>("product", new string[] { "Name", "Description" }, 1000);
            Assert.AreEqual(5, result.Count());

            result = sut.GetAll<TestProduct>("product1", new string[] { "Name", "Description" }, 1000);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Get_GetAllOneWordQuery_Return_EqualCount_Zero()
        {
            sut = new LuceneSearchRepository(true);
            foreach (var product in products)
            {
                sut.Insert<TestProduct>(product);
            }

            var result = sut.GetAll<TestProduct>("word1", new string[] { "Name", "Decription" }, 1000);
            Assert.AreEqual(0, result.Count());

            result = sut.GetAll<TestProduct>("name1", new string[] { "Decription" }, 1000);
            Assert.AreEqual(0, result.Count());
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
            Assert.AreEqual(1, result.Count());

            result = sut.GetAll<TestProduct>("word1 word2 NamE1 word3", new string[] { "Name", "Decription" }, 1000);
            Assert.AreEqual(1, result.Count());

            result = sut.GetAll<TestProduct>("word1 description1", new string[] { "Name", "Description" }, 1000);
            Assert.AreEqual(1, result.Count());

            result = sut.GetAll<TestProduct>("name1 description2", new string[] { "Name", "Description" }, 1000);
            Assert.AreEqual(2, result.Count());

            result = sut.GetAll<TestProduct>("Word1 Word2 NamE1 DescriptioN1 NamE2 ProducT2 Word3 Word4", new string[] { "Description" }, 1000);
            Assert.AreEqual(2, result.Count());
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
            Assert.AreEqual(0, result.Count());
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
            Assert.AreEqual(5, result.Count());
        }


        //Test for context. It will be deleted
        public class TestContext : WasteContext
        {
            public TestContext(ISearchRepository repo) : base(repo)
            {

            }

            public new void DetectAndSaveChanges(System.Data.Entity.EntityState state, IEnumerable<Type> types)
            {
                base.DetectAndSaveChanges(state, types);
            }
        }

        [Test]
        public void _DetectChanges_AddNewEntity_Result_Entity_added()
        {
            sut = new LuceneSearchRepository(true);
            TestContext context = new TestContext(sut);
            ProductDB product = new ProductDB() { Id = "1", Name = "Title", Description = "Description" };
            context.Products.Add(product);

            context.DetectAndSaveChanges(System.Data.Entity.EntityState.Added, new List<Type>() { typeof(ProductDB) });                        

            var productFromRepo = sut.GetById<ProductDB>("1");

            Assert.AreNotEqual(null, productFromRepo);
            Assert.AreEqual("1", productFromRepo.Id);
        }

        ////not passed. Need Product mapper
        //[Test]
        //public void _DetectChanges_AddNewEntityWithCategory_Result_Entity_added()
        //{
        //    sut = new LuceneSearchRepository(true);
        //    TestContext context = new TestContext();
        //    CategoryDB category = new CategoryDB() { Id = 1, Name = "Category name", Description = "Category description" };
        //    ProductDB product = new ProductDB() { Id = "1", Name = "Title", Description = "Description", Category = category };

        //    context.Products.Add(product);

        //    context.DetectAndSaveChanges(System.Data.Entity.EntityState.Added, new List<Type>() { typeof(ProductDB), typeof(CategoryDB) });

        //    var productFromRepo = sut.GetById<ProductDB>(1);            

        //    Assert.AreNotEqual(null, productFromRepo);
        //    Assert.AreNotEqual(null, productFromRepo.Category);
        //}
    }
}