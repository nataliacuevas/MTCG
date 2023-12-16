using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal class DatabaseUserDao
    {
        private readonly string _connectionString;
        public DatabaseUserDao(string connectionString) 
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        private void EnsureTables()
        {
            using (IDbConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS User (
                            username varchar(40) PRIMARY KEY,
                            password varchar(30) NOT NULL, 
                        )";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
