using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MTCG.Interfaces;
using MTCG.Models;

namespace MTCG.DAL
{
    public class DatabaseCardDao
    {
        public DatabaseCardDao()
        {
            EnsureTables();
        }

        public void InsertCard(ICard card)
        {
            // This is not ideal, connection should stay open to allow a faster batch save mode
            // but for now it is ok
            string connectionString = ConnectionString.Get();
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = @"INSERT INTO card (type, name, damage, elementtype, monstertype )
                                            VALUES (@type, @name, @damage, @elementtype, @monstertype)";
                    if (card is IMonster)
                    {
                        IMonster mCard = (IMonster)card;
                        command.AddParameterWithValue("type", DbType.String, "monster");
                        command.AddParameterWithValue("monstertype", DbType.String, mCard.Mtype.ToString());

                    }
                    else if (card is ISpell)
                    {
                        command.AddParameterWithValue("type", DbType.String, "spell");
                        command.AddParameterWithValue("monstertype", DbType.String, null);
                    }
                    else
                    {

                        throw new ArgumentException("Unexpected Card Type");
                    }

                    command.AddParameterWithValue("name", DbType.String, card.Name);
                    command.AddParameterWithValue("damage", DbType.Int32, card.Damage);
                    command.AddParameterWithValue("elementtype", DbType.String, card.Type.ToString());

                    command.ExecuteNonQuery();
                }
            }
        }

        public ICard GetCardById(int id)
        {

            string connectionString = ConnectionString.Get();

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = "SELECT * FROM card WHERE id=@id";
                    command.AddParameterWithValue("id", DbType.Int32, id);
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.GetString(1) == "monster")
                            {
                                return new Monster(reader.GetString(2),
                                    (ElementType)Enum.Parse(typeof(ElementType), reader.GetString(4)),
                                    reader.GetInt32(3),
                                    (MonsterType)Enum.Parse(typeof(MonsterType), reader.GetString(5)));
                            }
                            else if (reader.GetString(1) == "spell")
                            {
                                return new Spell(reader.GetString(2),
                                    (ElementType)Enum.Parse(typeof(ElementType), reader.GetString(4)),
                                    reader.GetInt32(3));
                            }
                            else
                            {
                                throw new ArgumentException("Unexpected Card Type while in database");
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

            }
        }
        private static void EnsureTables()
        {
            string connectionString = ConnectionString.Get();
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Card (
                            id serial PRIMARY KEY,
                            type varchar(20) NOT NULL, 
                            name varchar(100) NOT NULL, 
                            damage integer NOT NULL, 
                            elementtype varchar(15) NOT NULL,  
                            monstertype varchar(15)
                        )";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
