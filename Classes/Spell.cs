using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Classes
{
    class Spell : Card
    {
        public Spell(string name, ElementType type, int damage) : base(name, type, damage) { }

        public override void Print()
        {
            Console.WriteLine("Spell Card Name: {0}, Element Type:  {1}, Damage: {2}", Name, Type, Damage);
        }

        public override (string, int) DamageModifier(Card other)
        {
            if (other is Monster)
            {
                Monster otherMonster = (Monster)other;
                //Special cases
                if (otherMonster.Mtype == MonsterType.Kraken)
                {
                    return ("The Kraken is immune against spells. ", 0);
                }
                else // Normal case: ElementModifier handles all the elemental cases
                {
                    return ("", (int)(this.Damage * ElementModifier(this.Type, other.Type)));
                }
            }
            else if (other is Spell)
            {
                // Normal case: ElementModifier handles all the elemental cases
                return ("", (int)(this.Damage * ElementModifier(this.Type, other.Type)));
            }
            else
            {
                throw new ArgumentException("Unexpected Card Sub-class");
            }

        }
    }
}
