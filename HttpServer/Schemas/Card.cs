using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTCG.HttpServer.Schemas
{
    enum CardName
    {
        WaterGoblin, FireGoblin, RegularGoblin, WaterTroll, FireTroll, RegularTroll, WaterElf, FireElf, RegularElf, WaterSpell, FireSpell, RegularSpell, Knight, Dragon, Ork, Kraken, Wizzard
    }
    internal class Card
    {
        public string Id { get; private set; }
        private CardName? _cName;
        //implemented custom setter to convert from string to Enum using JSON.NET
        public String Name
        {
            get
            {
                if (_cName == null)
                {
                    return null;
                }
                else
                {
                    return _cName.ToString();
                }
            }

            set
            {
                CardName nameIntermediate;
                bool result = Enum.TryParse<CardName>(value, out nameIntermediate);
                if (!result)
                {
                    _cName = null;
                }
                else
                {
                    _cName = nameIntermediate;
                }
            }
        }
        public double? Damage { get; set; }

        public bool IsValid()
        {
            if (_cName == null || Id == null || Damage == null)
            {
                return false;
            }
            return true;
        }

        public void Print()
        {
            if (_cName == null)
            {
                Console.WriteLine("cName: NULL");
            }
            else
            {
                Console.WriteLine("cName: {0}", _cName);
            }
            if (Id == null)
            {
                Console.WriteLine("Id: NULL");
            }
            else
            {
                Console.WriteLine("Id: {0}", Id);
            }
            if (Damage == null)
            {
                Console.WriteLine("Damage: NULL");
            }
            else
            {
                Console.WriteLine("Damage: {0}", Damage);
            }
        }

    }
}
