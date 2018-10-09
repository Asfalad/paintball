using Dapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Paintball.DAL.Entities;

namespace Paintball.DAL
{
    public class UserStore<TUser> : IUserStore<TUser, Guid>,
        IUserPasswordStore<TUser, Guid>,
        IUserEmailStore<TUser, Guid>,
        IUserSecurityStampStore<TUser, Guid>,
        IUserRoleStore<TUser, Guid>,
        IUserLockoutStore<TUser, Guid>,
        IUserPhoneNumberStore<TUser, Guid>,
        IUserTwoFactorStore<TUser, Guid>,
        IUserLoginStore<TUser, Guid>,
        IQueryableUserStore<TUser, Guid>
        where TUser : Entities.IdentityUser
    {
        private IDbContext _context;

        public UserStore(IDbContext context)
        {
            _context = context;
        }

        /* IUserStore
        ---------------------------*/

        public System.Threading.Tasks.Task CreateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            
            //ensure UserId
            if (user.UserId == default(Guid))
                user.UserId = Guid.NewGuid();

            //conver to sql min date
            var sqlMinDate = new DateTimeOffset(1753, 1, 1, 0, 0, 0, TimeSpan.FromHours(0));
            if (user.LockoutEndDateUtc < sqlMinDate)
                user.LockoutEndDateUtc = Convert.ToDateTime(sqlMinDate);

                return System.Threading.Tasks.Task.FromResult(_context.Connection.Execute("IdentityUser_Create", new
                {
                    user.UserId,
                    user.Email,
                    user.EmailConfirmed,
                    user.PasswordHash,
                    user.SecurityStamp,
                    user.PhoneNumber,
                    user.PhoneNumberConfirmed,
                    user.TwoFactorEnabled,
                    user.LockoutEndDateUtc,
                    user.LockoutEnabled,
                    user.AccessFailedCount,
                    user.UserName,
                    user.CompanyId,
                    user.Salary
                }, commandType: System.Data.CommandType.StoredProcedure));
        }

