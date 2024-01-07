using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;

namespace MTCG.BLL
{
    public class SpellLogic : CardLogic
    {
        public SpellLogic(string name, ElementType type, double damage) : base(name, type, damage) { }
        public SpellLogic(Card cardSchema) : base(cardSchema) { }

        public override (string, double) DamageModifier(CardLogic other)
        {
            if (other is MonsterLogic)
            {
                MonsterLogic otherMonster = (MonsterLogic)other;
                //Special cases
                if (otherMonster.Mtype == MonsterType.Kraken)
                {
                    return ("The Kraken is immune against spells. ", 0);
                }
                else // Normal case: ElementModifier handles all the elemental cases
                {
                    return ("", (int)(this.Damage * ElementLogic.ElementModifier(this.Type, other.Type)));
                }
            }
            else if (other is SpellLogic)
            {
                // Normal case: ElementModifier handles all the elemental cases
                return ("", (int)(this.Damage * ElementLogic.ElementModifier(this.Type, other.Type)));
            }
            else
            {
                throw new ArgumentException("Unexpected Card Sub-class");
            }

        }
    }
}
