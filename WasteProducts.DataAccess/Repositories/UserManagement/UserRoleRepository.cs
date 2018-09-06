using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;
using System.Data.Entity;
using WasteProducts.DataAccess.Common.Repositories.Search;
using Ninject;

namespace WasteProducts.DataAccess.Repositories.UserManagement
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly WasteContext _context;

        private readonly RoleStore<IdentityRole> _store;

        private bool _disposed;

        public UserRoleRepository()
        {
            //TODO: Injection
            //Код ниже был вставлен исключительно для того, что бы собирался проект. Измените его так, как нужно
            StandardKernel kernel = new StandardKernel();
            kernel.Load(new DataAccess.InjectorModule());
            _context = new WasteContext(kernel.Get<ISearchRepository>());
            //

            //_context = new WasteContext();
            _store = new RoleStore<IdentityRole>(_context)
            {
                DisposeContext = true
            };
        }

        public UserRoleRepository(WasteContext context)
        {
            //TODO: Injection
            //Код ниже был вставлен исключительно для того, что бы собирался проект. Измените его так, как нужно
            StandardKernel kernel = new StandardKernel();
            kernel.Load(new DataAccess.InjectorModule());
            _context = new WasteContext(kernel.Get<ISearchRepository>());
            //

            //_context = context;
            _store = new RoleStore<IdentityRole>(_context)
            {
                DisposeContext = true
            };
        }

        ~UserRoleRepository()
        {
            if (!_disposed)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _store?.Dispose();
                _disposed = true;
            }
        }

        public async Task AddAsync(UserRoleDB role)
        {
            IdentityRole identityRole = new IdentityRole(role.Name) { Id = Guid.NewGuid().ToString() };

            await _store.CreateAsync(identityRole);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserRoleDB role)
        {
            IdentityRole identityRole = await _store.FindByIdAsync(role.Id);

            await _store.DeleteAsync(identityRole);
            await _context.SaveChangesAsync();
        }

        public async Task<UserRoleDB> FindByIdAsync(string roleId)
        {
            IdentityRole ir = await _store.FindByIdAsync(roleId);
            UserRoleDB result = new UserRoleDB() { Id = ir.Id, Name = ir.Name };
            return result;
        }

        public async Task<UserRoleDB> FindByNameAsync(string roleName)
        {
            IdentityRole ir = await _store.FindByNameAsync(roleName);
            if (ir == null)
            {
                return null;
            }
            UserRoleDB result = new UserRoleDB() { Id = ir.Id, Name = ir.Name };
            return result;
        }

        public async Task UpdateRoleNameAsync(UserRoleDB role)
        {
            IdentityRole ir = await _store.FindByIdAsync(role.Id);
            ir.Name = role.Name;
            await _store.UpdateAsync(ir);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDB>> GetRoleUsers(UserRoleDB role)
        {
            IdentityRole ir = await _store.FindByIdAsync(role.Id);
            List<string> userIds = new List<string>();

            foreach (IdentityUserRole iur in ir.Users)
            {
                userIds.Add(iur.UserId);
            }

            IEnumerable<UserDB> result = _context.Users.Include(u => u.Roles).
                                                  Include(u => u.Claims).
                                                  Include(u => u.Logins).
                                                  Include(u => u.Friends).
                                                  Include(u => u.ProductDescriptions).
                                                  Where(u => userIds.Contains(u.Id));

            return result.ToArray();
        }
    }
}
