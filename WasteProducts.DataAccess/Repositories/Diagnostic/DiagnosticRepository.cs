using Bogus;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.Diagnostic;
using WasteProducts.DataAccess.Contexts;
using System.Data.Entity;
using WasteProducts.DataAccess.Common.Models.Groups;
using System.Collections.Generic;

namespace WasteProducts.DataAccess.Repositories.Diagnostic
{
    public class DiagnosticRepository : IDiagnosticRepository
    {
        private readonly Random _random = new Random();

        private readonly WasteContext _context;

        private readonly UserStore<UserDB> _store;

        private readonly UserManager<UserDB> _manager;

        private readonly Faker _faker;

        private readonly string[] productDescriptions =
        {
            "Отрава, никогда не покупать снова",    // 1
            "Продукт очень плох",                   // 2
            "Не очень",                             // 3
            "Более или менее неплохой продукт",     // 4
            "Хороший продукт, можно покупать"       // 5
        };

        private bool _disposed;

        public DiagnosticRepository(WasteContext context, Faker faker)
        {
            _context = context;
            _store = new UserStore<UserDB>(_context)
            {
                DisposeContext = true
            };
            _manager = new UserManager<UserDB>(_store);
            _faker = faker;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _manager?.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        public async Task RecreateAsync()
        {
            await Task.Run(() =>
            {
                _context.Database.Delete();
                _context.Database.Create();
            }).ConfigureAwait(false);
        }

        public async Task SeedAsync(IList<string> prodIds)
        {
            await CreateUsers();
            var user = AddFriendsToFirstUser();

            CreateProductsAndAddThemToTheUsers();

            CreateGroups();

            await _context.SaveChangesAsync().ConfigureAwait(false);

            async Task CreateUsers()
            {
                for (int i = 0; i < 10; i++)
                {
                    var userToCreate = new UserDB
                    {
                        Id = i.ToString(),
                        UserName = $"UserName{i}",
                        Email = _faker.Internet.Email(),
                        EmailConfirmed = true,
                        Created = DateTime.UtcNow.AddDays(-15)
                    };
                    _context.Users.Add(userToCreate);
                }
                await _context.SaveChangesAsync().ConfigureAwait(false);

                for (int i = 0; i < 10; i++)
                {
                    await _manager.AddPasswordAsync(i.ToString(), $"{i}{i}{i}{i}{i}{i}").ConfigureAwait(false);
                }
            }


            UserDB AddFriendsToFirstUser()
            {
                var user0 = _context.Users.Include(u => u.Friends).Include(u => u.ProductDescriptions.Select(d => d.Product)).First(u => u.Id == "0");
                var user1 = _context.Users.Include(u => u.Friends).Include(u => u.ProductDescriptions.Select(d => d.Product)).First(u => u.Id == "1");
                var user2 = _context.Users.Include(u => u.Friends).Include(u => u.ProductDescriptions.Select(d => d.Product)).First(u => u.Id == "2");
                var user3 = _context.Users.Include(u => u.Friends).Include(u => u.ProductDescriptions.Select(d => d.Product)).First(u => u.Id == "3");

                user0.Friends.Add(user1);
                user0.Friends.Add(user2);
                user0.Friends.Add(user3);

                user1.Friends.Add(user0);

                user0.Modified = DateTime.UtcNow.AddDays(-3);

                return user0;
            }

            void CreateProductsAndAddThemToTheUsers()
            {
                for (int i = 0; i < 7; i++)
                {
                    var rating = _random.Next(1, 6);
                    var prodDescription = new UserProductDescriptionDB
                    {
                        UserId = "0",
                        ProductId = prodIds[i],
                        Rating = rating,
                        Description = productDescriptions[rating - 1],
                        Created = DateTime.UtcNow.AddDays(-2)
                    };
                    _context.UserProductDescriptions.Add(prodDescription);

                    _context.SaveChanges();
                }

                for (int i = 1; i < 10; i++)
                {
                    foreach (var prodId in prodIds)
                    {
                        var rating = _random.Next(1, 6);
                        var prodDescription = new UserProductDescriptionDB
                        {
                            UserId = i.ToString(),
                            ProductId = prodId,
                            Rating = rating,
                            Description = productDescriptions[rating - 1],
                            Created = DateTime.UtcNow.AddDays(-2)
                        };
                        _context.UserProductDescriptions.Add(prodDescription);
                        _context.SaveChanges();
                    }
                }
            }

            void CreateGroups()
            {
                for (int i = 0; i < 3; i++)
                {
                    var group = new GroupDB
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = $"{_faker.Lorem.Word()} group",
                        Information = _faker.Lorem.Sentence(),
                        AdminId = i.ToString(),
                        Created = DateTime.UtcNow.AddDays(-4),
                        IsNotDeleted = true
                    };
                    _context.Groups.Add(group);

                    for (int j = 0; j < 10; j++)
                    {
                        var groupUser = new GroupUserDB
                        {
                            UserName = $"UserName{i}",
                            UserId = j.ToString(),
                            GroupId = group.Id,
                            IsConfirmed = true,
                            RightToCreateBoards = true,
                            Created = DateTime.UtcNow.AddDays(-1)
                        };
                        _context.GroupUsers.Add(groupUser);
                    }

                    for (int j = 0; j < 2; j++)
                    {
                        var gbName = $"Board about {_faker.Lorem.Word()}";
                        var groupBoard = new GroupBoardDB
                        {
                            Id = Guid.NewGuid().ToString(),
                            GroupId = group.Id,
                            CreatorId = i.ToString(),
                            Name = gbName,
                            Information = _faker.Lorem.Sentence(),
                            Created = DateTime.UtcNow.AddDays(-2),
                            IsNotDeleted = true
                        };
                        _context.GroupBoards.Add(groupBoard);

                        var groupProduct = new GroupProductDB
                        {
                            Id = Guid.NewGuid().ToString(),
                            GroupBoardId = groupBoard.Id,
                            Name = _faker.Commerce.ProductName(),
                            ProductId = prodIds[_random.Next(0, 9)],
                            Information = _faker.Lorem.Sentence()
                        };
                        _context.GroupProducts.Add(groupProduct);

                        for (int k = 0; k < 10; k++)
                        {
                            var groupComment = new GroupCommentDB
                            {
                                Id = Guid.NewGuid().ToString(),
                                GroupBoardId = groupBoard.Id,
                                CommentatorId = _random.Next(0, 10).ToString(),
                                Comment = _faker.Lorem.Sentence()
                            };
                            _context.GroupComments.Add(groupComment);
                        }
                    }
                }
            }
        }

        ~DiagnosticRepository()
        {
            Dispose();
        }
    }
}
