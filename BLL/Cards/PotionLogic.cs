using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Interfaces;
using MTCG.HttpServer.Schemas;

using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using MTCG.Models;

namespace MTCG.BLL.Cards
{
    public class PotionLogic : CardLogic
    {
        public PotionLogic(string name, ElementType type, double damage) : base(name, type, damage) { }
        public PotionLogic(Card cardSchema) : base(cardSchema)
        {

        }

        public override (string, double) DamageModifier(CardLogic otherCard)
        {
            return ("Potions cannot do damage. ", 0);
        }

        public CardLogic ApplyPotion(CardLogic otherCard)
        {
            if (otherCard is MonsterLogic)
            {
                var tempCard = (MonsterLogic)otherCard;
                // The potion amplifies the damage of this monster
                double newDamage = otherCard.Damage * this.Damage;
                var poweredUpMonster = new MonsterLogic(otherCard.Name, otherCard.Type, newDamage, tempCard.Mtype);
                // Do something
                return poweredUpMonster;
            }
            else
            {
                return otherCard;
            }
        }
        public override void Print()
        {
            Console.WriteLine("Potion Card Name: {0}, Amplifyier: {2}", Name, Damage);
        }
    }

}
