using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Classes
{
    class Stack
    {
        private List<Card> _cards = new List<Card>();

        public Stack() { }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }
        public void RemoveCard(Card card)
        {
            _cards.Remove(card);
        }
        public void Print()
        {
            foreach (Card card in _cards)
            {
                card.Print();    
            }
        }
    }
}
