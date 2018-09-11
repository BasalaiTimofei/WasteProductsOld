using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using WasteProducts.DataAccess.Common.Repositories.Search;
using Ninject;

namespace WasteProducts.DataAccess.Repositories.UserManagement
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly WasteContext _context;

        private readonly RoleStore<IdentityRole> _store;

        private readonly RoleManager<IdentityRole> _manager;

        private bool _disposed;

        public UserRoleRepository(WasteContext context)
        {
            _context = context;
            _store = new RoleStore<IdentityRole>(_context)
            {
                DisposeContext = true
            };
            _manager = new RoleManager<IdentityRole>(_store);
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

        public async Task AddAsync(UserRoleDB role)
        {
            IdentityRole identityRole = new IdentityRole(role.Name) { Id = Guid.NewGuid().ToString() };

            await _manager.CreateAsync(identityRole);
        }

        public async Task DeleteAsync(UserRoleDB role)
        {
            IdentityRole identityRole = await _manager.FindByIdAsync(role.Id);

            await _manager.DeleteAsync(identityRole);
        }

        public async Task<UserRoleDB> FindByIdAsync(string roleId)
        {
            IdentityRole ir = await _manager.FindByIdAsync(roleId);
            UserRoleDB result = new UserRoleDB() { Id = ir.Id, Name = ir.Name };
            return result;
        }

        public async Task<UserRoleDB> FindByNameAsync(string roleName)
        {
            IdentityRole ir = await _manager.FindByNameAsync(roleName);
            if (ir == null)
            {
                return null;
            }
            UserRoleDB result = new UserRoleDB() { Id = ir.Id, Name = ir.Name };
            return result;
        }

        public async Task UpdateRoleNameAsync(UserRoleDB role)
        {
            IdentityRole ir = await _manager.FindByIdAsync(role.Id);
            ir.Name = role.Name;
            await _manager.UpdateAsync(ir);
        }

        public async Task<IEnumerable<UserDB>> GetRoleUsers(UserRoleDB role)
        {
            IdentityRole ir = await _manager.FindByIdAsync(role.Id);
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

        ~UserRoleRepository()
        {
            if (!_disposed)
            {
                Dispose();
            }
        }
    }
}
