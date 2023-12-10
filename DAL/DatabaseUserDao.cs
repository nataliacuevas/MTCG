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
        public DatabaseUserDao() 
        {
            EnsureTables();
        }

        private static void EnsureTables()
        {
            string connectionString = ConnectionString.Get();
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
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
