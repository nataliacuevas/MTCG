using MTCG.BLL;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Interfaces;

namespace MTCG.DAL
{
    public static class DatabaseInitializer
    {
        //erase existing DB and create a new one
        public static void InitializeCleanDatabase()

        {
            string connectionString = ConnectionString.Get();

            var builder = new NpgsqlConnectionStringBuilder(connectionString);

            string dbName = builder.Database;

            builder.Remove("Database");
            string cs = builder.ToString();

            using (IDbConnection connection = new NpgsqlConnection(cs))
            {
                connection.Open();

                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"DROP DATABASE IF EXISTS {dbName} WITH (force)";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"CREATE DATABASE {dbName}";
                    cmd.ExecuteNonQuery();
                }
            }
            CreateCards();
        }

        public static void CreateCards()
        {
            List<ICard> allCards = new List<ICard>();

            allCards.Add(new Monster("Armed Knight", ElementType.Water, 1, MonsterType.Knight));
            allCards.Add(new Monster("Elf", ElementType.Water, 2, MonsterType.Elf));
            allCards.Add(new Monster("Kraken", ElementType.Water, 3, MonsterType.Kraken));
            allCards.Add(new Monster("Goblin", ElementType.Fire, 4, MonsterType.Goblin));
            allCards.Add(new Monster("Wizard", ElementType.Fire, 5, MonsterType.Wizard));
            allCards.Add(new Monster("Dragon", ElementType.Normal, 6, MonsterType.Dragon));
            allCards.Add(new Monster("Orc", ElementType.Normal, 7, MonsterType.Orc));

            allCards.Add(new Spell("Fireball1", ElementType.Fire, 8));
            allCards.Add(new Spell("Lightning1", ElementType.Normal, 9));
            allCards.Add(new Spell("Splash", ElementType.Water, 10));
            allCards.Add(new Spell("Blast1", ElementType.Fire, 11));
            allCards.Add(new Spell("Freeze1", ElementType.Water, 12));

            allCards.Add(new Monster("Armed Knight", ElementType.Normal, 13, MonsterType.Knight));
            allCards.Add(new Monster("Elf2", ElementType.Water, 14, MonsterType.Elf));
            allCards.Add(new Monster("Kraken2", ElementType.Fire, 15, MonsterType.Kraken));
            allCards.Add(new Monster("Goblin2", ElementType.Normal, 16, MonsterType.Goblin));
            allCards.Add(new Monster("Wizard2", ElementType.Water, 17, MonsterType.Wizard));
            allCards.Add(new Monster("Dragon2", ElementType.Fire, 18, MonsterType.Dragon));
            allCards.Add(new Monster("Orc2", ElementType.Normal, 19, MonsterType.Orc));

            DatabaseCardDao cardDao = new DatabaseCardDao();

            for (int i = 0; i < allCards.Count; i++)
            {
                cardDao.InsertCard(allCards[i]);
            }

        }
    }
}
