using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal class DatabaseCardDao
    {

        private const string CreateCardTableCommand = @"CREATE TABLE IF NOT EXISTS cards (id varchar PRIMARY KEY, name varchar, name varchar, damage float);";
       // private const string SelectAllUsersCommand = @"SELECT username, password, name, bio, image FROM users";
       // private const string SelectUserByUsernameCommand = "SELECT username, password, name, bio, image FROM users WHERE username=@username";
       // private const string InsertUserCommand = @"INSERT INTO users(username, password) VALUES (@username, @password)";
       // private const string UpdateUserDataCommand = @"UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
        private readonly string _connectionString;
        public DatabaseCardDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        private static void EnsureTables()
        {
            string connectionString = ConnectionString.Get();
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateCardTableCommand, connection);
            cmd.ExecuteNonQuery();

        }
    }
}
