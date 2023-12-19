﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.API.Routing.Packages;
using MTCG.HttpServer.Schemas;
using Npgsql;

namespace MTCG.DAL
{

    internal class DatabasePackagesDao
    {
        private const string CreatePackagesTableCommand = @"CREATE TABLE IF NOT EXISTS packages (package_id serial PRIMARY KEY, card1_id varchar REFERENCES cards(id), card2_id varchar REFERENCES cards(id), card3_id varchar REFERENCES cards(id), card4_id varchar REFERENCES cards(id), card5_id varchar REFERENCES cards(id));";
        /* private const string SelectAllUsersCommand = @"SELECT username, password, name, bio, image FROM users";
         private const string SelectUserByUsernameCommand = "SELECT username, password, name, bio, image FROM users WHERE username=@username";
         private const string InsertUserCommand = @"INSERT INTO users(username, password) VALUES (@username, @password)";
         private const string UpdateUserDataCommand = @"UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
        */
        private readonly string _connectionString;
        public DatabasePackagesDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        // **********************TO EDIT+++++++++++++++++++++
        public void CreateUser(UserCredentials userCredentials)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreatePackagesTableCommand, connection);

            cmd.Parameters.AddWithValue("username", userCredentials.Username);
            cmd.Parameters.AddWithValue("password", userCredentials.Password);

            var affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows == 0)
            {
                //Exception to be thrown when intended to create user that is already in the DB
                throw new DuplicateNameException();
            }
        }
        private static void EnsureTables()
        {
            string connectionString = ConnectionString.Get();
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreatePackagesTableCommand, connection);
            cmd.ExecuteNonQuery();

        }

    }
}
