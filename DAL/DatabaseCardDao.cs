using MTCG.HttpServer.Schemas;
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
        private const string CreateCardTableCommand = @"CREATE TABLE IF NOT EXISTS cards (id varchar PRIMARY KEY, name varchar, damage float);";
       // private const string SelectAllUsersCommand = @"SELECT username, password, name, bio, image FROM users";
       // private const string SelectUserByUsernameCommand = "SELECT username, password, name, bio, image FROM users WHERE username=@username";
        private const string InsertCardCommand = @"INSERT INTO cards(id, name, damage) VALUES (@id, @name, @damage)";
       // private const string UpdateUserDataCommand = @"UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
        private readonly string _connectionString;
        public DatabaseCardDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public void CreateCard(Card card)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(InsertCardCommand, connection);

            cmd.Parameters.AddWithValue("id", card.Id);
            cmd.Parameters.AddWithValue("name", card.Name);
            cmd.Parameters.AddWithValue("damage", card.Damage);

            var affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows == 0)
            {
                //IS THIS STILL WORTH IT
                //Exception to be thrown when intended to create user that is already in the DB
                //throw new DuplicateNameException();
                throw new NpgsqlException();
            }
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
