using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer.Schemas
{
    internal enum CardName
    {
        WaterGoblin, FireGoblin, RegularGoblin, WaterTroll, FireTroll, RegularTroll, WaterElf, FireElf, RegularElf, WaterSpell, FireSpell, RegularSpell, Knight, Dragon, Ork, Kraken, Wizzard
    }
    internal class Card
    {
        public string Id { get; private set; }
        //TODO ASSIGN NAME TO CARD WHEN CREATED
        public CardName Name { get; private set; }
        public float Damage { get; private set; }

        public static Card CreateCard(string id, string name, float damage)
        {
            if (!Enum.TryParse<CardName>(name, out CardName cardName))
            {
                // Handle invalid name here (throw exception, set default, etc.)
                throw new ArgumentException("Invalid card name");
            }

            return new Card
            {
                Id = id,
                Name = cardName,
                Damage = damage,
            };

        }
    }
}
