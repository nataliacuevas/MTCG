﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.HttpServer.Schemas;
using MTCG.API.Routing.MarketDeals;
using MTCG.API.Routing.TradingDeals;
using System.Data;

namespace MTCG.DAL
{
    public class DatabaseMarketDealsDao
    {

        private const string CreateMarketDealsTableCommand = @"CREATE TABLE IF NOT EXISTS marketDeals (marketdeal_id varchar PRIMARY KEY, card_id varchar REFERENCES cards(id), price float);";
        private const string SelectAllMarketDealsCommand = @"SELECT marketdeal_id, card_id, price FROM marketDeals";
        private const string SelectMarketDealByIdCommand = "SELECT marketdeal_id, card_id, price FROM marketDeals WHERE marketdeal_id=@marketdeal_id";
        private const string CreateMarketDealCommand = @"INSERT INTO marketDeals (marketdeal_id, card_id, price) VALUES (@marketdeal_id, @card_id, @price)";
        private const string DeleteMarketDealByIdCommand = @"DELETE FROM marketDeals WHERE marketdeal_id = @marketdeal_id;";
        /*   private const string DeleteMultipleDealsByCardIdCommand = @"DELETE FROM tradingDeals WHERE card_id = @card_id;";
           private const string ConfigureDeckCommand = @"UPDATE stacks SET in_deck = true WHERE card_id = @card_id";
           private const string SelectCardsInDeckByUsernameCommand = "SELECT card_id FROM stacks WHERE username=@username AND in_deck = true";

           DELETE FROM tradingDeals WHERE deal_id = 'your_deal_id';
           */

        private readonly string _connectionString;
        public DatabaseMarketDealsDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public List<MarketDeal> GetAllMarketDeals()
        {
            // TODO: handle exceptions
            var allDeals = new List<MarketDeal>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectAllMarketDealsCommand, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                allDeals.Add(ReadDeal(reader));
            }
            return allDeals;
        }
        private MarketDeal ReadDeal(IDataRecord record)
        {
            //the ! is for the compiler to treat the result as non-nullable 
            var marketdeal_id = Convert.ToString(record["marketdeal_id"])!;
            var card_id = Convert.ToString(record["card_id"])!;
            var price = Convert.ToInt32(record["price"])!;

            var marketDeal = new MarketDeal();
            marketDeal.Id = marketdeal_id;
            marketDeal.CardToSell = card_id;
            marketDeal.Price = price;


            return marketDeal;
        }

        public void CreateDeal(MarketDeal tdeal)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(CreateMarketDealCommand, connection);

            cmd.Parameters.AddWithValue("marketdeal_id", tdeal.Id);
            cmd.Parameters.AddWithValue("card_id", tdeal.CardToSell);
            cmd.Parameters.AddWithValue("price", tdeal.Price);

            var affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows == 0)
            {
                //Exception to be thrown when intended to create deal that is already in the DB
                throw new DuplicateNameException();
            }
        }
        public MarketDeal SelectMarketDealById(string dealId)
        {
            MarketDeal deal = null;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectMarketDealByIdCommand, connection);

            cmd.Parameters.AddWithValue("marketdeal_id", dealId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                deal = ReadDeal(reader);
            }
            return deal;
        }
        public void DeleteMarketDeal(string dealId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(DeleteMarketDealByIdCommand, connection);

            cmd.Parameters.AddWithValue("marketdeal_id", dealId);
            cmd.ExecuteNonQuery();
        }
        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreateMarketDealsTableCommand, connection);
            cmd.ExecuteNonQuery();

        }
    }
}
