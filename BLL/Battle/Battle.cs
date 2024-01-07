using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MTCG.BLL;
using MTCG.Models;

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

        public (string, Players?) Round(CardLogic cardOne, CardLogic cardTwo, string userA, string userB)
        {

            //using deconstruction to get the output results
            var (description1, dmg1) = cardOne.DamageModifier(cardTwo);

            var (description2, dmg2) = cardTwo.DamageModifier(cardOne);

            string log = userA + ": " + cardOne.Type + cardOne.Name + "(" + cardOne.Damage + ") vs ";
            log += userB + ": " + cardTwo.Type + cardTwo.Name + "(" + cardTwo.Damage + ") \n    -> ";
            log += description1 + description2 + " -> " +  dmg1.ToString() + " vs " + dmg2.ToString() + "\n    -> ";

            if (dmg1 > dmg2)
            {
                return (log + cardOne.Name + " wins. (" + userA+ ")", Players.PlayerA);
            }
            else if (dmg1 < dmg2)
            {
                return (log + cardTwo.Name + " wins. (" + userB + ")", Players.PlayerB);
            }
            else
            {
                return (log  + " Draw. ", null);
            }
        }

        public (string, Players?) Match(Deck deckA, Deck deckB, string userA, string userB)
        {
            //TODOO RANDOMIZE CARD
            string finaLog = "";
            int roundCounter = 0;


            do
            {
                //method to get card out of list deck A
                //method to get card out of list deck B
                CardLogic cardA = deckA.PopRandomCard();
                CardLogic cardB = deckB.PopRandomCard();
                Console.WriteLine(cardA.Name + " vs " + cardB.Name + ": " + deckA.Count.ToString() + " vs " + deckB.Count.ToString());
                var(log, winner) = Round(cardA, cardB, userA, userB);
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
            // return null to indicate a draw
            return (finaLog, null);
        }

        public string FinalMatchText(Players? winner, string userA, string userB)
        {
            if (winner == null)
            {
                return "Draw, there is no winner";
            }
            else
            {
                string winnerUser;
                if(winner == Players.PlayerA)
                {
                    winnerUser = userA;
                }
                else
                {
                    winnerUser = userB;
                }
                return "The winner of the Battle is " + winnerUser;
            }
        }
    }
}
