using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MTCG.Interfaces;
using MTCG.Models;
using static System.Net.Mime.MediaTypeNames;

namespace MTCG.BLL
{
    internal class FromICardToCardLogic
    {
        public static CardLogic Cast(ICard card)
        {
            if (card is IMonster)
            {
                IMonster mCard = (IMonster)card;
                return new MonsterLogic(card.Name, card.Type, card.Damage, mCard.Mtype);
            }
            else if (card is ISpell)
            {
                ISpell sCard = (ISpell)card;
                return new SpellLogic(card.Name, card.Type, card.Damage);
            }
            else
            {
                throw new ArgumentException("something went wrong");
            }
        }
    }
}
