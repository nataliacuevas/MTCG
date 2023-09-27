using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Classes
{     
    public enum ElementType
    {
        Fire,
        Water,
        Normal,
    }
    class Card
    {
        public string Name { get; }
        public ElementType Type { get; }
        public int Damage { get; }

        public Card(string name, ElementType type, int damage)
        {
            Name = name;
            Type = type;
            Damage = damage;
        }
        public void Print()
        {
            Console.WriteLine("Card name: {0}, Element Type:  {1}, Damage: {2}", Name, Type, Damage);
        }
    }
}
