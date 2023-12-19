using MTCG.Interfaces;
using MTCG.Models;
using MTCG.HttpServer.Schemas;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Json.Net;
using MTCG.BLL;
using System.Net;

namespace MTCG.DAL
{
    internal class DatabaseUserDao
    {
        private const string CreateUserTableCommand = @"CREATE TABLE IF NOT EXISTS users (username varchar PRIMARY KEY, password varchar, name varchar DEFAULT '', bio varchar DEFAULT '', image varchar DEFAULT '');";
        private const string SelectAllUsersCommand = @"SELECT username, password, name, bio, image FROM users";
        private const string SelectUserByUsernameCommand = "SELECT username, password, name, bio, image FROM users WHERE username=@username";
        private const string InsertUserCommand = @"INSERT INTO users(username, password) VALUES (@username, @password)";
        private const string UpdateUserDataCommand = @"UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
        private readonly string _connectionString;
        public DatabaseUserDao(string connectionString) 
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        public User SelectUserByUsername(string Username)
        {
            //TODO: HANDLE EXCEPTIONS!!

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectUserByUsernameCommand, connection);

            cmd.Parameters.AddWithValue("username", Username);
            using (IDataReader reader = cmd.ExecuteReader())
            
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
        private IEnumerable<User> GetAllUsers()
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

            return new User(username, password, name, bio, image);
        }
        //This will return null if the token doesnt correspond to any user
        public User GetUserByAuthToken(string authToken)
        {
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

            var affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows > 0)
            {
                Console.WriteLine("User data updated successfully.");
            }
            else
            {
                Console.WriteLine("No rows were updated. User may not exist or data is the same.");
            }
        }

        public User LoginUser(UserCredentials credentials)
        {

            if(SelectUserByUsername(credentials.Username).Password != credentials.Password)
            {
                throw new UserNotFoundException();

            }
            return SelectUserByUsername(credentials.Username);

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
