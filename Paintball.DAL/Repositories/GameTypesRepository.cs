using Dapper;
using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL.Repositories
{
    public class GameTypesRepository : IRepository<GameType, int>
    {
        private IDbContext _context;
        public GameTypesRepository(IDbContext context)
        {
            _context = context;
        }

        public GameType CreateOrUpdate(GameType entity)
        {
            if (entity == null)
                throw new ArgumentNullException("GameType entity can`t be null");

            if (entity.CompanyId <= 0)
                throw new ArgumentException("CompanyId can`t be zero or below");

            if (string.IsNullOrEmpty(entity.Name))
                throw new ArgumentException("Name can`t be null");

            if (entity.Price <= 0)
                throw new ArgumentNullException("Price can`t be null");

            if (_context.Connection.ExecuteScalar<bool>("select count(1) from GameTypes where Id=@id", new { id = entity.Id }))
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
                    Id = entity.Id,
                    Name = entity.Name,
                    CompanyId = entity.CompanyId,
                    Price = entity.Price
                });
                // New
                _context.Connection.Execute("Gametypes_Create", param, commandType: System.Data.CommandType.StoredProcedure);
                entity.Id = param.Get<int>("@Id");
            }

            return entity;
        }

        public bool Delete(int key)
        {
            int affectedRows = _context.Connection.Execute("Gametypes_Delete", new { Id = key },
                commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public IEnumerable<GameType> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0");

            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be grater than 0");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM GameTypes) AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<GameType>(query, new { PageSize = pageSize, PageNumber = pageNumber });
        }

        public IEnumerable<GameType> Search(string searchQuery, object data, bool descending = false)
        {
            if (string.IsNullOrEmpty(searchQuery))
                throw new ArgumentNullException("SearchQuery can`t be empty");

            if (data == null)
                throw new ArgumentNullException("Data can`t be null");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM GameTypes WHERE " + searchQuery + ") AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<GameType>(query, data);
        }

        public GameType Read(int key)
        {
            return _context.Connection.Query<GameType>("SELECT * FROM GameTypes WHERE Id = @id", new { id = key }).FirstOrDefault();
        }

        public bool Update(GameType entity)
        {
            if (entity == null)
                throw new ArgumentNullException("GameType can`t be null");

            if (entity.Id <= 0)
                throw new ArgumentException("GameType doesn`t exists");

            int affectedRows = _context.Connection.Execute("Gametypes_Update", new
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price
            }, commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public int Count()
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM GameTypes");
        }

        public int Count(string query, object data)
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM GameTypes WHERE " + query, data);
        }

        public IEnumerable<int> GetAllIds()
        {
            return _context.Connection.Query<int>("SELECT Id FROM GameTypes");
        }
    }
}
