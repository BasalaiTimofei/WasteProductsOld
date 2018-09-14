using System.Collections.Generic;
using System.Linq;
using Bogus;
using NLog;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.Diagnostic;

namespace WasteProducts.Logic.Services
{
    /// <summary>
    /// Implementation of ITestModelsService using <c>Bogus</c> generator
    /// </summary>
    public class TestModelsService : ITestModelsService
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">NLog logger</param>
        public TestModelsService(ILogger logger)
        {
            _logger = logger;
        }
        
        /// <inheritdoc />
        public User GenerateUser(string email = null, string userName = null)
        {
            return new Faker<User>()
                .RuleFor(user => user.UserName,faker => userName ?? faker.Person.UserName)

                // TODO: Доделать

                .FinishWith((faker, user) => _logger.Debug($"Created User: {user}"))
                .Generate();
        }

        /// <inheritdoc />
        public Product GenerateProduct(Barcode barcode, Category category)
        {
            return new Faker<Product>()
                .RuleFor(product => product.Name, faker => barcode.ProductName)
                .RuleFor(product => product.Composition, faker => $"Product made from {faker.Commerce.ProductMaterial()}")

                .RuleFor(product => product.Barcode, faker => barcode)
                .RuleFor(product => product.Category, faker => category)

                .FinishWith((faker, product) => _logger.Debug($"Created Product: {product}"))
                .Generate();
        }

        /// <inheritdoc />
        public Category GenerateCategory()
        {
            return new Faker<Category>()
                .RuleFor(category => category.Name, faker => faker.Commerce.Categories(1).First())
                .RuleFor(category => category.Description, faker => $"This Category is subcategory of {faker.Commerce.Categories(1).FirstOrDefault()}.")
                
                .FinishWith((faker, category) => _logger.Debug($"Created Category: {category}"))
                .Generate();
        }

        /// <inheritdoc />
        public Barcode GenerateBarcode()
        {
            return new Faker<Barcode>()
                .RuleFor(barcode => barcode.Code, faker => faker.Random.ReplaceNumbers("############"))

                .RuleFor(barcode => barcode.Brend, faker => faker.Company.CompanyName())
                .RuleFor(barcode => barcode.Country, faker => faker.Address.Country())

                .RuleFor(barcode => barcode.ProductName, faker => faker.Commerce.ProductName())
                .RuleFor(barcode => barcode.Weight, faker => faker.Random.Double(0.1, 100))
                .RuleFor(barcode => barcode.Type, faker => faker.Random.Bool() ? "UPC-B" : "UPC-C")

                .FinishWith((faker, barcode) => _logger.Debug($"Created Barcode: {barcode}"))
                .Generate();
        }
    }
}