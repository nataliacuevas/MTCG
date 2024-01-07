using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Schemas;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace MTCG.DAL
{
    public class DatabaseCardDao : ICardDao
    {
        private const string CreateCardTableCommand = @"CREATE TABLE IF NOT EXISTS cards (id varchar PRIMARY KEY, name varchar, damage float);";
        private const string SelectCardByIdCommand = "SELECT id, name, damage FROM cards WHERE id=@id";
        private const string InsertCardCommand = @"INSERT INTO cards(id, name, damage) VALUES (@id, @name, @damage)";
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
                    return null;
                }
            }


        }
        private Card ReadCard(IDataRecord record)
        {
            //the ! is for the compiler to treat the result as non-nullable 
            var card_id = Convert.ToString(record["id"])!;
            var name = Convert.ToString(record["name"])!;
            var damage = Convert.ToDouble(record["damage"])!;

            var card = new Card();
            card.Id = card_id;
            card.Name = name;
            card.Damage = damage;

            return card;
        }

        public List<Card> GetCardsByIdList(List<string> ids)
        {
            List<Card> cards = new List<Card>();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectCardByIdCommand, connection);


            foreach (var card_id in ids)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("id", card_id);


                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cards.Add(ReadCard(reader));
                }
            }
            return cards;
        }
        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateCardTableCommand, connection);
            cmd.ExecuteNonQuery();

        }


    }
}
