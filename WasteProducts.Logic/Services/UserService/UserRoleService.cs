using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Common.Models.Users;
using AutoMapper;

namespace WasteProducts.Logic.Services.UserService
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _roleRepo;

        public UserRoleService(IUserRoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task CreateAsync(UserRole role)
        {
            await _roleRepo.AddAsync(MapTo<UserRoleDB>(role));
        }

        public async Task DeleteAsync(UserRole role)
        {
            await _roleRepo.DeleteAsync(MapTo<UserRoleDB>(role));
        }

        public async Task<UserRole> FindByIdAsync(string roleId)
        {
            UserRoleDB roleDB = await _roleRepo.FindByIdAsync(roleId);
            return MapTo<UserRole>(roleDB);
        }

        public async Task<UserRole> FindByNameAsync(string roleName)
        {
            UserRoleDB roleDB = await _roleRepo.FindByNameAsync(roleName);
            return MapTo<UserRole>(roleDB);
        }

        public async Task UpdateRoleNameAsync(UserRole role)
        {
            await _roleRepo.UpdateRoleNameAsync(MapTo<UserRoleDB>(role));
        }

        public async Task<IEnumerable<User>> GetRoleUsers(UserRole role)
        {
            UserRoleDB roleDB = MapTo<UserRoleDB>(role);
            IEnumerable<UserDB> subResult = await _roleRepo.GetRoleUsers(roleDB);
            IEnumerable<User> result = Mapper.Map<IEnumerable<User>>(subResult);
            return result;
        }

        private UserRoleDB MapTo<T>(UserRole role)
            where T : UserRoleDB 
            => 
            Mapper.Map<UserRoleDB>(role);

        private UserRole MapTo<T>(UserRoleDB role)
            where T : UserRole
            =>
            Mapper.Map<UserRole>(role);
    }
}
