using MTCG.HttpServer.Schemas;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL
{
    internal class DatabaseUserDao
    {
        private const string CreateUserTableCommand = @"CREATE TABLE IF NOT EXISTS users (username varchar PRIMARY KEY, password varchar, name varchar DEFAULT '', bio varchar DEFAULT '', image varchar DEFAULT '');";
        private const string SelectAllUsersCommand = @"SELECT username, password FROM users";
        private const string SelectUserByCredentialsCommand = "SELECT username, password FROM users WHERE username=@username AND password=@password";
        private const string InsertUserCommand = @"INSERT INTO users(username, password) VALUES (@username, @password)";
        private readonly string _connectionString;
        public DatabaseUserDao(string connectionString) 
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        public User GetUserByUsername(string Username)
        {

            string connectionString = ConnectionString.Get();

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = "SELECT * FROM usertable WHERE username=@username";
                    command.AddParameterWithValue("username", DbType.String, Username);
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string username = reader.GetString(0);
                            string password = reader.GetString(1);
                            string name = reader.GetString(2);
                            string bio = reader.GetString(3);
                            string image = reader.GetString(4);

                            return new User(username, password, name, bio, image);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

            }
        }
        public void CreateUser(UserCredentials userCredentials)
        {
            string connectionString = ConnectionString.Get();
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = @"INSERT INTO usertable (username, password)
                                            VALUES (@username, @password)";

                    command.AddParameterWithValue("username", DbType.String, userCredentials.Username);
                    command.AddParameterWithValue("password", DbType.String, userCredentials.Password);

                    command.ExecuteNonQuery();
                }
            }
        }
        private static void EnsureTables()
        {
            string connectionString = ConnectionString.Get();
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateUserTableCommand, connection);
            cmd.ExecuteNonQuery();

        }


    }

}
