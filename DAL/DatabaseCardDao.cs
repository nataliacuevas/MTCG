using MTCG.HttpServer.Schemas;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    internal class DatabaseCardDao
    {
        private const string CreateCardTableCommand = @"CREATE TABLE IF NOT EXISTS cards (id varchar PRIMARY KEY, name varchar, damage float);";
       // private const string SelectAllUsersCommand = @"SELECT username, password, name, bio, image FROM users";
        private const string SelectCardByIdCommand = "SELECT id, name, damage FROM cards WHERE id=@id";
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
                //Is this still worth it? 
                //Copy pasted from sample code 
                throw new NpgsqlException();
            }
        }

        public void CreateCards(List<Card> cards)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            foreach (var card in cards)
            {
                using var cmd = new NpgsqlCommand(InsertCardCommand, connection);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("id", card.Id);
                cmd.Parameters.AddWithValue("name", card.Name);
                cmd.Parameters.AddWithValue("damage", card.Damage);

                var affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows == 0)
                {
                    //Is this still worth it? 
                    // copy pasted from Message_Server code
                    throw new NpgsqlException();
                }
            }
        }
        public Card GetCardbyId(string id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectCardByIdCommand, connection);

            cmd.Parameters.AddWithValue("id", id);
            using (IDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var card = new Card();
                    card.Id = reader.GetString(0);
                    card.Name = reader.GetString(1);
                    card.Damage = reader.GetDouble(2);

                    return card;
                }
                else
                {
                    //instead of using exception, function returns null, meaning that the card is not in the DB
                    return null;
                }
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
