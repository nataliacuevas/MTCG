using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MTCG.Classes;
using MTCG.Models;
using MTCG.BLL;
using MTCG.DAL;

//here we connect with the classes folder


namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseInitializer.InitializeCleanDatabase();
            DatabaseCardDao dbCard = new DatabaseCardDao();
            


            Deck deckA = new Deck();
            Deck deckB = new Deck();

            User playerA = new User("Nat", "1234", 100, deckA);
            User playerB = new User("Pancho", "12345", 100, deckB);

            //add MonsterLogic card
            for (int i = 6; i <= 10; i++)
            {
                CardLogic card = FromICardToCardLogic.Cast(dbCard.GetCardById(i));
                playerA.AddCardToDeck(card);
                CardLogic card2 = FromICardToCardLogic.Cast(dbCard.GetCardById(i+5));
                playerB.AddCardToDeck(card2);
            }
            
            playerA.Print();
            playerB.Print();

            Battle fight = new Battle();

            var (log, winner)  = fight.Match(playerA.UserDeck , playerB.UserDeck);
            log += "\n" + fight.FinalMatchText(winner);
            ELOHandler.Update(winner, playerA, playerB);

            Console.WriteLine(log);
            playerA.Print();
            playerB.Print();
        }
    }
}
