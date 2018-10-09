using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL.Repositories
{
    public interface IRepository<TEntity, TKey>
    {
        IEnumerable<TKey> GetAllIds();
        IEnumerable<TEntity> GetAll(int pageSize = 20, int pageNumber = 1, bool descending = false);

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="searchQuery">Without WHERE statement</param>
        /// <param name="data">must contains: new { PageSize = pageSize, PageNumber = pageNumber }</param>
        /// <returns></returns>
        IEnumerable<TEntity> Search(string searchQuery, object data, bool descending = false); 
        
        TEntity CreateOrUpdate(TEntity entity);

        TEntity Read(TKey key);

        bool Update(TEntity entity);

        bool Delete(TKey key);

        int Count();

        int Count(string query, object data);
    }
}
