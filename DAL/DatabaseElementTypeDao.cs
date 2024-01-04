using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MTCG.DAL
{
    public class DatabaseElementTypeDao
    {
        private const string CreateDatabaseElementTypeTableCommand = @"CREATE TABLE IF NOT EXISTS elementtype (id serial PRIMARY KEY, name varchar);";

        private void EnsureTables()
        {
            // TODO: handle exceptions
            string connectionString = "TODO"; // ConnectionString.Get();
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreateDatabaseElementTypeTableCommand, connection);
            cmd.ExecuteNonQuery();
        }

    }
}
