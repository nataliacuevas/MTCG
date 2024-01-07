using System;

namespace MTCG.HttpServer.Schemas
{
    // Enum holding the accepted card names, as specified by the OpenAPI specification (with added Potion card)
    public enum CardName
    {
        WaterGoblin, FireGoblin, RegularGoblin, WaterTroll, FireTroll, RegularTroll, WaterElf, FireElf, RegularElf, WaterSpell, FireSpell, RegularSpell, Knight, Dragon, Ork, Kraken, Wizzard, Potion
    }
    public class Card
    {
        public string Id { get; set; }
        private CardName? _cName;
        // implemented custom setter/getter to convert from string to Enum using JSON.NET
        // these custom setters/getters require a private field (here, _cName)
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
        // To be used when converting to BLL/CardLogic
        public string GetCardType()
        {
            if (Name.Contains("Spell"))
            {
                return "spell";
            }
            else if (Name.Contains("Potion"))
            {
                return "potion";
            }
            else
            {
                return "monster";
            }
        }
        public bool IsValid()
        {
            if (_cName == null || Id == null || Damage == null)
            {
                return false;
            }
            return true;
        }
        public string PlainFormat()
        {
            return $"{Name} (DMG: {Damage}, ID: {Id})";
        }


    }
}
