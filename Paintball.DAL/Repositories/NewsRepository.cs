using Dapper;
using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL.Repositories
{
    public class NewsRepository : IRepository<News, int>
    {
        private IDbContext _context;
        public NewsRepository(IDbContext context)
        {
            _context = context;
        }

        public News CreateOrUpdate(News entity)
        {
            if (entity == null)
                throw new ArgumentNullException("News entity can`t be null");

            if (string.IsNullOrEmpty(entity.Title))
                throw new ArgumentException("Title can`t be empty");

            if (entity.PublishDate.Year < 1753)
                throw new ArgumentException("Publish date must be set");

            if (string.IsNullOrEmpty(entity.Text))
                throw new ArgumentException("Text can`t be empty");

            if (string.IsNullOrEmpty(entity.ShortDescription))
                throw new ArgumentException("Short description must be set");

            if (_context.Connection.ExecuteScalar<bool>("select count(1) from News where Id=@id", new { id = entity.Id }))
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
                    Title = entity.Title,
                    AuthorId = entity.AuthorId,
                    PublishDate = entity.PublishDate,
                    Text = entity.Text,
                    CompanyId = entity.CompanyId,
                    EditDate = entity.EditDate,
                    TitleImage = entity.TitleImage,
                    ShortDescription = entity.ShortDescription
                });
                // New
                _context.Connection.Execute("News_Create", param, 
                    commandType: System.Data.CommandType.StoredProcedure);
                entity.Id = param.Get<int>("@Id");
            }

            return entity;
        }

        public bool Delete(int key)
        {
            int affectedRows = _context.Connection.Execute("News_Delete", new { Id = key },
                commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public IEnumerable<News> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0");

            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be grater than 0");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM News) AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<News>(query, new { PageSize = pageSize, PageNumber = pageNumber });
        }

        public IEnumerable<News> Search(string searchQuery, object data, bool descending = false)
        {
            if (string.IsNullOrEmpty(searchQuery))
                throw new ArgumentNullException("SearchQuery can`t be empty");

            if (data == null)
                throw new ArgumentNullException("Data can`t be null");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM News WHERE " + searchQuery + ") AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<News>(query, data);
        }

        public News Read(int key)
        {
            return _context.Connection.Query<News>("SELECT * FROM News WHERE Id = @id", new { id = key }).FirstOrDefault();
        }

        public bool Update(News entity)
        {
            if (entity == null)
                throw new ArgumentNullException("News can`t be null");

            if (entity.Id <= 0)
                throw new ArgumentException("News doesn`t exists");

            int affectedRows = _context.Connection.Execute("News_Update", new {
                Id = entity.Id,
                Title = entity.Title,
                Text = entity.Text,
                EditDate = entity.EditDate,
                TitleImage = entity.TitleImage,
                ShortDescription = entity.ShortDescription
            }, commandType: System.Data.CommandType.StoredProcedure);
            return affectedRows > 0;
        }

        public int Count()
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM News");
        }

        public int Count(string query, object data)
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM News WHERE " + query, data);
        }

        public IEnumerable<int> GetAllIds()
        {
            return _context.Connection.Query<int>("SELECT Id FROM News");
        }
    }
}
