using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Classes
{
    public class Deck
    {
        private List<Card> _cards = new List<Card>();
        public int Count { get; private set; }

        public Deck()
        {
            this.Count = 0;
        }

        public void AddCard(Card card)
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

        public void RemoveCard(Card card)
        {
            _cards.Remove(card);
            --Count;
        }

        public void Print()
        {
            foreach (Card card in _cards)
            {
                card.Print();    
            }
        }

        public Card PopRandomCard()
        {
            // get a random index within range of the available cards
            var random = new Random();
            int index = random.Next(0, _cards.Count);
            System.Console.WriteLine("random index: {0} ", index);
            //get the card in the given index
            Card randomCard = _cards[index];
            //remove card from deck
            RemoveAt(index);
            return randomCard;
        }   
    }
}
