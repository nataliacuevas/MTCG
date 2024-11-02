using MTCG.HttpServer.Schemas;
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

        // General method that can be implemented for different potion cards for different effects 
        // Ex. Change element type or monster type
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
    }

}
