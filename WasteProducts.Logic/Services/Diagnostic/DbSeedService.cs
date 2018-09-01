using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions;
using NLog;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.Diagnostic;

namespace WasteProducts.Logic.Services
{
    /// <inheritdoc />
    public class DbSeedService : IDbSeedService
    {
        private readonly IDbServiceFactory _dbServiceFactory;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbServiceFactory">Service factory, for seeding in database through services.</param>
        /// <param name="logger">NLog logger</param>
        public DbSeedService(IDbServiceFactory dbServiceFactory, ILogger logger)
        {
            _dbServiceFactory = dbServiceFactory;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<bool> SeedAsync(bool useTestData)
        {
            await SeedPermanentDataAsync();

            if (useTestData)
            {
                await SeedTestDataAsync();
            }

            return true;
        }

        /// <summary>
        /// Seeds permanent data to database
        /// </summary>
        /// <returns>Task</returns>
        private async Task SeedPermanentDataAsync()
        {
            //using (var roleService = _dbServiceFactory.CreateRoleService())
            //{

            //    string adminRoleName = "Admin";

            //    if (await roleService.FindByNameAsync(adminRoleName) == null)
            //    {
            //        Create Admin role
            //        var role = new UserRole() { Name = adminRoleName };
            //        await roleService.CreateAsync(new UserRole());
            //    }

            //    using (var userService = _dbServiceFactory.CreateUserService())
            //    {

            //        Create user for administrator
            //        User user = GetUserGenerator("Admin@gmail.com", "Admin", "Admin").Generate();
            //        User admin = await userService.RegisterAsync(user.Email, user.UserName, user.Password, user.Password);

            //        Add user to Role Admin
            //    if (admin != null)
            //        {
            //            await userService.AddToRoleAsync(admin, adminRoleName);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Seeds test data to database
        /// </summary>
        /// <returns>Task</returns>
        private async Task SeedTestDataAsync()
        {
            //using (var userService = _dbServiceFactory.CreateUserService())
            //{
            //    using (var productService = _dbServiceFactory.CreateProductService())
            //    {
            //        int testUsersCount = 10;
            //        var users = GetUserGenerator().Generate(testUsersCount);

            //        for (int i = 0; i < users.Count; i++)
            //        {
            //            var user = users[i];
            //            user = await userService.RegisterAsync(user.Email, user.UserName, user.Password, user.Password);

            //            //TODO ADD product to user
            //            var userProductCount = new Faker().Random.Int(1, 10);

            //            var userProducts= GetProductGenerator(,).Generate(userProductCount)

            //            productService.Add
            //        }
            //    }
            //}
        }



        #region Generators

        private Faker<User> GetUserGenerator(string email = null, string userName = null, string password = null)
        {
            var fakerForUser = new Faker<User>()
                .RuleFor(user => user.Email, faker => email ?? faker.Person.Email)
                .RuleFor(user => user.UserName, faker => userName ?? faker.Person.UserName)
                .RuleFor(user => user.Password, faker => password ?? faker.Internet.Password())

                .FinishWith((faker, user) => _logger.Debug($"Created User: {user}"));

            return fakerForUser;
        }

        private Faker<Product> GetProductGenerator(Barcode barcode, IEnumerable<Category> categories)
        {
            var fakerForProduct = new Faker<Product>()
                .RuleFor(product => product.Name, faker => barcode.ProductName)
                .RuleFor(product => product.Description, faker => $"Product made from {faker.Commerce.ProductMaterial()}")
                .RuleFor(product => product.Price, faker => decimal.Parse(faker.Commerce.Price()))
                .RuleFor(product => product.Category, faker => faker.PickRandom(categories))
                .RuleFor(product => product.Barcode, faker => barcode)

                .FinishWith((faker, product) => _logger.Debug($"Created Product: {product}"));

            return fakerForProduct;
        }

        private Faker<Category> GetCategoryGenerator()
        {
            var fakerForCategory = new Faker<Category>()
                .RuleFor(category => category.Name, faker => faker.Commerce.Categories(1).FirstOrDefault())
                .RuleFor(category => category.Description, faker => $"This Category is subcategory of {faker.Commerce.Categories(1).FirstOrDefault()}.")

                .FinishWith((faker, category) => _logger.Debug($"Created Category: {category}"));

            return fakerForCategory;
        }

        private Faker<Barcode> GetBarcodeGenerator()
        {
            var fakerForBarcode = new Faker<Barcode>()
                .RuleFor(barcode => barcode.Code, faker => faker.Random.ReplaceNumbers("############"))

                .RuleFor(barcode => barcode.Brend, faker => faker.Company.CompanyName())
                .RuleFor(barcode => barcode.Country, faker => faker.Address.Country())

                .RuleFor(barcode => barcode.ProductName, faker => faker.Commerce.ProductName())
                .RuleFor(barcode => barcode.Weight, faker => faker.Random.Double(0.1, 100))
                .RuleFor(barcode => barcode.Type, faker => faker.Random.Bool() ? "UPC-B" : "UPC-C")

                .FinishWith((faker, barcode) => _logger.Debug($"Created Barcode: {barcode}"));

            return fakerForBarcode;
        }

        #endregion

    }
}