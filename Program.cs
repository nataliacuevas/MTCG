using MTCG.API.Routing;
using MTCG.DAL;
using System.Net;



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
