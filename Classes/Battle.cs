using System;
using System.Collections.Generic;
using System.Linq;
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
    public struct RoundLog
    {
        public string Log {  get; set; } 
        public Players? Winner { get; set; } //null if draw
    }
    public class Battle
    {
        private const int _maxRounds = 100;

        public Battle()
        {
        }

        public RoundLog Round(Card cardOne, Card cardTwo)
        {

            //using deconstruction to get the output results
            var (description1, dmg1) = cardOne.DamageModifier(cardTwo);

            var (description2, dmg2) = cardTwo.DamageModifier(cardOne);

            string log = "PlayerA: " + cardOne.Type + cardOne.Name + "(" + cardOne.Damage + ") vs ";
            log += "PlayerB: " + cardTwo.Type + cardTwo.Name + "(" + cardTwo.Damage + ") -> ";
            log += description1 + description2 + " -> " +  dmg1.ToString() + " vs " + dmg2.ToString() + " -> ";

            if (dmg1 > dmg2)
            {
                return new RoundLog() { Log = log + cardOne.Name + " wins. ", Winner = Players.PlayerA };
            }
            else if (dmg1 < dmg2)
            {
                return new RoundLog() { Log = log + cardTwo.Name + " wins. ", Winner = Players.PlayerB };
            }
            else
            {
                return new RoundLog() { Log = log  + " Draw. ", Winner = null };
            }
        }

        // public (string, Players) Match(); 

    }
}
