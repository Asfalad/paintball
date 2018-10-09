using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paintball.DAL
{
    public interface IDbContext : IDisposable
    {
        IDbConnection Connection { get; set; }
    }
}
