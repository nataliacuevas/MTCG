using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.API.Routing.TradingDeals;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Data;
using MTCG.DAL.Interfaces;

namespace MTCG.DAL
{
    public class DatabaseTradingDealsDao : ITradingsDao
    {
        private const string CreateDealsTableCommand = @"CREATE TABLE IF NOT EXISTS tradingDeals (deal_id varchar PRIMARY KEY, card_id varchar REFERENCES cards(id), card_type varchar, minimum_damage float);";
        private const string SelectAllDealsCommand = @"SELECT deal_id, card_id, card_type, minimum_damage FROM tradingDeals";
        private const string CreateDealCommand = @"INSERT INTO tradingDeals (deal_id, card_id, card_type, minimum_damage) VALUES (@deal_id, @card_id, @card_type, @minimum_damage)";
        private const string SelectDealByIdCommand = "SELECT deal_id, card_id, card_type, minimum_damage FROM tradingDeals WHERE deal_id=@deal_id";
        private const string DeleteDealByIdCommand = @"DELETE FROM tradingDeals WHERE deal_id = @deal_id;";
        private const string DeleteMultipleDealsByCardIdCommand = @"DELETE FROM tradingDeals WHERE card_id = @card_id;";
        /*  private const string ConfigureDeckCommand = @"UPDATE stacks SET in_deck = true WHERE card_id = @card_id";
        private const string SelectCardsInDeckByUsernameCommand = "SELECT card_id FROM stacks WHERE username=@username AND in_deck = true";
       
        DELETE FROM tradingDeals WHERE deal_id = 'your_deal_id';
        */

        private readonly string _connectionString;
        public DatabaseTradingDealsDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        public List<TradingDeal> GetAllTradingDeals()
        {
            // TODO: handle exceptions
            var allDeals = new List<TradingDeal>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectAllDealsCommand, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                allDeals.Add(ReadDeal(reader));
            }
            return allDeals;
        }

        private TradingDeal ReadDeal(IDataRecord record)
        {
            //the ! is for the compiler to treat the result as non-nullable 
            var deal_id = Convert.ToString(record["deal_id"])!;
            var card_id = Convert.ToString(record["card_id"])!;
            var card_type = Convert.ToString(record["card_type"])!;
            var minimum_damage = Convert.ToDouble(record["minimum_damage"])!;

            var tradingDeal = new TradingDeal();
            tradingDeal.Id = deal_id;
            tradingDeal.CardToTrade = card_id;
            tradingDeal.Type = card_type;
            tradingDeal.MinimumDamage = minimum_damage;


            return tradingDeal;
        }
        public void CreateDeal(TradingDeal tdeal)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreateDealCommand, connection);

            cmd.Parameters.AddWithValue("deal_id", tdeal.Id);
            cmd.Parameters.AddWithValue("card_id", tdeal.CardToTrade);
            cmd.Parameters.AddWithValue("card_type", tdeal.Type);
            cmd.Parameters.AddWithValue("minimum_damage", tdeal.MinimumDamage);

            var affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows == 0)
            {
                //Exception to be thrown when intended to create deal that is already in the DB
                throw new DuplicateNameException();
            }
        }
        public TradingDeal SelectDealById(string dealId)
        {
            TradingDeal deal = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectDealByIdCommand, connection);

            cmd.Parameters.AddWithValue("deal_id", dealId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                deal = ReadDeal(reader);
            }
            return deal;
        }
        public void DeleteDeal(string dealId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(DeleteDealByIdCommand, connection);

            cmd.Parameters.AddWithValue("deal_id", dealId);
            cmd.ExecuteNonQuery();
        }
        public void DeleteMultipleDealsByCardId(string cardId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(DeleteMultipleDealsByCardIdCommand, connection);

            cmd.Parameters.AddWithValue("card_id", cardId);
            cmd.ExecuteNonQuery();
        }

        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateDealsTableCommand, connection);
            cmd.ExecuteNonQuery();

        }
    }
}
