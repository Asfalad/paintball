using Dapper;
using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL.Repositories
{
    public class EquipmentRepository : IRepository<Equipment, int>
    {
        private IDbContext _context;
        public EquipmentRepository(IDbContext context)
        {
            _context = context;
        }

        public Equipment CreateOrUpdate(Equipment entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Equipment entity can`t be null");

            if (string.IsNullOrEmpty(entity.Name))
                throw new ArgumentException("Equipment name can`t be empty");

            if (entity.Price < 0)
                throw new ArgumentException("Equipment price can`t be below zero");

            if (entity.Count < 0)
                throw new ArgumentException("Equipment count can`t be below zero");

            if (entity.CompanyId <= 0)
                throw new ArgumentException("CompanyId can`t be below or equal to zero");
            
            if (_context.Connection.ExecuteScalar<bool>("select count(1) from Equipment where Id=@id", new { id = entity.Id }))
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
                    Price = entity.Price,
                    State = entity.State,
                    Count = entity.Count,
                    CompanyId = entity.CompanyId
                });
                // New
                _context.Connection.Execute("Equipment_Create", param, commandType: System.Data.CommandType.StoredProcedure);
                entity.Id = param.Get<int>("@Id");
            }

            return entity;
        }

        public bool Delete(int key)
        {
            int affectedRows = _context.Connection.Execute("Equipment_Delete", new { Id = key },
                commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public IEnumerable<Equipment> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0");

            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be grater than 0");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM Equipment) AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Equipment>(query, new { PageSize = pageSize, PageNumber = pageNumber });
        }

        public IEnumerable<Equipment> Search(string searchQuery, object data, bool descending = false)
        {
            if (string.IsNullOrEmpty(searchQuery))
                throw new ArgumentNullException("SearchQuery can`t be empty");

            if (data == null)
                throw new ArgumentNullException("Data can`t be null");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM Equipment WHERE " + searchQuery + ") AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Equipment>(query, data);
        }

        public Equipment Read(int key)
        {
            return _context.Connection.Query<Equipment>("SELECT * FROM Equipment WHERE Id = @id", new { id = key }).FirstOrDefault();
        }

        public bool Update(Equipment entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Equipment can`t be null");

            if (entity.Id <= 0)
                throw new ArgumentException("Equipment doesn`t exists");


            int affectedRows = _context.Connection.Execute("Equipment_Update", new
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                State = entity.State,
                Count = entity.Count
            }, commandType: System.Data.CommandType.StoredProcedure);
            return affectedRows > 0;
        }

        public int Count()
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Equipment");
        }

        public int Count(string query, object data)
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Equipment WHERE " + query, data);
        }

        public IEnumerable<int> GetAllIds()
        {
            return _context.Connection.Query<int>("SELECT Id FROM Equipment");
        }
    }
}
