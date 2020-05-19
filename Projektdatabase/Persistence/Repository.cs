using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Projektdatabase.Persistence
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly string _table;


        public Repository()
        {
            _table = typeof(T).ToString();
            _table = _table.Replace("Projektdatabase.Models.", "");
            _table = _table.Remove(_table.Length - 5);
        }
        public T Get(int id, IDbConnectionFactory conn)
        {
            using var connect = conn.CreateConnection();
            T t = connect.QueryFirst<T>($"select * from {_table} where {_table}Id = {id}");
            return t;
        }

        public int GetId(string name, IDbConnectionFactory conn)
        {
            using IDbConnection connect = conn.CreateConnection();
            var tableId = connect.QuerySingle<int>($"Select {_table}Id From {_table} where {_table}Name = '{name}'");
            return tableId;
        }

        public List<T> GetAll(IDbConnectionFactory conn)
        {
            List<T> list = new List<T>();
            using (var connect = conn.CreateConnection())
            {
                list = connect.Query<T>("Select uddOmrId, uddOmrName FROM UddOmr").ToList();
            }

            return list;
        }
    }
}