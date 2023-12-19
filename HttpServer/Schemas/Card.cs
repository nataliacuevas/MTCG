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
        string Id;
        public CardName Name;
        float Damage;
    }
}
