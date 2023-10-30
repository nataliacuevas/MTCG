using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MTCG.Classes;

namespace MTCG.Classes
{
    public enum Players
    {
        PlayerA,
        PlayerB,
    }
    public class Battle
    {
        private const int _maxRounds = 100;

        public Battle()
        {
        }

        public (string, Players?) Round(Card cardOne, Card cardTwo)
        {

            //using deconstruction to get the output results
            var (description1, dmg1) = cardOne.DamageModifier(cardTwo);

            var (description2, dmg2) = cardTwo.DamageModifier(cardOne);

            string log = "PlayerA: " + cardOne.Type + cardOne.Name + "(" + cardOne.Damage + ") vs ";
            log += "PlayerB: " + cardTwo.Type + cardTwo.Name + "(" + cardTwo.Damage + ") -> ";
            log += description1 + description2 + " -> " +  dmg1.ToString() + " vs " + dmg2.ToString() + " -> ";

            if (dmg1 > dmg2)
            {
                return (log + cardOne.Name + " wins. ", Players.PlayerA);
            }
            else if (dmg1 < dmg2)
            {
                return (log + cardTwo.Name + " wins. ", Players.PlayerB);
            }
            else
            {
                return (log  + " Draw. ", null);
            }
        }

        public (string, Players?) Match(Deck deckA, Deck deckB)
        {
            //TODOO RANDOMIZE CARD
            string finaLog = "";
            int roundCounter = 0;


            do
            {
                //method to get card out of list deck A
                //method to get card out of list deck B
                Card cardA = deckA.PopRandomCard();
                Card cardB = deckB.PopRandomCard();
                var(log, winner) = Round(cardA, cardB);
                finaLog += "\n" + log;
                ++roundCounter;
                if (winner == Players.PlayerA)
                { 
                    deckA.AddCard(cardA);
                    deckA.AddCard(cardB);

                    if (deckB.Count == 0)
                    {   

                        return (finaLog, Players.PlayerA);
                    }
                }
                else if (winner == Players.PlayerB)
                {
                    deckB.AddCard(cardA);
                    deckB.AddCard(cardB);

                    if (deckA.Count == 0)
                    {
                        return (finaLog, Players.PlayerB);
                    }
                }
                else
                {
                    //draw Case
                    deckA.AddCard(cardA);
                    deckB.AddCard(cardB);
                }
            }
            while(roundCounter < _maxRounds);
            
            return (finaLog, null);
        }

        public string FinalMatchText(Players? winner)
        {
            if (winner == null)
            {
                return "Draw, there is no winner";
            }
            else
            {
                return "The winner of the Battle is " + winner.ToString();
            }
        }
    }
}
