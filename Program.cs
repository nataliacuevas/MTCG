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
using MTCG.API.Routing.TradingDeals;
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
            var tradingDao = new DatabaseTradingDealsDao(connectionString);
            var marketDao = new DatabaseMarketDealsDao(connectionString);
            var inMemoryBattleLobbyDao = new InMemoryBattleLobbyDao(cardDao, stacksDao, userDao);

            var router = new RequestRouter(userDao, cardDao, packagesDao, stacksDao, tradingDao, marketDao, inMemoryBattleLobbyDao);
            var server = new HttpServer.HttpServer(router, IPAddress.Any, 10001);


            server.Start();
        }
    }
}
