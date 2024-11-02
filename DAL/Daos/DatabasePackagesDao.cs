using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace MTCG.DAL
{

    public class DatabasePackagesDao : IPackagesDao
    {
        private const string CreatePackagesTableCommand = @"CREATE TABLE IF NOT EXISTS packages (package_id serial PRIMARY KEY, card1_id varchar REFERENCES cards(id), card2_id varchar REFERENCES cards(id), card3_id varchar REFERENCES cards(id), card4_id varchar REFERENCES cards(id), card5_id varchar REFERENCES cards(id));";
        private const string InsertPackageCommand = @"INSERT INTO packages(card1_id, card2_id, card3_id, card4_id, card5_id) VALUES (@card1_id, @card2_id, @card3_id, @card4_id, @card5_id)";
        private const string SelectAllPackagesCommand = @"SELECT package_id FROM packages";
        private const string SelectPackageByIdCommand = "SELECT card1_id, card2_id, card3_id, card4_id, card5_id FROM packages WHERE package_id=@package_id";
        private const string DeletePackageByIdCommand = "DELETE FROM packages WHERE package_id = @package_id";
        private readonly string _connectionString;

        public DatabasePackagesDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        //it is assumed that the list of cards has length 5 and are valid cards
        public void CreatePackage(List<Card> cards)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(InsertPackageCommand, connection);

            //This for Loop prepares the sql statement 
            for (int i = 0; i < cards.Count; i++)
            {
                cmd.Parameters.AddWithValue($"card{i + 1}_id", cards[i].Id);
            }
            var affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows == 0)
            {
                //Exception to be thrown when intended to create package that is already in the DB
                throw new DuplicateNameException();
            }
        }
        private List<int> GetAllPackagesIds()
        {
            var packages_id = new List<int>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectAllPackagesCommand, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //the ! is for the compiler to treat the result as non-nullable 
                var id = Convert.ToInt32(reader["package_id"])!;
                packages_id.Add(id);
            }
            return packages_id;
        }
        public Package SelectPackageById(int id)
        {
            var package = new Package();
            package.Id = id;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectPackageByIdCommand, connection);

            cmd.Parameters.AddWithValue("package_id", id);

            using (IDataReader reader = cmd.ExecuteReader())

                if (reader.Read())
                {
                    package.Card1Id = reader.GetString(0);
                    package.Card2Id = reader.GetString(1);
                    package.Card3Id = reader.GetString(2);
                    package.Card4Id = reader.GetString(3);
                    package.Card5Id = reader.GetString(4);

                    return package;
                }
                else
                {
                    return null;
                }
        }
        public void DeletePackageById(int id)
        {
            var package = new Package();
            package.Id = id;
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(DeletePackageByIdCommand, connection);

            cmd.Parameters.AddWithValue("package_id", id);

            cmd.ExecuteNonQuery();
        }
        public Package PopRandomPackage()
        {
            List<int> packages_id = GetAllPackagesIds();
            if (packages_id.Count == 0)
            {
                return null;
            }
            Random random = new Random();
            int randomIndex = random.Next(packages_id.Count);
            int randomPackageId = packages_id[randomIndex];

            Package package = SelectPackageById(randomPackageId);
            DeletePackageById(randomPackageId);
            return package;
        }

        private void EnsureTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(CreatePackagesTableCommand, connection);
            cmd.ExecuteNonQuery();

        }

    }
}
