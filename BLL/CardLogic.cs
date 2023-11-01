using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Interfaces;
using MTCG.Models;

namespace MTCG.BLL
{
    abstract public class CardLogic : ICard
    {
        public string Name { get; }
        public ElementType Type { get; }
        public int Damage { get; }

        public CardLogic(string name, ElementType type, int damage)
        {
            Name = name;
            Type = type;
            Damage = damage;
        }

        public abstract void Print();
        // get damage value modified when this.card interacts with another one
        public abstract (string, int) DamageModifier(CardLogic otherCard);
    }
}
