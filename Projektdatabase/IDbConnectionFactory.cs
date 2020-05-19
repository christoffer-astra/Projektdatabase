using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Projektdatabase
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
