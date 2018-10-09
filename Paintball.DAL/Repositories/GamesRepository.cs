using Dapper;
using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL.Repositories
{
    public class GamesRepository : IRepository<Game, int>
    {
        private IDbContext _context;
        public GamesRepository(IDbContext context)
        {
            _context = context;
        }

        public Game CreateOrUpdate(Game entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Game entity can`t be null");

            if (entity.CreatorId == Guid.Empty || entity.CreatorId == null)
                throw new ArgumentException("Creator Id can`t be null");

            if (entity.BeginDate.Year < 1753)
                throw new ArgumentException("BeginDate must be set");

            if (entity.GameType <= 0)
                throw new ArgumentException("Game Type must be set");

            if (entity.Playground <= 0)
                throw new ArgumentException("Playground must be set");

            if (entity.PlayerCount <= 0)
                throw new ArgumentException("Players Count can`t be less or equal zero");

            if (entity.GamePrice < 0)
                throw new ArgumentException("Game price can`t be less zero");

            if (entity.CompanyId <= 0)
                throw new ArgumentException("Company must be set");

            if (_context.Connection.ExecuteScalar<bool>("select count(1) from Games where Id=@id", new { id = entity.Id }))
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
                    BeginDate = entity.BeginDate,
                    CompanyId = entity.CompanyId,
                    CreatorId = entity.CreatorId,
                    EndDate = entity.EndDate,
                    GamePrice = entity.GamePrice,
                    GameType = entity.GameType,
                    PlayerCount = entity.PlayerCount,
                    Playground = entity.Playground
                });
                // New
                _context.Connection.Execute("Game_Create", param, commandType: System.Data.CommandType.StoredProcedure);
                entity.Id = param.Get<int>("@Id");
            }

            return entity;
        }

        public bool Delete(int key)
        {
            int affectedRows = _context.Connection.Execute("Game_Delete", new { Id = key },
                commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public IEnumerable<Game> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0");

            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be grater than 0");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, "+
                " * FROM Games) AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Game>(query, new { PageSize = pageSize, PageNumber = pageNumber });
        }

        public IEnumerable<Game> Search(string searchQuery, object data, bool descending = false)
        {
            if (string.IsNullOrEmpty(searchQuery))
                throw new ArgumentNullException("SearchQuery can`t be empty");

            if (data == null)
                throw new ArgumentNullException("Data can`t be null");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM Games WHERE " + searchQuery + ") AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Game>(query, data);
        }

        public Game Read(int key)
        {
            return _context.Connection.Query<Game>("SELECT * FROM Games WHERE Id = @id", new { id = key }).FirstOrDefault();
        }

        public bool Update(Game entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Game can`t be null");

            if (entity.Id <= 0)
                throw new ArgumentException("Game doesn`t exists");

            int affectedRows = _context.Connection.Execute("Game_Update", 
                new
                {
                    Id = entity.Id,
                    BeginDate = entity.BeginDate,
                    EndDate = entity.EndDate,
                    GameType = entity.GameType,
                    Playground = entity.Playground,
                    PlayerCount = entity.PlayerCount,
                    GamePrice = entity.GamePrice
                }, commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public int Count()
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Games");
        }

        public int Count(string query, object data)
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Games WHERE " + query, data);
        }

        public IEnumerable<int> GetAllIds()
        {
            return _context.Connection.Query<int>("SELECT Id FROM Games");
        }
    }
}
