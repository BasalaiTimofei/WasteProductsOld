using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Security;
using WasteProducts.DataAccess.Common.UoW.Security;
using WasteProducts.Logic.Common.Models.Security.Infrastructure;

namespace WasteProducts.Logic.Common.Repositories.Security.Strores
{
    internal class AppRoleStore : IAppRoleStore
    {
        private IUnitOfWork _uow;
        private IRoleRepository _roleRepository;

        public AppRoleStore(IUnitOfWork uow, IRoleRepository roleRepository)
        {
            _uow = uow;
            _roleRepository = roleRepository;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool _disposed;
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

        public async Task CreateAsync(IAppRole role)
        {
            ThrowIfDisposed();
            if (role == null)
                throw new ArgumentNullException("role");
            //to do ? приведение
            (_roleRepository as IRepositoryBase<IAppRole>).Add(role);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(IAppRole role)
        {
            ThrowIfDisposed();
            if (role == null)
                throw new ArgumentNullException("role");
            //to do ? приведение
            (_roleRepository as IRepositoryBase<IAppRole>).Update(role);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(IAppRole role)
        {
            ThrowIfDisposed();
            if (role == null)
                throw new ArgumentNullException("role");
            //to do ? приведение
            (_roleRepository as IRepositoryBase<IAppRole>).Remove(role);
            await _uow.SaveChangesAsync();
        }

        public async Task<IAppRole> FindByIdAsync(int roleId)
        {
            ThrowIfDisposed();
            //to do ? приведение
            return await (_roleRepository as IRepositoryBase<IAppRole>).GetByIdAsync(roleId);
        }

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
