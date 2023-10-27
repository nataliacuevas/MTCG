using System;
//using MTCG.Classes; //here we connect with the classes folder


namespace MTCG.Classes
{
    enum Players
    {
        Player1, 
        Player2,
    }
    struct RoundLog
    {
        string Log;
        Players winner;
    }
    public class Battle
    {

        public Battle()
        {
        }

        public RoundLog Round(Card cardOne, Card cardTwo)
        {
            if(cardOne is Monster && cardTwo is Monster)
            {
                Console.WriteLine("miau");
                return null;
            }
        }
    }

}

