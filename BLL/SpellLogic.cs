using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using MTCG.Interfaces;

namespace MTCG.BLL
{
    class SpellLogic : CardLogic, ISpell
    {
        public SpellLogic(string name, ElementType type, int damage) : base(name, type, damage) { }
        public override void Print()
        {
            Console.WriteLine("Spell Card Name: {0}, Element Type:  {1}, Damage: {2}", Name, Type, Damage);
        }

        public override (string, int) DamageModifier(CardLogic other)
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
