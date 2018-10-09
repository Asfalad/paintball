using Dapper;
using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paintball.DAL.Repositories
{
    public class TasksRepository : IRepository<Task, int>
    {
        private IDbContext _context;
        public TasksRepository(IDbContext context)
        {
            _context = context;
        }

        public Task CreateOrUpdate(Task entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Task entity can`t be null");

            if (entity.StaffId == null || entity.StaffId == Guid.Empty)
                throw new ArgumentException("StaffId must be set");

            if (string.IsNullOrEmpty(entity.Text))
                throw new ArgumentException("Text must be set");

            if (entity.CompanyId <= 0)
                throw new ArgumentNullException("Company must be set");

            if (_context.Connection.ExecuteScalar<bool>("select count(1) from Tasks where Id=@id", new { id = entity.Id }))
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
                    StaffId = entity.StaffId,
                    Text = entity.Text,
                    IsCompleted = entity.IsCompleted,
                    CompanyId = entity.CompanyId
                });
                // New
                _context.Connection.Execute("Tasks_Create", param, commandType: System.Data.CommandType.StoredProcedure);

                entity.Id = param.Get<int>("@Id");
            }

            return entity;
        }

        public bool Delete(int key)
        {
            int affectedRows = _context.Connection.Execute("Tasks_Delete", new { Id = key },
                commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public IEnumerable<Task> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0");

            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be grater than 0");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM Tasks) AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Task>(query, new { PageSize = pageSize, PageNumber = pageNumber });
        }

        public IEnumerable<Task> Search(string searchQuery, object data, bool descending = false)
        {
            if (string.IsNullOrEmpty(searchQuery))
                throw new ArgumentNullException("SearchQuery can`t be empty");

            if (data == null)
                throw new ArgumentNullException("Data can`t be null");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM Tasks WHERE " + searchQuery + ") AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<Task>(query, data);
        }

        public Task Read(int key)
        {
            return _context.Connection.Query<Task>("SELECT * FROM Tasks WHERE Id = @id", new { id = key }).FirstOrDefault();
        }

        public bool Update(Task entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Task can`t be null");

            if (entity.Id <= 0)
                throw new ArgumentException("Task doesn`t exists");

            int affectedRows = _context.Connection.Execute("Tasks_Update", new
            {
                Id = entity.Id,
                StaffId = entity.StaffId,
                Text = entity.Text,
                IsCompleted = entity.IsCompleted
            }, commandType: System.Data.CommandType.StoredProcedure);
            return affectedRows > 0;
        }

        public int Count()
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Tasks");
        }

        public int Count(string query, object data)
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM Tasks WHERE " + query, data);
        }

        public IEnumerable<int> GetAllIds()
        {
            return _context.Connection.Query<int>("SELECT Id FROM Tasks");
        }
    }
}
