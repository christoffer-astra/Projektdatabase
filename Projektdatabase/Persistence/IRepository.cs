using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Projektdatabase.Persistence
{
    public interface IRepository<T> where T : class
    {
        T Get(int id, IDbConnectionFactory conn);
        int GetId(string name, IDbConnectionFactory conn);
        List<T> GetAll(IDbConnectionFactory conn);
    }
}
