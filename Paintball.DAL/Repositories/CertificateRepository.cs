using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Paintball.DAL.Repositories
{
    public class CertificateRepository : IRepository<Certificate, int>
    {
        private IDbContext _context;
        public CertificateRepository(IDbContext context)
        {
            _context = context;
        }
        public Certificate CreateOrUpdate(Certificate entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Certificate entity are null");

            if (entity.StartDate.Year < 1753)
                throw new ArgumentException("StartDate are empty");

            if(entity.EndDate.Year < 1753)
                throw new ArgumentException("StartDate are empty");

            if (entity.EndDate <= entity.StartDate)
                throw new ArgumentException("End date can`t be less or equal Start Date");

            if (entity.Price < 0)
                throw new ArgumentException("Price can`t be less zero");

            if (entity.CompanyId <= 0)
                throw new ArgumentException("CompanyId can`t be less or equal zero");

            if (string.IsNullOrEmpty(entity.FirstName) || string.IsNullOrEmpty(entity.LastName))
                throw new ArgumentException("FirstName or LastName must be set");

            if (_context.Connection.ExecuteScalar<bool>("select count(1) from Certificates where Id=@id", new { id = entity.Id }))
            {
                // Exists
                Update(entity);
            }
            else
            {
                var param = new DynamicParameters();
                param.Add("@Id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                param.AddDynamicParams(new { FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Price = entity.Price,
                CompanyId = entity.CompanyId,
                OwnerId = entity.OwnerId});
                // New
                _context.Connection.Execute("Certificate_Create", param, commandType: System.Data.CommandType.StoredProcedure);
                entity.Id = param.Get<int>("@Id");
            }

            return entity;
        }

        public bool Delete(int key)
        {
            int affectedRows = _context.Connection.Execute("Certificate_Delete", new { Id = key },
                commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public IEnumerable<Certificate> Search(string searchQuery, object data, bool descending = false)
        {
            if (string.IsNullOrEmpty(searchQuery))
                throw new ArgumentNullException("SearchQuery can`t be empty");

            if (data == null)
                throw new ArgumentNullException("Data can`t be null");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM Certificates WHERE " + searchQuery + ") AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Certificate>(query, data);
        }
        
        public IEnumerable<Certificate> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0");

            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be grater than 0");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC ":"") + ") AS RowNum, * FROM Certificates) AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Certificate>(query, new { PageSize = pageSize, PageNumber = pageNumber });
        }

        public Certificate Read(int key)
        {
            return _context.Connection.Query<Certificate>("SELECT * FROM Certificates WHERE Id = @id", new { id = key }).FirstOrDefault();
        }
        
        public bool Update(Certificate entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Certificate can`t be null");

            if (entity.Id <= 0)
                throw new ArgumentException("Certificate doesn`t exists");
            
            int affectedRows = _context.Connection.Execute("Certificate_Update", new
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                EndDate = entity.EndDate,
                Price = entity.Price
            }, commandType: System.Data.CommandType.StoredProcedure);
            return affectedRows > 0;
        }

        public int Count()
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Certificates");
        }

        public int Count(string query, object data)
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Certificates WHERE " + query, data);
        }

        public IEnumerable<int> GetAllIds()
        {
            return _context.Connection.Query<int>("SELECT Id FROM Certificates");
        }
    }
}
