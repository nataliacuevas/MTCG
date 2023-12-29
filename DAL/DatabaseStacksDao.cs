using MTCG.HttpServer.Schemas;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MTCG.DAL
{
    internal class DatabaseStacksDao
    {
        private const string CreateStacksTableCommand = @"CREATE TABLE IF NOT EXISTS stacks (username varchar REFERENCES users(username), card_id varchar REFERENCES cards(id), PRIMARY KEY (username, card_id));";
        private const string InsertCardsCommand = @"INSERT INTO stacks(username, card_id) VALUES (@username, @card_id)";
        private const string SelectCardsByUsernameCommand = "SELECT card_id FROM stacks WHERE username=@username";

        /* private const string SelectAllUsersCommand = @"SELECT username, password, name, bio, image FROM users";
         private const string UpdateUserDataCommand = @"UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
        */
        private readonly string _connectionString;
        public DatabaseStacksDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public void AddPackageToUser(User user, Package package)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(InsertCardsCommand, connection);
            List<string> cardsIds = new List<string>();
            cardsIds.Add(package.Card1Id);
            cardsIds.Add(package.Card2Id);
            cardsIds.Add(package.Card3Id);
            cardsIds.Add(package.Card4Id);
            cardsIds.Add(package.Card5Id);

            foreach (var cardId in cardsIds)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("card_id", cardId);
                Console.WriteLine("card id: {0}", cardId.ToString());
                var affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows == 0)
                {
                    //Exception to be thrown when intended to add a card that is already in the DB
                    throw new DuplicateNameException();
                }
            }
        }
        public List<string> SelectCardsByUsername(string username) 
        {
            List<string> cardsIds = new List<string>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectCardsByUsernameCommand, connection);

            cmd.Parameters.AddWithValue("username", username);
            
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var cardId = reader.GetString(0);
                cardsIds.Add(cardId);
            }

            return cardsIds;
        }

        private static void EnsureTables()
        {
            string connectionString = ConnectionString.Get();
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateStacksTableCommand, connection);
            cmd.ExecuteNonQuery();

        }
    }
}
