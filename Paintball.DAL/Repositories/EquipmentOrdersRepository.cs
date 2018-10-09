using Dapper;
using Paintball.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL.Repositories
{
    public class EquipmentOrdersRepository : IRepository<EquipmentOrder, int>
    {
        private IDbContext _context;

        public EquipmentOrdersRepository(IDbContext _context)
        {
            this._context = _context;
        }

        public int Count()
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM EquipmentOrders");
        }

        public int Count(string query, object data)
        {
            return _context.Connection.QueryFirst<int>("SELECT COUNT(*) FROM EquipmentOrders WHERE " + query, data);
        }

        public EquipmentOrder CreateOrUpdate(EquipmentOrder entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Certificate entity are null");

            if (entity.EquipmentId <= 0)
                throw new ArgumentException("EquipmentId must be set");

            if (entity.GameId <= 0)
                throw new ArgumentException("GameId must be set");

            if (entity.Count <= 0)
                throw new ArgumentException("Count can`t be zero or below");

            if (_context.Connection.ExecuteScalar<bool>("select count(1) from EquipmentOrders where Id=@id", new { id = entity.Id }))
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
                    EquipmentId = entity.EquipmentId,
                    Count = entity.Count
                });
                // New
                _context.Connection.Execute("EquipmentOrder_Create", param,
                    commandType: System.Data.CommandType.StoredProcedure);

                entity.Id = param.Get<int>("@Id");
            }

            return entity;
        }

        public bool Delete(int key)
        {
            int affectedRows = _context.Connection.Execute("EquipmentOrder_Delete", new { Id = key },
                commandType: System.Data.CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public IEnumerable<EquipmentOrder> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than 0");

            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be grater than 0");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM EquipmentOrders) AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<EquipmentOrder>(query, new { PageSize = pageSize, PageNumber = pageNumber });
        }

        public IEnumerable<int> GetAllIds()
        {
            return _context.Connection.Query<int>("SELECT Id FROM EquipmentOrders");
        }

        public EquipmentOrder Read(int key)
        {
            return _context.Connection.Query<EquipmentOrder>("SELECT * FROM EquipmentOrders WHERE Id = @id", new { id = key }).FirstOrDefault();
        }

        public IEnumerable<EquipmentOrder> Search(string searchQuery, object data, bool descending = false)
        {
            if (string.IsNullOrEmpty(searchQuery))
                throw new ArgumentNullException("SearchQuery can`t be empty");

            if (data == null)
                throw new ArgumentNullException("Data can`t be null");

            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER ( ORDER BY Id " + (descending ? "DESC " : "") + ") AS RowNum, * FROM EquipmentOrders WHERE " + searchQuery + ") AS RowConstrainedResult " +
                "WHERE RowNum >= (@PageSize * (@PageNumber - 1)) + 1 AND RowNum <= (@PageSize * @PageNumber) ORDER BY RowNum";

            return _context.Connection.Query<EquipmentOrder>(query, data);
        }

        public bool Update(EquipmentOrder entity)
        {
            if (entity == null)
                throw new ArgumentNullException("EquipmentOrders can`t be null");

            if (entity.Id <= 0)
                throw new ArgumentException("Id must be set");

            int affectedRows = _context.Connection.Execute("EquipmentOrder_Update", 
                new { Id = entity.Id, EquipmentId = entity.EquipmentId, Count = entity.Count }, 
                commandType: System.Data.CommandType.StoredProcedure);
            return affectedRows > 0;
            
        }
    }
}
