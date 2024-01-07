using MTCG.BLL;
using MTCG.BLL.Cards;
using MTCG.HttpServer.Schemas;
using System;
using System.Collections.Generic;

namespace MTCG.Classes
{
    public class Deck
    {
        private List<CardLogic> _cards = new List<CardLogic>();
        private Random _random;
        public int Count { get; private set; }

        // Random seed input option is for testing purposes, when set the cards are not drawn randomly
        public Deck(int? randomSeed = null)
        {
            this.Count = 0;
            if (randomSeed == null)
            {
                _random = new Random();
            }
            else { _random = new Random(randomSeed ?? 0); }
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
