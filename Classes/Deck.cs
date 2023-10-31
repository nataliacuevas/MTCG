using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;
using MTCG.BLL;

namespace MTCG.Classes
{
    public class Deck
    {
        private List<CardLogic> _cards = new List<CardLogic>();
        public int Count { get; private set; }

        public Deck()
        {
            this.Count = 0;
        }

        public void AddCard(CardLogic card)
        {
            _cards.Add(card);
            ++Count;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            _cards.RemoveAt(index);
            --Count;
        }

        public void RemoveCard(CardLogic card)
        {
            _cards.Remove(card);
            --Count;
        }

        public void Print()
        {
            foreach (CardLogic card in _cards)
            {
                card.Print();    
            }
        }

        public CardLogic PopRandomCard()
        {
            // get a random index within range of the available cards
            var random = new Random();
            int index = random.Next(0, _cards.Count);
            //get the card in the given index
            CardLogic randomCard = _cards[index];
            //remove card from deck
            RemoveAt(index);
            return randomCard;
        }   
    }
}
