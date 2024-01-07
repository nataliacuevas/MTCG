using MTCG.HttpServer.Schemas;
using MTCG.Interfaces;
using MTCG.Models;
using System;

namespace MTCG.BLL
{
    abstract public class CardLogic : ICard
    {
        public string Name { get; }
        public ElementType Type { get; }
        public double Damage { get; }

        public CardLogic(string name, ElementType type, double damage)
        {
            Name = name;
            Type = type;
            Damage = damage;
        }

        public CardLogic(Card cardSchema)
        {
            Name = cardSchema.Name;
            Type = NameToType(Name);
            Damage = cardSchema.Damage ?? 0;
        }

        // get damage value modified when this.card interacts with another one
        public abstract (string, double) DamageModifier(CardLogic otherCard);

        public ElementType NameToType(string cardName)
        {
            switch (cardName)
            {
                case "WaterGoblin":
                    return ElementType.Water;
                case "FireGoblin":
                    return ElementType.Fire;
                case "RegularGoblin":
                    return ElementType.Normal;
                case "WaterTroll":
                    return ElementType.Water;
                case "FireTroll":
                    return ElementType.Fire;
                case "RegularTroll":
                    return ElementType.Normal;
                case "WaterElf":
                    return ElementType.Water;
                case "FireElf":
                    return ElementType.Fire;
                case "RegularElf":
                    return ElementType.Normal;
                case "WaterSpell":
                    return ElementType.Water;
                case "FireSpell":
                    return ElementType.Fire;
                case "RegularSpell":
                    return ElementType.Normal;
                case "Knight":
                    return ElementType.Normal;
                case "Dragon":
                    return ElementType.Normal;
                case "Ork":
                    return ElementType.Normal;
                case "Kraken":
                    return ElementType.Normal;
                case "Wizzard":
                    return ElementType.Normal;
                case "Potion":
                    return ElementType.Normal;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
