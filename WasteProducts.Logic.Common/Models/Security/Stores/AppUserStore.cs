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
    /// <summary>
    /// AppUserStore class has inheritance from IAppUserStore interface
    /// </summary>
    internal class AppUserStore : IAppUserStore
    {
        /// <summary>
        /// _uow field will be contains UnitOfWork object
        /// </summary>
        private IUnitOfWork _uow;

        /// <summary>
        /// _userRepository field will be contains User Repository object
        /// </summary>
        private IUserRepository _userRepository;

        /// <summary>
        /// _userLoginRepository field will be contains User Login Repository object
        /// </summary>
        private IUserLoginRepository _userLoginRepository;

        /// <summary>
        /// _userClaimRepository field will be contains User Claim Repository object
        /// </summary>
        private IUserClaimRepository _userClaimRepository;

        /// <summary>
        /// _userRoleRepository field will be contains User Role Repository object
        /// </summary>
        private IUserRoleRepository _userRoleRepository;

        /// <summary>
        /// _userRoleRepository field will be contains User Role Repository object
        /// </summary>
        private IRoleRepository _roleRepository;

        /// <summary>
        /// Initializes a new instance of AppUserStore
        /// </summary>
        /// <param name="uow">Used to set _uow field</param>
        /// <param name="userRepository">Used to set _userRepository field</param>
        /// <param name="userLoginRepository">Used to set _userLoginRepository field</param>
        /// <param name="userClaimRepository">Used to set _userClaimRepository field</param>
        /// <param name="userRoleRepository">Used to set _userRoleRepository field</param>
        /// <param name="roleRepository">Used to set _roleRepository field</param>
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

        /// <summary>
        /// Asynchronously inserts an AppUser.
        /// </summary>
        /// <param name="user">New user</param>
        public async Task CreateAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            //to do можно ли так приводить ?
            (_userRepository as IRepositoryBase<IAppUser>).Add(user);
            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously update current AppUser
        /// </summary>
        /// <param name="user">Update user</param>
        public async Task UpdateAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            //to do можно ли так приводить ?
            (_userRepository as IRepositoryBase<IAppUser>).Update(user);
            await _uow.SaveChangesAsync();
        }

        /// <summary>
        ///  Delete an AppUser async.
        /// </summary>
        /// <param name="user">Delete user</param>
        public async Task DeleteAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            (_userRepository as IRepositoryBase<IAppUser>).Remove(user);
            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously finds a user by ID.
        /// </summary>
        /// <param name="userId">user id number</param>
        /// <returns>returns IAppUser</returns>
        public async Task<IAppUser> FindByIdAsync(int userId)
        {
            ThrowIfDisposed();
            //to do можно ли так приводить ?
            return await (_userRepository as IRepositoryBase<IAppUser>).GetByIdAsync(userId);
        }

        /// <summary>
        /// Asynchronously finds a user by name.
        /// </summary>
        /// <param name="userName">user name string</param>
        /// <returns>returns Task AppUser</returns>
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
        /// <summary>
        /// Asynchronously adds a login to the user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="login">login as UserLoginInfo</param>
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

        /// <summary>
        /// Asynchronously removes a login from a user.
        /// </summary>
        /// <param name="user">user as IUser</param>
        /// <param name="login">login as UserLoginInfo</param>
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

        /// <summary>
        /// Asynchronously gets the logins for a user.
        /// </summary>
        /// <param name="user">user as IUser</param>
        /// <returns>returns Task List of UserLoginInfo</returns>
        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            var logins = await _userLoginRepository.GetByUserId(user.Id);
            return logins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList();
        }

        /// <summary>
        /// Asynchronously returns the user associated with this login.
        /// </summary>
        /// <param name="user">login as UserLoginInfo</param>
        /// <returns>returns Task User</returns>
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

        /// <summary>
        /// Asynchronously returns the claims for a user.
        /// </summary>
        /// <param name="user">user as IUser</param>
        /// <returns>returns Task List of Claim</returns>
        public async Task<IList<Claim>> GetClaimsAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            var claims = await _userClaimRepository.GetByUserId(user.Id);

            return claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        /// <summary>
        /// Asynchronously adds a claim to a user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="claim">user as Claim</param>
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

        /// <summary>
        /// Asynchronously removes a claim from a user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="claim">user as IUserClaim</param>
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

        /// <summary>
        /// Asynchronously adds a user to a role.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="roleName">roleName as string name of role</param>
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

        /// <summary>
        /// Asynchronously removes a role from a user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="roleName">roleName as string name of role</param>
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

        /// <summary>
        /// Asynchronously gets the names of the roles by user id.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns T List of reoles</returns>
        public async Task<IList<string>> GetRolesAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            return await _roleRepository.GetRolesNameByUserId(user.Id);
        }

        /// <summary>
        /// Asynchronously determines whether the user is in the named role.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="roleName">roleName as string name of role</param>
        /// <returns>returns true or false</returns>
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

        /// <summary>
        /// Asynchronously sets the security stamp for the user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="stamp">stamp string as security stamp</param>
        public async Task SetSecurityStampAsync(IAppUser user, string stamp)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.SecurityStamp = stamp;
            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously gets the security stamp for a user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns Task security stamp</returns>
        public Task<string> GetSecurityStampAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.SecurityStamp);
        }

        /// <summary>
        /// Asynchronously sets the user e-mail.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="email">email address string</param>
        public async Task SetEmailAsync(IAppUser user, string email)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.Email = email;
            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously gets the user's e-mail.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns Email name</returns>
        public Task<string> GetEmailAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.Email);
        }

        /// <summary>
        /// Asynchronously returns whether the user email is confirmed.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns Task true or false</returns>
        public Task<bool> GetEmailConfirmedAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.EmailConfirmed);
        }


        /// <summary>
        /// Asynchronously sets the IsConfirmed property for the user EmailConfirmed.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="confirmed">status as bool value</param>
        public async Task SetEmailConfirmedAsync(IAppUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.EmailConfirmed = confirmed;

            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously finds a user by e-mail.
        /// </summary>
        /// <param name="email">user as string address</param>
        /// <returns>returns Task IAppUser</returns>
        public async Task<IAppUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();
            //to do ???
            return await _userRepository.FindByEmailAsync(email) as IAppUser;
        }

        /// <summary>
        /// Asynchronously sets the user phone number.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="phoneNumber">phoneNumber as string</param>
        public async Task SetPhoneNumberAsync(IAppUser user, string phoneNumber)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.PhoneNumber = phoneNumber;

            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously gets a user's phone number.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns Task Phone number string</returns>
        public Task<string> GetPhoneNumberAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PhoneNumber);
        }

        /// <summary>
        /// Asynchronously returns whether the user phone number is confirmed.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns true or false</returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <summary>
        /// Asynchronously sets the PhoneNumberConfirmed property for the user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="confirmed">status as bool value</param>
        public async Task SetPhoneNumberConfirmedAsync(IAppUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.PhoneNumberConfirmed = confirmed;

            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously sets the Two Factor provider for the user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="confirmed">status as bool value</param>
        public async Task SetTwoFactorEnabledAsync(IAppUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.TwoFactorEnabled = enabled;

            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously determines whether the two-factor providers are enabled for the user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns Task true or false</returns>
        public Task<bool> GetTwoFactorEnabledAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Asynchronously returns the DateTimeOffset that represents the end of a users lockout, any time in the past should be considered not locked out.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns Task DateTimeOffset</returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.LockoutEndDateUtc.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc)) : new DateTimeOffset());
        }

        /// <summary>
        /// Asynchronously locks a user out until the specified end date (set to a past date, to unlock a user).
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="lockoutEnd">date end of lock</param>
        public async Task SetLockoutEndDateAsync(IAppUser user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? new DateTime?() : lockoutEnd.UtcDateTime;

            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously records the failed attempt to access the user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns failed count of access</returns>
        public async Task<int> IncrementAccessFailedCountAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            ++user.AccessFailedCount;

            await _uow.SaveChangesAsync();

            return user.AccessFailedCount;
        }

        /// <summary>
        /// Asynchronously resets the account access failed count, typically after the account is successfully accessed.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        public async Task ResetAccessFailedCountAsync(IAppUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount = 0;

            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously returns the current number of failed access attempts.This number usually will be reset whenever the password is verified or the account is locked out.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns failed count of access</returns>
        public Task<int> GetAccessFailedCountAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Asynchronously returns whether the user can be locked out.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns true or false</returns>
        public Task<bool> GetLockoutEnabledAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.LockoutEnabled);
        }

        /// <summary>
        /// Asynchronously sets whether the user can be locked out.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="enabled">enable lockout</param>
        public async Task SetLockoutEnabledAsync(IAppUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEnabled = enabled;
            await _uow.SaveChangesAsync();
        }

        #region password 

        /// <summary>
        /// Asynchronously sets the password hash for a user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <param name="passwordHash">passwordHash string</param>
        public async Task SetPasswordHashAsync(IAppUser user, string passwordHash)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.PasswordHash = passwordHash;
            await _uow.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously gets the password hash for a user.
        /// </summary>
        /// <param name="user">user as IAppUser</param>
        /// <returns>returns Task password Hash</returns>
        public Task<string> GetPasswordHashAsync(IAppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// Hash method for password string.
        /// </summary>
        /// <param name="password">current password</param>
        /// <returns>returns Hashed password string</returns>
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

        /// <summary>
        /// Checking async that password not null
        /// </summary>
        /// <param name="user">current user</param>
        /// <returns>returns true or false</returns>
        public Task<bool> HasPasswordAsync(IAppUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        /// <summary>
        /// Method for verify user password with current hashpassword in database
        /// </summary>
        /// <param name="hashedPassword">current hashedPassword</param>
        /// <param name="providedPassword">current providedPassword</param>
        /// <returns>returns PasswordVerificationResult.Success or PasswordVerificationResult.Failed</returns>
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

        /// <summary>
        /// Method checking equal byte arrays
        /// </summary>
        /// <param name="b1">first byte array</param>
        /// <param name="b2">second byte array</param>
        /// <returns>returns true or false</returns>
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
