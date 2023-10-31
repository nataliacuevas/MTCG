using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Interfaces;

namespace MTCG.Models
{


    abstract public class Card : ICard
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
