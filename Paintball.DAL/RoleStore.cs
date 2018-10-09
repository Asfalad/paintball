using Dapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Paintball.DAL
{
    public class RoleStore<TRole> : IRoleStore<TRole, Guid>, IQueryableRoleStore<TRole, Guid>
        where TRole : DAL.Entities.IdentityRole
    {
        private IDbContext _context;

        public RoleStore(IDbContext context)
        {
            _context = context;
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            string sql = @"
                INSERT INTO [dbo].[IdentityRole]
                           ([RoleId]
                           ,[Name])
                     VALUES
                           (@ROLEID
                           ,@NAME)";

            if (role.RoleId == default(Guid))
                role.RoleId = Guid.NewGuid();

            return Task.FromResult(_context.Connection.Execute(sql, role));
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            string sql = @"
                DELETE FROM IdentityRole 
                WHERE RoleId=@ROLEID";

            return Task.FromResult(_context.Connection.Execute(sql, role));
        }

        public Task<TRole> FindByIdAsync(Guid roleId)
        {
            var sql = @"
                SELECT 
                    *
                FROM 
                    IdentityRole   
                WHERE 
                    RoleId=@ROLEID";

                return Task.FromResult<TRole>(_context.Connection.Query<TRole>(sql, roleId).FirstOrDefault());
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            if (String.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");

            var sql = @"
                SELECT 
                    *
                FROM 
                    IdentityRole   
                WHERE 
                    Name=@NAME";

                return Task.FromResult<TRole>(_context.Connection.Query<TRole>(sql, new { Name = roleName }).FirstOrDefault());
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            string sql = @"
                UPDATE IdentityRole SET 
	                [Name] = @NAME
                WHERE 
                    RoleId = @ROLEID";

                return Task.FromResult(_context.Connection.Execute(sql, role));
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context = null;
            }
        }

        /* IQueryableRoleStore
        ---------------------------*/

        public IQueryable<TRole> Roles
        {
            get
            {
                var sql = @"
                    SELECT 
                        * 
                    FROM IdentityRole";

                var result = _context.Connection.Query<TRole>(sql);

                return result.AsQueryable();
            }
        }
    }
}