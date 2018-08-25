using System;
using System.ComponentModel;
using Moq;
using NUnit.Framework;
using NSubstitute;
using FluentAssertions;
using NSubstitute.Extensions;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Services;
using AutoMapper;
using WasteProducts.Logic.Mappings;

namespace WasteProducts.Logic.Tests
{
    #region NSubstitute description
    //What is NSubstitute? http://nsubstitute.github.io/
    //NSubstitute is designed as a friendly substitute for .NET mocking libraries.
    //It is an attempt to satisfy our craving for a mocking library with a succinct syntax that helps 
    //us keep the focus on the intention of our tests, rather than on the configuration of our test doubles.
    //We've tried to make the most frequently required operations obvious and easy to use, keeping less usual
    //scenarios discoverable and accessible, and all the while maintaining as much natural language as possible.
    #endregion

    #region FluentAssertions descreiption
    //https://fluentassertions.com/documentation/
    //As you may have noticed, the purpose of this open-source project is to not only be the best 
    //assertion framework in the.NET realm, but to also demonstrate high-quality code. We heavily 
    //practice Test Driven Development and one of the promises TDD makes is that unit tests can be
    //treated as your API’s documentation. So although you are free to go through the many examples
    //here, please consider to analyze the many unit tests. (developer's comment)
    #endregion

    [TestFixture]
    public class ProductServiceTests
    {
        private Barcode _barcode;
        private IProductService _productSrvc;
        private Mock<IProductRepository> _repo;
        private ProductDB _db;
        private readonly IMapper _mapper;


        [SetUp]
        public void Init()
        {
            _barcode = new Barcode { Code = "125478569", Brand = "Mars", Country = "Russia", Id = "51515" };
            _repo = new Mock<IProductRepository>();
            _db = Substitute.For<ProductDB>();

            var c = new MapperConfiguration(cfg=>cfg.AddProfile(new ProductProfile()));
            var mapper = new Mapper(c);
            _productSrvc = new ProductService(_repo.Object, mapper);

        }

        [TearDown]
        public void Init1()
        {
            Mapper.Reset();

        }
        //[Test]
        //public void AddingProductByBarcore_BarcodeIsNotNull()
        //{
        //    var _repoMoq = new Mock<IProductRepository>();
        //    _repoMoq.Setup(s => s.Add(_productDb)).Verifiable();
        //    var productService = new ProductService(_repoMoq.Object);
        //    productService.AddByBarcode(_barcode);
        //    _repoMoq.Verify(s => s.Add(_productDb));
        //}

        [Test]
        public void AddingProductByBarcore_BarcodeIsNotNull()
        {

            var b = new Barcode();
            var isSuccess = _productSrvc.AddByBarcode(b);
            isSuccess.Should().BeTrue();
        }

