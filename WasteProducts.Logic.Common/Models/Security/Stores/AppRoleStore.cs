using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Security;
using WasteProducts.DataAccess.Common.UoW.Security;
using WasteProducts.Logic.Common.Models.Security.Infrastructure;

namespace WasteProducts.Logic.Common.Repositories.Security.Strores
{
    /// <summary>
    /// AppRoleStore class has inheritance from IAppRoleStore interface
    /// </summary>
    internal class AppRoleStore : IAppRoleStore
    {
        /// <summary>
        /// _uow field contains UnitOfWork object
        /// </summary>
        private IUnitOfWork _uow;

        /// <summary>
        /// _roleRepository field contains Role repository
        /// </summary>
        private IRoleRepository _roleRepository;

        /// <summary>
        /// Initializes a new instance of AppRoleStore
        /// </summary>
        /// <param name="uow">Used to set _uow field</param>
        /// <param name="roleRepository">Used to set _roleRepository field</param>
        public AppRoleStore(IUnitOfWork uow, IRoleRepository roleRepository)
        {
            _uow = uow;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Method for Throwing ObjectDisposedException
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Method for Execute Dispose() and do not call finializer after Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// bool dispose variable
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing">bool variable for dispose</param>
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && _uow != null)
            {
                if (disposing)
                {
                    _uow = null;
                    _roleRepository = null;
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Create role via role repository async
        /// </summary>
        /// <param name="role">This role will be add to repository</param>
        public async Task CreateAsync(IAppRole role)
        {
            ThrowIfDisposed();
            if (role == null)
                throw new ArgumentNullException("role");
            //to do ? приведение
            (_roleRepository as IRepositoryBase<IAppRole>).Add(role);
            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Update current role async
        /// </summary>
        /// <param name="role">Role for update</param>
        public async Task UpdateAsync(IAppRole role)
        {
            ThrowIfDisposed();
            if (role == null)
                throw new ArgumentNullException("role");
            //to do ? приведение
            (_roleRepository as IRepositoryBase<IAppRole>).Update(role);
            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Delete current role async
        /// </summary>
        /// <param name="role">Role for delete</param>
        public async Task DeleteAsync(IAppRole role)
        {
            ThrowIfDisposed();
            if (role == null)
                throw new ArgumentNullException("role");
            //to do ? приведение
            (_roleRepository as IRepositoryBase<IAppRole>).Remove(role);
            await _uow.SaveChangesAsync();
        }


        /// <summary>
        /// Find role by current id async
        /// </summary>
        /// <param name="roleId">Role id number</param>
        /// <returns>Role for search</returns>
        public async Task<IAppRole> FindByIdAsync(int roleId)
        {
            ThrowIfDisposed();
            //to do ? приведение
            return await (_roleRepository as IRepositoryBase<IAppRole>).GetByIdAsync(roleId);
        }

        /// <summary>
        /// Find role by current name async
        /// </summary>
        /// <param name="roleName">role name</param>
        /// <returns>Role for search</returns>
        public async Task<IAppRole> FindByNameAsync(string roleName)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");
            //to do ? приведение
            return await _roleRepository.FindByNameAsync(roleName) as IAppRole;
        }
    }
}
