using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL
{
    public class DbContext : IDbContext
    {
        public IDbConnection Connection { get; set; }
        public DbContext()
        {
            Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            Connection.Open();
        }

        public static IDbContext Create()
        {
            return new DbContext();
        }

        public DbContext(string connectionString)
        {
            Connection = new SqlConnection(connectionString);

            Connection.Open();
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(Connection != null)
                {
                    if(Connection.State != ConnectionState.Closed)
                    {
                        Connection.Close();
                    }
                    Connection = null;
                }
            }
        }
    }
}