        [Test]
        public void AddingProductByBarcore_IfBarcodesAreEqualShouldNotBeAdded()
        {
            var barc1 = new Barcode
            {
                Code = "45376896782",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var barc2 = new Barcode
            {
                Code = "863863896745",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };

            var result1 = _productSrvc.AddByBarcode(barc1);
            var result2 = _productSrvc.AddByBarcode(barc2);

            Assert.AreNotSame(result1,result2);
        }

        [Test]
        public void AddingProductByBarcore_SuccessfullyAdded()
        {
            var isSuccess = _productSrvc.AddByBarcode(_barcode);
           
            Assert.That(isSuccess);
        }

        [Test]
        public void AddingProductByName_NameIsNull()
        {
            var goal = _productSrvc.AddByName(null);

            goal.Should().BeTrue();
        }

        [Test]
        public void AddingProductByName_SuccessfullyAddedProduct()
        {
            var name = "Choco";


            var result = _productSrvc.AddByName(name);
            var prod = new Product
            {
                Name = "Choco"
            };
           
            result.Should().Be(true).And.Should().ReceivedCalls();
        }

        [Test]
        public void AddingProductByName_WasNotAddedProduct()
        {
            var chocolate = "Chocolate";
            var goal = _productSrvc.AddByName(chocolate);

            goal.Should().BeFalse();
        }

        [Test]
        public void AddingProductByName_IfProductsNameMeetSameNameInAnotherBarcode()
        {
            var barc1 = new Barcode();
            var barc2 = new Barcode();

            var prod1 = new Product
            {
                Name = "ice cream"
            };
            var prod2 = new Product
            {
                Name = "ice cream with vanilla flavour"
            };

            barc1.Code.Should().BeSameAs(barc2.Code);
            barc1.Id.Should().BeSameAs(barc2.Id);
            prod2.Name.Should().Contain("ice")
                .And.NotBeSameAs(prod1.Name);

            var result = _productSrvc.AddByName(prod2.Name);

            result.Should().BeTrue();
        }

        [Test]
        public void DeletingProductByBarcore_BarcodeIsNotNull_AndProductSuccessfullyDeleted()
        {
            var result = _productSrvc.DeleteByBarcode(_barcode);

            _barcode.Should().NotBe(null);
            result.Should().BeTrue();
        }
        [Test]
        public void DeletingProductByName_IfProductsNamesAreSame()
        {
            var barc1 = new Barcode
            {
                Code = "45376896782",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var barc2 = new Barcode
            {
                Code = "863863896745",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var prod1 = new Product { Name = "Red Cherry", Barcode = barc1 };
            var prod2 = new Product { Name = "Red Cherry", Barcode = barc2 };

            barc1.ProductName.Should().BeEquivalentTo(barc2.ProductName);
            prod1.Name.Should().BeSameAs(barc1.ProductName);
            prod2.Name.Should().BeSameAs(barc2.ProductName);
            barc1.Code.Should().NotBeSameAs(barc2.Code);
            barc1.Id.Should().NotBeSameAs(barc2.Id);

            var result = _productSrvc.DeleteByName(prod1.Name);
            prod1.Name.Should().BeSameAs(prod2.Name);

            result.Should().BeTrue();
        }
        [Test]
        public void DeletingProductByName_SuccessfullyDeletedProduct()
        {
            var prodGummy = "My favorite gummy";

            var result = _productSrvc.DeleteByName(prodGummy);

            prodGummy.Should().BeEquivalentTo("MY FAVORITE GUMMY")
                .And.NotBeNullOrWhiteSpace()
                .And.NotBeEmpty();
            result.Should().Be(true);
        }
    }
}

//[TestFixture]
//public class UserServiceTest
//{
//    private Mock<IUserRepository> userRepoMock;
//    private Mock<IMailService> mailServiceMock;
//    private UserService userService;

//    [OneTimeSetUp]
//    public void TestFixtureSetup()
//    {
//        mailServiceMock = new Mock<IMailService>();
//        userRepoMock = new Mock<IUserRepository>();
//        userService = new UserService(mailServiceMock.Object, userRepoMock.Object);

//        Mapper.Initialize(cfg =>
//        {
//            cfg.AddProfile(new UserProfile()); ;
//        });
//    }

//    [OneTimeTearDown]
//    public void TestFixtureTearDown()
//    {
//        Mapper.Reset();
//    }

//    [Test]
//    public void UserServiceTest_01_RegisterAsync_Password_and_Confirmation_Doesnt_Match_Returns_Null()
//    {
//        // arrange
//        var validEmail = "validEmail@gmail.com";
//        mailServiceMock.Setup(a => a.IsValidEmail(validEmail)).Returns(true);
//        var password = "password";
//        var passwordConfirmationDoesntMatch = "doesn't match";
//        var userNameValid = "validUserName";
//        User expected = null;

//        // act
//        var actual = userService.RegisterAsync(validEmail,
//            userNameValid, password,
//            passwordConfirmationDoesntMatch)
//            .GetAwaiter().GetResult();

//        // assert
//        Assert.AreEqual(expected, actual);
//    }

//    [Test]
//    public void UserServiceTest_02_RegisterAsync_Email_Is_Not_Valid_Returns_Null()
//    {
//        // arrange
//        var invalidEmail = "invalidEmail@gmail.com";
//        mailServiceMock.Setup(a => a.IsValidEmail(invalidEmail)).Returns(false);
//        var password = "password";
//        var passwordConfirmationMatch = "password";
//        var userNameValid = "validUserName";
//        User expected = null;

//        // act
//        var actual = userService.RegisterAsync(invalidEmail,
//            userNameValid, password, passwordConfirmationMatch)
//            .GetAwaiter().GetResult();

//        // assert
//        Assert.AreEqual(expected, actual);
//    }

//    [Test]
//    public void UserServiceTest_03_RegisterAsync_Confirmation_and_Email_Invalid_Returns_Null()
//    {
//        // arrange
//        var invalidEmail = "invalidEmail@gmail.com";
//        mailServiceMock.Setup(a => a.IsValidEmail(invalidEmail)).Returns(false);
//        var password = "password";
//        var passwordConfirmationDoesntMatch = "doesn't match";
//        var userNameValid = "validUserName";
//        User expected = null;

//        // act
//        var actual = userService.RegisterAsync(invalidEmail,
//            userNameValid, password, passwordConfirmationDoesntMatch)
//            .GetAwaiter().GetResult();

//        // assert
//        Assert.AreEqual(expected, actual);
//    }

//    [Test]
//    public void UserServiceTest_04_RegisterAsync_Successful_Register_Returns_Task_User()
//    {
//        // arrange
//        var validEmail = "validEmail@gmail.com";
//        mailServiceMock.Setup(a => a.IsValidEmail(validEmail)).Returns(true);
//        var password = "password";
//        var passwordConfirmationMatch = "password";
//        var userNameValid = "validUserName";
//        var expectedEmail = validEmail;
//        var expectedPassword = password;
//        var expectedUserName = userNameValid;

//        User expectedUser = new User()
//        {
//            Email = expectedEmail,
//            Password = expectedPassword,
//            UserName = expectedUserName
//        };

//        userRepoMock.Setup(a => a.AddAsync(It.IsAny<UserDB>())).Returns(Task.CompletedTask);
//        userRepoMock.Setup(b => b.Select(It.Is<string>(c => c == validEmail),
//            It.IsAny<bool>())).Returns(Mapper.Map<UserDB>(expectedUser));

//        // act
//        var actualUser = userService.RegisterAsync(validEmail,
//            userNameValid, password, passwordConfirmationMatch)
//            .GetAwaiter().GetResult();

//        // assert
//        Assert.AreEqual(expectedUser.Id, actualUser.Id);
//        Assert.AreEqual(expectedEmail, actualUser.Email);
//        Assert.AreEqual(expectedPassword, actualUser.Password);
//        Assert.AreEqual(expectedUserName, actualUser.UserName);
//    }


//}
