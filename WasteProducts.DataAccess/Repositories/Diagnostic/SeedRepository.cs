//using Bogus;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using WasteProducts.DataAccess.Common.Models.Products;
//using WasteProducts.DataAccess.Common.Models.Users;
//using WasteProducts.DataAccess.Common.Repositories.Diagnostic;
//using WasteProducts.DataAccess.Contexts;
//using System.Data.Entity;

//namespace WasteProducts.DataAccess.Repositories.Diagnostic
//{
//    public class SeedRepository : ISeedRepository
//    {
//        private readonly WasteContext _context;

//        private readonly UserStore<UserDB> _store;

//        private readonly UserManager<UserDB> _manager;

//        private readonly Faker _faker; 

//        private bool _disposed;

//        public SeedRepository(WasteContext context, Faker faker)
//        {
//            _context = context;
//            _store = new UserStore<UserDB>(_context)
//            {
//                DisposeContext = true
//            };
//            _manager = new UserManager<UserDB>(_store);
//            _faker = faker;
//        }

//        public void Dispose()
//        {
//            if (!_disposed)
//            {
//                _manager?.Dispose();
//                _disposed = true;
//                GC.SuppressFinalize(this);
//            }
//        }

//        public async Task RecreateAndSeedAsync()
//        {
//            await Task.Run(() =>
//            {
//                _context.Database.Delete();
//                _context.Database.Create();
//                CreateUsers();
//                var user = AddFriendsToFirstUser();
//                _context.SaveChanges();

//                CreateProductCategories();
//                _context.SaveChanges();

//                CreateProductsAndAddThemToTheUser(user);
//                _context.SaveChanges();
//            });

//            void CreateProductsAndAddThemToTheUser(UserDB userDB)
//            {
//                var category1 = _context.Categories.Find(0);
//                var category2 = _context.Categories.Find(1);
//                for (int i = 0; i < 6; i++)
//                {
//                    var name = $"Product number {i}";
//                    var prod = new ProductDB
//                    {
//                        Id = i.ToString(),
//                        Name = name,
//                        Created = DateTime.UtcNow.AddDays(-2),
//                        Modified = null,
//                        Category = i > 2 ? category1 : category2
//                    };
//                    _context.Products.Add(prod);
//                    _context.SaveChanges();
//                    var descr = new UserProductDescriptionDB
//                    {
//                        UserId = userDB.Id,
//                        ProductId = prod.Id,
//                        Rating = i,
//                        Description = (i > 2 ? "Хороший продукт, покупать" : "Плохой продукт, не брать никогда"),
//                        Created = DateTime.UtcNow.AddDays(-2)
//                    };
//                    userDB.ProductDescriptions.Add(descr);
//                    _context.SaveChanges();
//                }
//            }

//            void CreateProductCategories()
//            {
//                var category0 = new CategoryDB
//                {
//                    Id = 0,
//                    Name = "Первая категория",
//                    Description = "Первая категория товаров, хорошие вещи"
//                };
//                var category1 = new CategoryDB
//                {
//                    Id = 1,
//                    Name = "Вторая категория",
//                    Description = "Вторая категория товаров, плохие вещи"
//                };

//                _context.Categories.Add(category0);
//                _context.Categories.Add(category1);
//            }

//            void CreateUsers()
//            {
//                for (int i = 0; i < 10; i++)
//                {
//                    var nameAndPassword = $"{i}{i}{i}{i}{i}{i}";
//                    var userToCreate = new UserDB
//                    {
//                        Id = i.ToString(),
//                        UserName = nameAndPassword,
//                        Email = _faker.Internet.Email(),
//                        EmailConfirmed = true,
//                        Created = DateTime.UtcNow.AddDays(-15)
//                    };
//                    _manager.Create(userToCreate, nameAndPassword);
//                }
//            }

//            UserDB AddFriendsToFirstUser()
//            {
//                var user0 = _context.Users.Include(u => u.Friends).Include(u => u.ProductDescriptions.Select(d => d.Product)).First(u => u.Id == "0");
//                var user1 = _context.Users.Include(u => u.Friends).Include(u => u.ProductDescriptions.Select(d => d.Product)).First(u => u.Id == "1");
//                var user2 = _context.Users.Include(u => u.Friends).Include(u => u.ProductDescriptions.Select(d => d.Product)).First(u => u.Id == "2");
//                var user3 = _context.Users.Include(u => u.Friends).Include(u => u.ProductDescriptions.Select(d => d.Product)).First(u => u.Id == "3");

//                user0.Friends.Add(user1);
//                user0.Friends.Add(user2);
//                user0.Friends.Add(user3);

//                user1.Friends.Add(user0);

//                user0.Modified = DateTime.UtcNow.AddDays(-3);

//                return user0;
//            }
//        }

//        ~SeedRepository()
//        {
//            Dispose();
//        }
//    }
//}
