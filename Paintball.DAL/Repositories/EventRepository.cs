using Dapper;
using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL.Repositories
{
    public class EventsRepository : IRepository<Event, int>
    {
        private IDbContext _context;
        public EventsRepository(IDbContext context)
        {
            _context = context;
        }

        public Event CreateOrUpdate(Event entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Event entity can`t be null");

            if (entity.GameId <= 0)
                throw new ArgumentException("GameId can`t be zero or below");

            if (entity.CompanyId <= 0)
                throw new ArgumentException("CompanyId can`t be zero or below");

            if (string.IsNullOrEmpty(entity.Description))
                throw new ArgumentException("Description must be set");

            if (string.IsNullOrEmpty(entity.Title))
                throw new ArgumentException("Title must be set");

            if (string.IsNullOrEmpty(entity.ShortDescription))
                throw new ArgumentException("ShortDescription must be set");
            
            if (_context.Connection.ExecuteScalar<bool>("select count(1) from Events where Id=@id", new { id = entity.Id }))
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
                    GameId = entity.GameId,
                    Description = entity.Description,
                    CompanyId = entity.CompanyId,
                    Title = entity.Title,
                    TitleImage = entity.TitleImage,
                    ShortDescription = entity.ShortDescription
                });
                // New
                _context.Connection.Execute("Event_Create", param, commandType: System.Data.CommandType.StoredProcedure);
                entity.Id = param.Get<int>("@Id");
            }

            return entity;
        }

        public bool Delete(int key)
        {
            int affectedRows = _context.Connection.Execute("Event_Delete", new { Id = key },
                commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public IEnumerable<Event> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0");

            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be grater than 0");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM Events) AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Event>(query, new { PageSize = pageSize, PageNumber = pageNumber });
        }

        public IEnumerable<Event> Search(string searchQuery, object data, bool descending = false)
        {
            if (string.IsNullOrEmpty(searchQuery))
                throw new ArgumentNullException("SearchQuery can`t be empty");

            if (data == null)
                throw new ArgumentNullException("Data can`t be null");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM Events WHERE " + searchQuery + ") AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Event>(query, data);
        }

        public Event Read(int key)
        {
            return _context.Connection.Query<Event>("SELECT * FROM Events WHERE Id = @id", new { id = key }).FirstOrDefault();
        }

        public bool Update(Event entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Event can`t be null");

            if (entity.Id <= 0)
                throw new ArgumentException("Event doesn`t exists");

            int affectedRows = _context.Connection.Execute("Event_Update", new
            {
                Id = entity.Id,
                Description = entity.Description,
                Title = entity.Title,
                TitleImage = entity.TitleImage,
                ShortDescription = entity.ShortDescription
            }, commandType: System.Data.CommandType.StoredProcedure);
            return affectedRows > 0;
        }

        public int Count()
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Events");
        }

        public int Count(string query, object data)
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Events WHERE " + query, data);
        }

        public IEnumerable<int> GetAllIds()
        {
            return _context.Connection.Query<int>("SELECT Id FROM Events");
        }
    }
}