        public void Create(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            
            //ensure UserId
            if (user.UserId == default(Guid))
                user.UserId = Guid.NewGuid();

            //conver to sql min date
            var sqlMinDate = new DateTimeOffset(1753, 1, 1, 0, 0, 0, TimeSpan.FromHours(0));
            if (user.LockoutEndDateUtc < sqlMinDate)
                user.LockoutEndDateUtc = Convert.ToDateTime(sqlMinDate);

            _context.Connection.Execute("IdentityUser_Create", new
            {
                user.UserId,
                user.Email,
                user.EmailConfirmed,
                user.PasswordHash,
                user.SecurityStamp,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.TwoFactorEnabled,
                user.LockoutEndDateUtc,
                user.LockoutEnabled,
                user.AccessFailedCount,
                user.UserName,
                user.CompanyId,
                user.Salary
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public System.Threading.Tasks.Task DeleteAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(_context.Connection.Execute("IdentityUser_Delete", 
                new { USERID = user.UserId }, commandType: System.Data.CommandType.StoredProcedure));
        }

        public System.Threading.Tasks.Task<TUser> FindByIdAsync(Guid userId)
        {
            var sql = @"
            SELECT 
                IU.*, -- identity user
                IP.* -- identiy profile
            FROM IdentityUser IU
                INNER JOIN IdentityProfile IP ON IU.UserId = IP.UserId 
            WHERE IU.UserId=@USERID";

            
            var result = _context.Connection.Query<TUser, Entities.IdentityProfile, TUser>(sql, (user, profile) =>
            {
                user.Profile = profile;

                return user;
            }, new { userId }, splitOn: "UserId").SingleOrDefault();

            return System.Threading.Tasks.Task.FromResult(result);
        }

        public TUser FindById(Guid userId)
        {
            var sql = @"
                SELECT 
                    IU.*, -- identity user
                    IP.* -- identiy profile
                FROM IdentityUser IU
                    INNER JOIN IdentityProfile IP ON IU.UserId = IP.UserId 
                WHERE IU.UserId=@USERID";

            var result = _context.Connection.Query<TUser, Entities.IdentityProfile, TUser>(sql, (user, profile) =>
            {
                user.Profile = profile;

                return user;
            }, new { userId }, splitOn: "UserId").SingleOrDefault();

            return result;
        }

        public System.Threading.Tasks.Task<TUser> FindByNameAsync(string userName)
        {
            var sql = @"
                SELECT 
                    IU.*, -- identity user
                    IP.* -- identiy profile
                FROM IdentityUser IU
                    INNER JOIN IdentityProfile IP ON IU.UserId = IP.UserId 
                WHERE IU.UserName=@USERNAME";

            var result = _context.Connection.Query<TUser, Entities.IdentityProfile, TUser>(sql, (user, profile) =>
            {
                user.Profile = profile;

                return user;
            }, new { userName }, splitOn: "UserId").SingleOrDefault();

            return System.Threading.Tasks.Task.FromResult(result);
        }

        public TUser FindByName(string userName)
        {
            var sql = @"
                SELECT 
                    IU.*, -- identity user
                    IP.* -- identiy profile
                FROM IdentityUser IU
                    INNER JOIN IdentityProfile IP ON IU.UserId = IP.UserId 
                WHERE IU.UserName=@USERNAME";

            var result = _context.Connection.Query<TUser, Entities.IdentityProfile, TUser>(sql, (user, profile) =>
            {
                user.Profile = profile;

                return user;
            }, new { userName }, splitOn: "UserId").SingleOrDefault();

            return result;
        }

        public System.Threading.Tasks.Task UpdateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(_context.Connection.Execute("IdentityUser_Update", new
            {
                //user
                user.UserId,
                user.Email,
                user.EmailConfirmed,
                user.PasswordHash,
                user.SecurityStamp,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.TwoFactorEnabled,
                user.LockoutEndDateUtc,
                user.LockoutEnabled,
                user.AccessFailedCount,
                user.UserName,
                user.CompanyId,
                user.Salary,

                //profile
                user.Profile.FirstName,
                user.Profile.MiddleName,
                user.Profile.LastName
            }, commandType: System.Data.CommandType.StoredProcedure ));
        }

        public int Update(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

           return _context.Connection.Execute("IdentityUser_Update", new
            {
                //user
                user.UserId,
                user.Email,
                user.EmailConfirmed,
                user.PasswordHash,
                user.SecurityStamp,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.TwoFactorEnabled,
                user.LockoutEndDateUtc,
                user.LockoutEnabled,
                user.AccessFailedCount,
                user.UserName,
                user.CompanyId,
                user.Salary,

                //profile
                user.Profile.FirstName,
                user.Profile.MiddleName,
                user.Profile.LastName
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public int AddToCompany(IdentityUser user, Company company)
        {
            return _context.Connection.Execute("IdentityUser_AddToCompany",
                new
                {
                    USERID = user.Id,
                    COMPANYID = company.Id
                }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public int RemoveFromCompany(IdentityUser user)
        {
            return _context.Connection.Execute("IdentityUser_RemoveFromCompany",
                new
                {
                    USERID = user.Id
                }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context = null;
            }
        }

        /* IUserPasswordStore
        ---------------------------*/

        public System.Threading.Tasks.Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(user.PasswordHash);
        }

        public System.Threading.Tasks.Task<bool> HasPasswordAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(!String.IsNullOrEmpty(user.PasswordHash));
        }

        public System.Threading.Tasks.Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PasswordHash = passwordHash;

            return System.Threading.Tasks.Task.FromResult(0);
        }

        /* IUserEmailStore
        ---------------------------*/

        public System.Threading.Tasks.Task<TUser> FindByEmailAsync(string email)
        {
            if (String.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");

            var sql = @"
                SELECT 
                    IU.*, -- identity user
                    IP.* -- identiy profile
                FROM IdentityUser IU
                    INNER JOIN IdentityProfile IP ON IU.UserId = IP.UserId 
                WHERE IU.Email=@EMAIL";

                var result = _context.Connection.Query<TUser, Entities.IdentityProfile, TUser>(sql, (user, profile) =>
                {
                    user.Profile = profile;

                    return user;
                }, new { email }, splitOn: "UserId").SingleOrDefault();

                return System.Threading.Tasks.Task.FromResult(result);
        }

        public System.Threading.Tasks.Task<string> GetEmailAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(user.Email);
        }

        public System.Threading.Tasks.Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(user.EmailConfirmed);
        }

        public System.Threading.Tasks.Task SetEmailAsync(TUser user, string email)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.Email = email;

            return System.Threading.Tasks.Task.FromResult(0);
        }

        public System.Threading.Tasks.Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.EmailConfirmed = confirmed;

            return System.Threading.Tasks.Task.FromResult(0);
        }

