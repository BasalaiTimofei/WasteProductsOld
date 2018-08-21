using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Security;
using WasteProducts.DataAccess.Common.UoW.Security;
using WasteProducts.Logic.Common.Models.Security.Infrastructure;
using WasteProducts.Logic.Common.Models.Security.Models;

namespace WasteProducts.Logic.Common.Repositories.Security.Strores
{
   
    //to do нужно отнаследоваться и переопределить метод на свой тип
    internal class AppUserStore : IAppUserStore
    {
        private IUnitOfWork _uow;
        private IUserRepository _userRepository;
        private IUserLoginRepository _userLoginRepository;
        private IUserClaimRepository _userClaimRepository;
        private IUserRoleRepository _userRoleRepository;
        private IRoleRepository _roleRepository;

        public AppUserStore(IUnitOfWork uow,
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            IUserClaimRepository userClaimRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository)
        {
            _uow = uow;
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _userClaimRepository = userClaimRepository;
            _userRoleRepository = userRoleRepository;
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
                    _userRepository = null;
                    _userLoginRepository = null;
                    _userClaimRepository = null;
                    _userRoleRepository = null;
                    _roleRepository = null;
                }
                _disposed = true;
            }
        }

        #region user

        public async Task CreateAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            //to do можно ли так приводить ?
            (_userRepository as IRepositoryBase<IAppUser>).Add(user);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            //to do можно ли так приводить ?
            (_userRepository as IRepositoryBase<IAppUser>).Update(user);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            (_userRepository as IRepositoryBase<IAppUser>).Remove(user);
            await _uow.SaveChangesAsync();
        }

        public async Task<IAppUser> FindByIdAsync(int userId)
        {
            ThrowIfDisposed();
            //to do можно ли так приводить ?
            return await (_userRepository as IRepositoryBase<IAppUser>).GetByIdAsync(userId);
        }

        public async Task<IAppUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            //to do ???
            return await _userRepository.FindByNameAsync(userName) as IAppUser;
        }

        #endregion

        #region userlogin

        public async Task AddLoginAsync(IAppUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            //to do необходимо подменить экземпляр инжектором
            //to do можно ли так приводить ?
            (_userLoginRepository as IRepositoryBase<IUserLogin>).Add(new UserLogin { LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey, UserId = user.Id });
            await _uow.SaveChangesAsync();
        }

        public async Task RemoveLoginAsync(IAppUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            //to do можно ли так приводить ?
            (_userLoginRepository as IRepositoryBase<IUserLogin>).Remove(l => l.UserId == user.Id && l.LoginProvider == login.LoginProvider && l.UserId == user.Id);
            await _uow.SaveChangesAsync();
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            var logins = await _userLoginRepository.GetByUserId(user.Id);
            return logins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList();
        }

        public async Task<IAppUser> FindAsync(UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (login == null)
                throw new ArgumentNullException("login");

            var userLogin = await _userLoginRepository.FindByLoginProviderAndProviderKey(login.LoginProvider, login.ProviderKey);
            if (userLogin == null)
                return default(IAppUser);

            //to do можно ли так приводить ?
            return await (_userRepository as IRepositoryBase<IAppUser>).GetByIdAsync(userLogin.UserId);
        }

        #endregion

        #region claims

        public async Task<IList<Claim>> GetClaimsAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            var claims = await _userClaimRepository.GetByUserId(user.Id);

            return claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        public async Task AddClaimAsync(IAppUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            //to do необходимо подменить параметр на инжектор
            //to do можно ли так приводить
            (_userClaimRepository as IRepositoryBase<IUserClaim>).Add(new UserClaim
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            });
            await _uow.SaveChangesAsync();
        }

        public async Task RemoveClaimAsync(IAppUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            //to do можно ли так приводить ?
            (_userClaimRepository as IRepositoryBase<IUserClaim>).Remove(c => c.UserId == user.Id && c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
            await _uow.SaveChangesAsync();
        }

        #endregion

        #region roles

        public async Task AddToRoleAsync(IAppUser user, string roleName)
        {
            ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");

            var role = await _roleRepository.FindByNameAsync(roleName);
            if (role == null)
                throw new InvalidOperationException("role not found");

            //to do необходимо разорвать зависимость
            //to do можно ли так приводить ?
            (_userRoleRepository as IRepositoryBase<IUserRole>).Add(new UserRole
            {
                RoleId = role.Id,
                UserId = user.Id
            });
            await _uow.SaveChangesAsync();
        }

        public async Task RemoveFromRoleAsync(IAppUser user, string roleName)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");

            var role = await _roleRepository.FindByNameAsync(roleName);
            if (role == null)
                throw new InvalidOperationException("role not found");

            //to do можно ли так приводить ?
            (_userRoleRepository as IRepositoryBase<IUserRole>).Remove(r => r.UserId == user.Id && r.RoleId == role.Id);
            await _uow.SaveChangesAsync();
        }

        public async Task<IList<string>> GetRolesAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            return await _roleRepository.GetRolesNameByUserId(user.Id);
        }

        public async Task<bool> IsInRoleAsync(IAppUser user, string roleName)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");

            var role = await _roleRepository.FindByNameAsync(roleName);
            if (role == null)
                throw new InvalidOperationException("role not found");

            return await _userRoleRepository.IsInRoleAsync(user.Id, role.Id);
        }

        #endregion


        public async Task SetSecurityStampAsync(IAppUser user, string stamp)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.SecurityStamp = stamp;
            await _uow.SaveChangesAsync();
        }

        public Task<string> GetSecurityStampAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.SecurityStamp);
        }

        public async Task SetEmailAsync(IAppUser user, string email)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.Email = email;
            await _uow.SaveChangesAsync();
        }

        public Task<string> GetEmailAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.EmailConfirmed);
        }

        public async Task SetEmailConfirmedAsync(IAppUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.EmailConfirmed = confirmed;

            await _uow.SaveChangesAsync();
        }

        public async Task<IAppUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();
            //to do ???
            return await _userRepository.FindByEmailAsync(email) as IAppUser;
        }

        public async Task SetPhoneNumberAsync(IAppUser user, string phoneNumber)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.PhoneNumber = phoneNumber;

            await _uow.SaveChangesAsync();
        }

        public Task<string> GetPhoneNumberAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberConfirmedAsync(IAppUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.PhoneNumberConfirmed = confirmed;

            await _uow.SaveChangesAsync();
        }

        public async Task SetTwoFactorEnabledAsync(IAppUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.TwoFactorEnabled = enabled;

            await _uow.SaveChangesAsync();
        }

        public Task<bool> GetTwoFactorEnabledAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.LockoutEndDateUtc.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc)) : new DateTimeOffset());
        }

        public async Task SetLockoutEndDateAsync(IAppUser user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? new DateTime?() : lockoutEnd.UtcDateTime;

            await _uow.SaveChangesAsync();
        }

        public async Task<int> IncrementAccessFailedCountAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            ++user.AccessFailedCount;

            await _uow.SaveChangesAsync();

            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount = 0;

            await _uow.SaveChangesAsync();
        }

        public Task<int> GetAccessFailedCountAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.LockoutEnabled);
        }

        public async Task SetLockoutEnabledAsync(IAppUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEnabled = enabled;
            await _uow.SaveChangesAsync();
        }

        #region password 

        public async Task SetPasswordHashAsync(IAppUser user, string passwordHash)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.PasswordHash = passwordHash;
            await _uow.SaveChangesAsync();
        }

        public Task<string> GetPasswordHashAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash);
        }

        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public Task<bool> HasPasswordAsync(IAppUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return PasswordVerificationResult.Failed;
            }
            if (providedPassword == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return PasswordVerificationResult.Failed;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(providedPassword, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            if (ByteArraysEqual(buffer3, buffer4))
                return PasswordVerificationResult.Success;

            return PasswordVerificationResult.Failed;
        }

        public bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }

        #endregion





    }
}
