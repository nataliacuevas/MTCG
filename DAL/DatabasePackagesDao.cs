using System;
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
        private const string InsertPackageCommand = @"INSERT INTO packages(card1_id, card2_id, card3_id, card4_id, card5_id) VALUES (@card1_id, @card2_id, @card3_id, @card4_id, @card5_id)";
        /* private const string SelectAllUsersCommand = @"SELECT username, password, name, bio, image FROM users";
         private const string SelectUserByUsernameCommand = "SELECT username, password, name, bio, image FROM users WHERE username=@username";
         private const string UpdateUserDataCommand = @"UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
        */
        private readonly string _connectionString;
        public DatabasePackagesDao(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }
        //it is assumed that the list of cardsa has length 5 and are valid cards
        public void CreatePackage(List<Card> cards)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var cmd = new NpgsqlCommand(InsertPackageCommand, connection);

            //TODO CHANGE SOURCE FROM VALUES TO PUT ON TABLE
            for(int i = 0; i < cards.Count; i++)
            {
                cmd.Parameters.AddWithValue($"card{i+1}_id", cards[i].Id);
            }
            var affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows == 0)
            {
                //Exception to be thrown when intended to create package that is already in the DB
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
