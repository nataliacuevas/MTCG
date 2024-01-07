using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;
using MTCG.BLL;
using MTCG.HttpServer.Schemas;
using MTCG.BLL.Cards;

namespace MTCG.Classes
{
    public class Deck
    {
        private List<CardLogic> _cards = new List<CardLogic>();
        private Random _random;
        public int Count { get; private set; }

        public Deck()
        {
            this.Count = 0;
            _random = new Random();
        }
        //This constructor transforms from the Card class (which is a DB model) to a CardLogic (which is BLL class) 
        public Deck(List<Card> cards)
        {
            foreach (var card in cards)
            {
                string cardType = card.GetCardType();
                if (cardType == "spell")
                {
                    _cards.Add(new SpellLogic(card));
                }
                else if (cardType == "potion")
                {
                    _cards.Add(new PotionLogic(card));
                }
                else
                {
                    _cards.Add(new MonsterLogic(card));
                }
                this.Count = _cards.Count;
            }
            _random = new Random();
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

            int index = _random.Next(0, _cards.Count);
            //get the card in the given index
            
            CardLogic randomCard = _cards[index];
            //remove card from deck
            RemoveAt(index);
            return randomCard;
        }   
    }
}
