using MTCG.Interfaces;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Classes;
using System.Runtime.Remoting.Messaging;
using MTCG.HttpServer.Schemas;

namespace MTCG.DAL
{
    internal class DatabaseUserDao
    {
        public DatabaseUserDao() 
        {
            EnsureTables();
        }

        public UserTable GetUserByUsername(string Username)
        {

            string connectionString = ConnectionString.Get();

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = "SELECT * FROM usertable WHERE username=@username";
                    command.AddParameterWithValue("username", DbType.String , Username);
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string username = reader.GetString(0);
                            string password = reader.GetString(1);
                            string name = reader.GetString(2);
                            string bio = reader.GetString(3);
                            string image = reader.GetString(4);

                            return new UserTable(username, password, name, bio, image);
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
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Usertable (
                            username varchar(40) PRIMARY KEY,
                            password varchar(30) NOT NULL,
                            name varchar(100) DEFAULT '', 
                            bio varchar (100) DEFAULT '',
                            image varchar (15) DEFAULT ''
                        )";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
