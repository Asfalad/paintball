using Dapper;
using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL.Repositories
{
    public class CompaniesRepository : IRepository<Company, int>
    {
        private IDbContext _context;

        public CompaniesRepository(IDbContext context)
        {
            _context = context;
        }

        public Company CreateOrUpdate(Company entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Company entity are null");

            if (string.IsNullOrEmpty(entity.Name))
                throw new ArgumentException("Company Name can`t be empty");

            if (string.IsNullOrEmpty(entity.Adress))
                throw new ArgumentException("Company Adress can`t be empty");

            if (entity.OwnerId == Guid.Empty)
                throw new ArgumentException("Company OwnerId can`t be empty");

            if (_context.Connection.ExecuteScalar<bool>("select count(1) from Companies where Id=@id", new { id = entity.Id }))
            {
                // Exists
                Update(entity);
            }
            else
            {

                var param = new DynamicParameters();
                param.Add("@Id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                param.AddDynamicParams(new
                {
                    Name = entity.Name,
                    Adress = entity.Adress,
                    OwnerId = entity.OwnerId,
                    Description = entity.Description,
                    LogoImage = entity.LogoImage
                });
                // New
                _context.Connection.Execute("Company_Create", param, commandType: System.Data.CommandType.StoredProcedure);
                entity.Id = param.Get<int>("@Id");
            }

            return entity;
        }

        public bool Delete(int key)
        {
            int affectedRows = _context.Connection.Execute("Company_Delete", new { Id = key }, 
                commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public IEnumerable<Company> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0");

            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be grater than 0");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum,"+
                " * FROM Companies ) AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Company>(query, new { PageSize = pageSize, PageNumber = pageNumber });
        }

        public IEnumerable<Company> Search(string searchQuery, object data, bool descending = false)
        {
            if (string.IsNullOrEmpty(searchQuery))
                throw new ArgumentNullException("SearchQuery can`t be empty");

            if (data == null)
                throw new ArgumentNullException("Data can`t be null");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum,"+
                " * FROM Companies WHERE " + searchQuery + ") AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Company>(query, data);
        }

        public Company Read(int key)
        {
            return _context.Connection.Query<Company>("SELECT * FROM Companies WHERE Id = @id", new { id = key }).FirstOrDefault();
        }

        public bool Update(Company entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Company entity can`t be null");

            if (entity.Id <= 0)
                throw new ArgumentException("Companies doesn`t exists");

            int affectedRows = _context.Connection.Execute("Company_Update", new {
                Id = entity.Id,
                Name = entity.Name,
                Adress = entity.Adress,
                Description = entity.Description,
                LogoImage = entity.LogoImage
            }, commandType: System.Data.CommandType.StoredProcedure);
            return affectedRows > 0;
        }

        public int Count()
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Companies");
        }

        public int Count(string query, object data)
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Companies WHERE " + query, data);
        }

        public IEnumerable<int> GetAllIds()
        {
            return _context.Connection.Query<int>("SELECT Id FROM Companies");
        }
    }
}
