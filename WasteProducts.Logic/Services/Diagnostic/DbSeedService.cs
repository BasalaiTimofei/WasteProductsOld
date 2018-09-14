﻿using System.Collections.Generic;
using System.Linq;
using Bogus;
using NLog;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Common.Services.Products;
using WasteProducts.Logic.Common.Services.Users;

namespace WasteProducts.Logic.Services
{
    /// <inheritdoc />
    public class DbSeedService : IDbSeedService
    {
        private readonly ITestModelsService _testModelsService;
        private readonly IServiceFactory _serviceFactory;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="testModelsService">Generates test models</param>
        /// <param name="serviceFactory">Service factory, for seeding in database through services.</param>
        /// <param name="logger">NLog logger</param>
        public DbSeedService(ITestModelsService testModelsService, IServiceFactory serviceFactory, ILogger logger)
        {
            _testModelsService = testModelsService;
            _serviceFactory = serviceFactory;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task SeedBaseDataAsync()
        {
            _logger.Debug("Seeding base data");

            // TODO: Seeding base data using services from IServiceFactory

            await AddAdminUser().ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <summary>
        /// Seeds test data to database
        /// </summary>
        /// <returns>Task</returns>
        public async Task SeedTestDataAsync()
        {
            _logger.Debug("Seeding test data");

            IUserService userService = null;
            IProductService productService = null;
            try
            {
                userService = _serviceFactory.CreateUserService();
                productService = _serviceFactory.CreateProductService();

                var faker = new Faker("en");

                var categoriesCount = 5;
                List<Category> categories = CreateCategories(faker, categoriesCount);

                for (int i = 0; i < 10; i++)
                {
                    var email = faker.Internet.Email();
                    await userService.RegisterAsync(email, $"user{i}", $"password{i}", null).ConfigureAwait(false);
                }
                var users = await userService.GetAllUsersAsync();

                foreach (var user in users)
                {
                    for (int j = 0; j < categoriesCount; j++)
                    {
                        var productName = faker.Commerce.ProductName();
                        productService.Add(productName, out  var addedProduct);

                        var product = await productService.GetByNameAsync(productName).ConfigureAwait(false);
                        productService.AddCategory(product, categories[j]);

                        var isGoodProduct = faker.Random.Bool();
                        var rating = faker.Random.Int(0, 10);
                        await userService.AddProductAsync(user.Id, product.Id, rating, $"The '{productName}' is the {(isGoodProduct ? "best" : "most crappy")} product I've ever seen .. bla-bla-bla ...").ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                userService?.Dispose();
                productService?.Dispose();
            }
        }


        private List<Category> CreateCategories(Faker faker, int categoriesCount)
        {
            var categories = new List<Category>(categoriesCount);
            using (var categoryService = _serviceFactory.CreateCategoryService())
            {
                var categoryNames = faker.Commerce.Categories(5);
                categoryService.AddRange(categoryNames);

                categories.AddRange(categoryNames.Select(categoryName => categoryService.Get(categoryName)));
            }

            return categories;
        }

        private async Task AddAdminUser()
        {
            var adminEmail = "admin@wasteproducts.com";
            var adminUsername = "admin";
            var adminPassword = "admin1";
            var adminRoleName = "Admin";

            using (var roleService = _serviceFactory.CreateRoleService())
            {
                var adminRole = new UserRole() { Name = adminRoleName };
                await roleService.CreateAsync(adminRole).ConfigureAwait(false);
            }

            using (var userService = _serviceFactory.CreateUserService())
            {
                await userService.RegisterAsync(adminEmail, adminUsername, adminPassword, null).ConfigureAwait(false);
                var adminUser = await userService.LogInByNameAsync(adminUsername, adminPassword).ConfigureAwait(false);
                await userService.AddToRoleAsync(adminUser.Id, adminRoleName).ConfigureAwait(false);
            }
        }
    }
}
