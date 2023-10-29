using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Classes
{
    public struct CommentedInteger
    {
        public string Description { get; set; }
        public int Value { get; set; }
    }

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
         // get damage value modified when this.card interacts with another one
        public abstract (string, int) DamageModifier(Card otherCard);

        //TODO PuT IT in some class 
        public double ElementModifier(ElementType dis, ElementType other)
        {
            //Fire & water cases
            if (dis == ElementType.Water && other == ElementType.Fire)
            {
                return 2;
            }
            else if (dis == ElementType.Fire && other == ElementType.Water)
            {
                return 1.0 / 2.0;
            }
            // Fire & Normal cases
            else if (dis == ElementType.Fire && other == ElementType.Normal)
            {
                return 2;
            }
            else if (dis == ElementType.Normal && other == ElementType.Fire)
            {
                return 1.0 / 2.0;
            }
            //Normal & water cases
            else if (dis == ElementType.Normal && other == ElementType.Water)
            {
                return 2;
            }
            else if (dis == ElementType.Water && other == ElementType.Normal)
            {
                return 1.0 / 2.0;
            }
            else
            {
                return 1;
            }
        }
    }
}
