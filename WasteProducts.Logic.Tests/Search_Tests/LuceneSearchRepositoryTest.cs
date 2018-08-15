using System;
using System.Collections.Generic;
using NUnit.Framework;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.Logic.Common.Models.Users;
using FluentAssertions;
using System.Linq;

namespace WasteProducts.Logic.Tests.Search_Tests
{
    [TestFixture]
    public class LuceneSearchRepositoryTest
    {

        //private IEnumerable<User> users;
        LuceneSearchRepository sut;

        [OneTimeSetUp]
        public void Setup()
        {
            sut = new LuceneSearchRepository();
        }

        [Test]
        public void TestGetFromRepositoryByField()
        {
            sut.Clear();

            var user1 = new User { Id = 1, Login = "user1", Email = "user1@mail.net" };
            var user2 = new User { Id = 2, Login = "user2", Email = "user2@mail.net" };
            sut.Insert<User>(user1);
            sut.Insert<User>(user2);
            var resultUser1 = sut.Get<User>("user1", "Login");
            var resultUser2 = sut.Get<User>("user2", "Login");
            resultUser1.Should().BeEquivalentTo(user1);
            resultUser2.Should().BeEquivalentTo(user2);
        }

        [Test]
        public void TestGetFromRepositoryById()
        {

            sut.Clear();

            var user1 = new User { Id = 1, Login = "user1", Email = "user1@mail.net" };
            var user2 = new User { Id = 2, Login = "user2", Email = "user2@mail.net" };
            sut.Insert<User>(user1);
            sut.Insert<User>(user2);
            var resultUser1 = sut.GetById<User>(1);
            var resultUser2 = sut.GetById<User>(2);
            resultUser1.Should().BeEquivalentTo(user1);
            resultUser2.Should().BeEquivalentTo(user2);
        }

        [Test]
        public void TestGetAllEntitiesFromRepository()
        {

            sut.Clear();

            var user1 = new User { Id = 1, Login = "user1", Email = "user1@mail.net" };
            var user2 = new User { Id = 2, Login = "user2", Email = "user2@mail.net" };
            var user3 = new User { Id = 3, Login = "user3", Email = "user3@mail.net" };
            var user4 = new User { Id = 4, Login = "user4", Email = "user4@mail.net" };
            sut.Insert<User>(user1);
            sut.Insert<User>(user2);
            sut.Insert<User>(user3);
            sut.Insert<User>(user4);
            var resultList = sut.GetAll<User>().ToList();
            Assert.AreEqual(4, resultList.Count);
        }
    }
}
