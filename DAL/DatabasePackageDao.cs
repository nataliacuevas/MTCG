using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.API.Routing.Packages;
using MTCG.HttpServer.Schemas;
using Npgsql;

namespace MTCG.DAL
{
    
    internal class DatabasePackageDao
    {
        private const string CreatePackagesTableCommand = @"CREATE TABLE IF NOT EXISTS packages (card1 varchar FOREIGN KEY, password varchar, name varchar DEFAULT '', bio varchar DEFAULT '', image varchar DEFAULT '');";
       /* private const string SelectAllUsersCommand = @"SELECT username, password, name, bio, image FROM users";
        private const string SelectUserByUsernameCommand = "SELECT username, password, name, bio, image FROM users WHERE username=@username";
        private const string InsertUserCommand = @"INSERT INTO users(username, password) VALUES (@username, @password)";
        private const string UpdateUserDataCommand = @"UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
       */
        private readonly string _connectionString;
        public DatabaseUserDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        private static void EnsureTables()
        {
            string connectionString = ConnectionString.Get();
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreatePackageTableCommand, connection);
            cmd.ExecuteNonQuery();

        }

    }
