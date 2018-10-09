using Dapper;
using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL.Repositories
{
    public class OperationsRepository : IRepository<Operation, int>
    {
        private IDbContext _context;
        public OperationsRepository(IDbContext context)
        {
            _context = context;
        }
        public Operation CreateOrUpdate(Operation entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Operation entity are null");

            if (entity.Price == 0)
                throw new ArgumentException("Price can`t be zero");

            if (entity.Date.Year < 1753)
                throw new ArgumentException("Date must be set");

            if (entity.CompanyId <= 0)
                throw new ArgumentException("CompanyId must be set");

            if (_context.Connection.ExecuteScalar<bool>("select count(1) from Operations where Id=@id", new { id = entity.Id }))
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
                    Price = entity.Price,
                    Date = entity.Date,
                    CompanyId = entity.CompanyId,
                    Description = entity.Description,
                    GameId = entity.GameId,
                    CertificateId = entity.CertificateId,
                    StaffId = entity.StaffId
                });
                // New
                _context.Connection.Execute("Operations_Create", param, commandType: System.Data.CommandType.StoredProcedure);
                entity.Id = param.Get<int>("@Id");
            }

            return entity;
        }

        public bool Delete(int key)
        {
            int affectedRows = _context.Connection.Execute("Operations_Delete", new { Id = key },
                commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public IEnumerable<Operation> Search(string searchQuery, object data, bool descending = false)
        {
            if (string.IsNullOrEmpty(searchQuery))
                throw new ArgumentNullException("SearchQuery can`t be empty");

            if (data == null)
                throw new ArgumentNullException("Data can`t be null");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM Operations WHERE " + searchQuery + ") AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Operation>(query, data);
        }

        public IEnumerable<Operation> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0");

            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be grater than 0");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM Operations) AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Operation>(query, new { PageSize = pageSize, PageNumber = pageNumber });
        }

        public Operation Read(int key)
        {
            return _context.Connection.Query<Operation>("SELECT * FROM Operations WHERE Id = @id", new { id = key }).FirstOrDefault();
        }

        public bool Update(Operation entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Operation can`t be null");

            if (entity.Id <= 0)
                throw new ArgumentException("Operation doesn`t exists");

            int affectedRows = _context.Connection.Execute("Operations_Update", new
            {
                Id = entity.Id,
                Price = entity.Price,
                Date = entity.Date,
                Description = entity.Description
            }, commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public int Count()
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Operations");
        }

        public int Count(string query, object data)
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Operations WHERE " + query, data);
        }

        public IEnumerable<int> GetAllIds()
        {
            return _context.Connection.Query<int>("SELECT Id FROM Operations");
        }
    }
}
