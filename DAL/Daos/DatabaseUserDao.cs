﻿using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MTCG.DAL
{
    public class DatabaseUserDao : IUserDao
    {
        private const string CreateUserTableCommand = @"CREATE TABLE IF NOT EXISTS users (username varchar PRIMARY KEY, password varchar, name varchar DEFAULT '', bio varchar DEFAULT '', image varchar DEFAULT '', coins int DEFAULT 20, elo int DEFAULT 100, wins int DEFAULT 0, losses int DEFAULT 0);";
        private const string SelectAllUsersCommand = @"SELECT username, password, name, bio, image, coins, elo, wins, losses FROM users ORDER BY elo DESC";
        private const string SelectUserByUsernameCommand = "SELECT username, password, name, bio, image, coins, elo, wins, losses FROM users WHERE username=@username";
        private const string InsertUserCommand = @"INSERT INTO users(username, password) VALUES (@username, @password)";
        private const string UpdateUserDataCommand = @"UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
        private const string UpdateUserCoinsCommand = @"UPDATE users SET coins = @coins WHERE username = @username";
        private const string UpdateUserEloWinsLossesCommand = @"UPDATE users SET elo = @elo, wins = @wins, losses = @losses WHERE username = @username";

        private readonly string _connectionString;
        public DatabaseUserDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        public User SelectUserByUsername(string Username)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectUserByUsernameCommand, connection);

            cmd.Parameters.AddWithValue("username", Username);
            using (IDataReader reader = cmd.ExecuteReader())

                if (reader.Read())
                {
                    return ReadUser(reader);
                }
                else
                {
                    //instead of using exception, function returns null, meaning that the username is not in the DB
                    return null;
                }
        }
        public void CreateUser(UserCredentials userCredentials)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(InsertUserCommand, connection);

            cmd.Parameters.AddWithValue("username", userCredentials.Username);
            cmd.Parameters.AddWithValue("password", userCredentials.Password);

            var affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows == 0)
            {
                //Exception to be thrown when intended to create user that is already in the DB
                throw new DuplicateNameException();
            }
        }
        public IEnumerable<User> GetAllUsers()
        {
            // TODO: handle exceptions
            var users = new List<User>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectAllUsersCommand, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var user = ReadUser(reader);
                users.Add(user);
            }

            return users;
        }
        private User ReadUser(IDataRecord record)
        {
            //the ! is for the compiler to treat the result as non-nullable 
            var username = Convert.ToString(record["username"])!;
            var password = Convert.ToString(record["password"])!;
            var name = Convert.ToString(record["name"])!;
            var bio = Convert.ToString(record["bio"])!;
            var image = Convert.ToString(record["image"])!;
            var coins = Convert.ToInt32(record["coins"])!;
            var elo = Convert.ToInt32(record["elo"])!;
            var wins = Convert.ToInt32(record["wins"])!;
            var losses = Convert.ToInt32(record["losses"])!;

            return new User(username, password, name, bio, image, coins, elo, wins, losses);
        }
        //This will return null if the token doesnt correspond to any user
        public User GetUserByAuthToken(string authToken)
        {
            //GetAllUsers() = gets a list of all the users in the DB
            //SingleOrDefault = returns the only users that satisfy the condition
            return GetAllUsers().SingleOrDefault(u => u.Token == authToken);
        }


        public void UpdateUserData(UserData userdata, User user)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(UpdateUserDataCommand, connection);

            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("name", userdata.Name);
            cmd.Parameters.AddWithValue("bio", userdata.Bio);
            cmd.Parameters.AddWithValue("image", userdata.Image);
            cmd.ExecuteNonQuery();

        }
        public void UpdateUserCoins(User user, int newValue)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(UpdateUserCoinsCommand, connection);

            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("coins", newValue);
            cmd.ExecuteNonQuery();

        }

        public User LoginUser(UserCredentials credentials)
        {

            if (SelectUserByUsername(credentials.Username).Password != credentials.Password)
            {
                return null;

            }
            return SelectUserByUsername(credentials.Username);
        }

        public void UpdateEloWinsLosses(User user)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(UpdateUserEloWinsLossesCommand, connection);

            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("elo", user.Elo);
            cmd.Parameters.AddWithValue("wins", user.Wins);
            cmd.Parameters.AddWithValue("losses", user.Losses);
            cmd.ExecuteNonQuery();

        }
        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateUserTableCommand, connection);
            cmd.ExecuteNonQuery();

        }


    }

}
