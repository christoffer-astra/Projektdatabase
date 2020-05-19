using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Projektdatabase.Models;

namespace Projektdatabase.Persistence
{
    class ProjektRepository : IProjektRepository
    {
        public ProjektRepository()
        {
        }

        public ProjektModel GetModel(int id)
        {
            return null;
        }

        public List<ProjektModel> GetAll(IDbConnectionFactory conn)
        {
            List<ProjektModel> projektList = new List<ProjektModel>();
            using (var connect = conn.CreateConnection())
            {
                projektList = connect.Query<ProjektModel>("Select * FROM Projekt").ToList();
            }

            return projektList;
        }
    }
}