        /* IUserSecurityStampStore
        ---------------------------*/

        public System.Threading.Tasks.Task<string> GetSecurityStampAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(user.SecurityStamp);
        }

        public System.Threading.Tasks.Task SetSecurityStampAsync(TUser user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.SecurityStamp = stamp;

            return System.Threading.Tasks.Task.FromResult(0);
        }

        /* IUserRoleStore
        ---------------------------*/

        public System.Threading.Tasks.Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            
            return System.Threading.Tasks.Task.FromResult(_context.Connection.Execute("IdentityUser_AddToRole", new
            {
                user.UserId,
                roleName
            }, commandType: System.Data.CommandType.StoredProcedure));
        }

        public void AddToRole(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _context.Connection.Execute("IdentityUser_AddToRole", new
            {
                user.UserId,
                roleName
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public System.Threading.Tasks.Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var sql = @"
                SELECT
                    IR.Name
                FROM
                    IdentityRole IR
                    INNER JOIN IdentityUserRole IUR ON IR.RoleId = IUR.RoleId AND IUR.UserId=@USERID";

                return System.Threading.Tasks.Task.FromResult<IList<string>>(_context.Connection.Query<string>(sql, new
                {
                    user.UserId
                }).ToList());
        }

        public IList<string> GetRoles(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var sql = @"
                SELECT
                    IR.Name
                FROM
                    IdentityRole IR
                    INNER JOIN IdentityUserRole IUR ON IR.RoleId = IUR.RoleId AND IUR.UserId=@USERID";

            return new List<string>( _context.Connection.Query<string>(sql, new
            {
                user.UserId
            }));
        }

        public async System.Threading.Tasks.Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            var result = await GetRolesAsync(user);

            if (result == null || result.Count == 0)
                return false;

            return result.Contains<string>(roleName);
        }

        public bool IsInRole(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            var result = GetRoles(user);

            if (result == null || result.Count == 0)
                return false;

            return result.Contains<string>(roleName);
        }

        public System.Threading.Tasks.Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

           
            return System.Threading.Tasks.Task.FromResult(_context.Connection.Execute("IdentityUser_RemoveFromRole", new
            {
                user.UserId,
                roleName
            }, commandType: System.Data.CommandType.StoredProcedure));
        }

        public void RemoveFromRole(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");


            _context.Connection.Execute("IdentityUser_RemoveFromRole", new
            {
                user.UserId,
                roleName
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        /* IUserLockoutStore
        ---------------------------*/

        public System.Threading.Tasks.Task<int> GetAccessFailedCountAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(user.AccessFailedCount);
        }

        public System.Threading.Tasks.Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(user.LockoutEnabled);
        }

        public System.Threading.Tasks.Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (!user.LockoutEndDateUtc.HasValue)
                return null;

            return System.Threading.Tasks.Task.FromResult(new DateTimeOffset(user.LockoutEndDateUtc.Value));
        }

        public System.Threading.Tasks.Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount++;

            return System.Threading.Tasks.Task.FromResult(user.AccessFailedCount);
        }

        public System.Threading.Tasks.Task ResetAccessFailedCountAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount = 0;

            return System.Threading.Tasks.Task.FromResult(0);
        }

        public System.Threading.Tasks.Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEnabled = enabled;

            return System.Threading.Tasks.Task.FromResult(0);
        }

        public System.Threading.Tasks.Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var sqlMinDate = new DateTimeOffset(1753, 1, 1, 0, 0, 0, TimeSpan.FromHours(0));

            if (lockoutEnd < sqlMinDate)
            {
                lockoutEnd = sqlMinDate;
            }

            user.LockoutEndDateUtc = Convert.ToDateTime(lockoutEnd);

            return System.Threading.Tasks.Task.FromResult(0);
        }

        /* IUserPhoneNumberStore
        ---------------------------*/

        public System.Threading.Tasks.Task<string> GetPhoneNumberAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(user.PhoneNumber);
        }

        public System.Threading.Tasks.Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(user.PhoneNumberConfirmed);
        }

        public System.Threading.Tasks.Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PhoneNumber = phoneNumber;

            return System.Threading.Tasks.Task.FromResult(0);
        }

        public System.Threading.Tasks.Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PhoneNumberConfirmed = confirmed;

            return System.Threading.Tasks.Task.FromResult(0);
        }

        /* IUserTwoFactorStore
        ---------------------------*/

        public System.Threading.Tasks.Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return System.Threading.Tasks.Task.FromResult(user.TwoFactorEnabled);
        }

        public System.Threading.Tasks.Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.TwoFactorEnabled = enabled;

            return System.Threading.Tasks.Task.FromResult(0);
        }

        /* IUserLoginStore
        ---------------------------*/

        public System.Threading.Tasks.Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (login == null)
                throw new ArgumentNullException("login");

            var sql = @"
                INSERT INTO [dbo].[IdentityLogin]
                           ([LoginProvider]
                           ,[ProviderKey]
                           ,[UserId])
                     VALUES
                           (@LOGINPROVIDER
                           ,@PROVIDERKEY
                           ,@USERID)";

            return System.Threading.Tasks.Task.FromResult(_context.Connection.Execute(sql, new
            {
                login.LoginProvider,
                login.ProviderKey,
                user.UserId
            }));
        }

        public System.Threading.Tasks.Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            var sql = @"
                SELECT
                    UserId 
                FROM 
                    IdentityLogin
                WHERE 
                    LoginProvider=@LOGINPROVIDER
                    AND ProviderKey=@PROVIDERKEY";

            var userId = default(Guid);

                //get user id (could combine this into a single sql statement)
                userId = _context.Connection.Query<Guid>(sql, new
                {
                    login.LoginProvider,
                    login.ProviderKey,
                }).SingleOrDefault();

            //return user
            if (userId != default(Guid))
                return FindByIdAsync(userId);

            //null user
            return System.Threading.Tasks.Task.FromResult<TUser>(null);
        }

        public System.Threading.Tasks.Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var sql = @"
                SELECT
                    * 
                FROM 
                    IdentityLogin
                WHERE 
                    UserId=@USERID";

                return System.Threading.Tasks.Task.FromResult<IList<UserLoginInfo>>(_context.Connection.Query<UserLoginInfo, dynamic, UserLoginInfo>(sql, (userLoginInfo, result) =>
                {
                    return new UserLoginInfo(result.LoginProvider, result.ProviderKey);
                }, new
                {
                    user.UserId
                }, splitOn: "UserId").ToList());
        }

        public System.Threading.Tasks.Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            var sql = @"
                DELETE FROM IdentityLogin
                WHERE 
                    UserId=@USERID
                    AND LoginProvider=@LOGINPROVIDER
                    AND ProviderKey=@PROVIDERKEY";

                return System.Threading.Tasks.Task.FromResult(_context.Connection.Query<UserLoginInfo>(sql, new
                {
                    user.UserId,
                    login.LoginProvider,
                    login.ProviderKey
                }).ToList() as IList<UserLoginInfo>);
        }

        /* IQueryableUserStore
        ---------------------------*/

        public IQueryable<TUser> Users
        {
            get
            {
                var sql = @"
                SELECT 
                    IU.*, -- identity user
                    IP.* -- identiy profile
                FROM IdentityUser IU
                    INNER JOIN IdentityProfile IP ON IU.UserId = IP.UserId";

                var result = _context.Connection.Query<TUser, Entities.IdentityProfile, TUser>(sql, (user, profile) =>
                {
                    user.Profile = profile;

                    return user;
                }, splitOn: "UserId");

                    return result.AsQueryable();
            }
        }
    }
}