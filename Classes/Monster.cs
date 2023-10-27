using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Classes
{
    public enum MonsterType
    {
        Goblin,
        Dragon,
        Wizzard,
        Orc,
        Knight,
        Kraken,
        Elf,
        Troll,
    }
    class Monster : Card
    {
        public MonsterType Mtype { get; }
        public Monster(string name, ElementType type, int damage, MonsterType mtype) : base(name, type, damage)
        {
            Mtype = mtype;
        }

        public override void Print()
        {
            Console.WriteLine("Monster Card Name: {0}, Element Type:  {1}, Damage: {2}", Name, Type, Damage);
        }
    }
}
