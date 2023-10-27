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
        Player1,
        Player2,
    }
    public struct RoundLog
    {
        public string Log {  get; set; } 
        public Players Winner { get; set; }
    }
    public class Battle
    {

        public Battle()
        {
        }

        public RoundLog Round(Card cardOne, Card cardTwo)
        {
            if (cardOne is Monster && cardTwo is Monster)
            {
                Console.WriteLine("miau");
                RoundLog output = new RoundLog();
                output.Log = "";
                output.Winner = Players.Player1;

                return output; 
            }
            else
            {
                RoundLog output = new RoundLog();
                output.Log = "";
                output.Winner = Players.Player1;
                return output;
            }
        }
    }

}
