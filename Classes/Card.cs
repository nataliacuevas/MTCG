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
    abstract public class Card
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
        public abstract void Print();
    }
}
