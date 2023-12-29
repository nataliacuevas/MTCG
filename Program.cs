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
using MTCG.HttpServer;
using MTCG.HttpServer.Schemas;
using MTCG.API.Routing;
using MTCG.API.Routing.Users;
using System.Net;
//here we connect with the classes folder


namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Host=localhost;Username=postgres;Password=changeme;Database=simpledatastore";

            var userDao = new DatabaseUserDao(connectionString);
            var cardDao = new DatabaseCardDao(connectionString);
            var packagesDao = new DatabasePackagesDao(connectionString);
            var stacksDao = new DatabaseStacksDao(connectionString);

            var router = new RequestRouter(userDao, cardDao, packagesDao, stacksDao);
            var server = new HttpServer.HttpServer(router, IPAddress.Any, 10001);
            server.Start();
        }
        // ************************************* old things ***************************************
      /* System.Environment.Exit(0);
            OLDServer servi = new OLDServer();
            System.Environment.Exit(0);

            

            DatabaseInitializer.InitializeCleanDatabase();
            DatabaseCardDao dbCard = new DatabaseCardDao();
            


            Deck deckA = new Deck();
            Deck deckB = new Deck();

            UserBattle playerA = new UserBattle("P1", "1234", 100, deckA);
            UserBattle playerB = new UserBattle("P2", "12345", 100, deckB);

            //add MonsterLogic card
            for (int i = 6; i <= 9; i++)
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
      */
    }
}
