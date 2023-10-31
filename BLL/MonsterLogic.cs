using MTCG.Interfaces;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace MTCG.BLL
{
    class MonsterLogic : CardLogic, IMonster
    {
        public MonsterType Mtype { get; }
        public MonsterLogic(string name, ElementType type, int damage, MonsterType mtype) : base(name, type, damage)
        {
            Mtype = mtype;
        }

        public override void Print()
        {
            Console.WriteLine("Monster Card Name: {0}, Element Type:  {1}, Damage: {2}", Name, Type, Damage);
        }

        public override (string, int) DamageModifier(CardLogic other)
        {
            if (other is MonsterLogic)
            {
                MonsterLogic otherMonster = (MonsterLogic)other;
                //Special cases
                if (this.Mtype == MonsterType.Goblin && otherMonster.Mtype == MonsterType.Dragon)
                {
                    return ("Goblin is too afraid of Dragon to attack", 0);
                }
                else if (this.Mtype == MonsterType.Orc && otherMonster.Mtype == MonsterType.Wizard)
                {
                    return ("The Orc is controled by the Wizzard, therefore cannot do any damage", 0);
                }
                else if (this.Mtype == MonsterType.Dragon && otherMonster.Type == ElementType.Fire && otherMonster.Mtype == MonsterType.Elf)
                {
                    return ("The Fire Elf knows the Dragon since they were little and can evade their attacks. ", 0);
                }
                else
                {
                    // Normal Case
                    return ("", this.Damage);
                }
            }
            else if (other is SpellLogic)
            {
                //Special Cases 
                if (this.Mtype == MonsterType.Knight && other.Type == ElementType.Water)
                {
                    return ("The armor of the Knight is so heavy that that the Water spell made him drown instantly", 0);
                }
                else
                {
                    // Normal case: ElementModifier handles all the elemental cases
                    return ("", (int)(this.Damage * ElementLogic.ElementModifier(this.Type, other.Type)));
                }
            }
            else
            {
                throw new ArgumentException("Unexpected Card Sub-class");
            }

        }
    }
}
