using MTCG.DAL;
using MTCG.HttpServer.Request;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MTCG.API.Routing;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.API.Routing.Users;
using MTCG.API.Routing.Cors;
using MTCG.API.Routing.Packages;
using MTCG.API.Routing.Cards;
using MTCG.API.Routing.Deck;
using MTCG.API.Routing.Stats;
using MTCG.API.Routing.
using Json.Net;
using MTCG.HttpServer;



namespace MTCG.API.Routing
{
    internal class RequestRouter
    {
        private readonly DatabaseUserDao _databaseUserDao;
        private readonly DatabaseCardDao _databaseCardDao;
        private readonly DatabasePackagesDao _databasePackagesDao;
        private readonly DatabaseStacksDao _databaseStacksDao;
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public RequestRouter(DatabaseUserDao userDao, DatabaseCardDao cardDao, DatabasePackagesDao packagesDao, DatabaseStacksDao databaseStacksDao)
        {
            _databaseUserDao = userDao;
            _databaseCardDao = cardDao;
            _databasePackagesDao = packagesDao;
            _identityProvider = new IdentityProvider(userDao);
            _routeParser = new IdRouteParser();
            _databaseStacksDao = databaseStacksDao;
        }

        public IRouteCommand Resolve(HttpRequest request)
        {
            var usersRoute = (string path) => _routeParser.IsMatch(path, "/users/{username}");
            var matchUsername = (string path) => _routeParser.ParseParameters(path, "/users/{username}")["username"];
            var deckRoute = (string path) => _routeParser.IsMatch(path, "/deck");
            var matchFormat = (string path) => _routeParser.ParseParameters(path, "/deck")["format"];
            //TODO if required
            //var checkBody = (string payload) => payload ?? throw new InvalidDataException();
            if (request.Payload != null)
            {
                Console.WriteLine(request.Payload.ToString());
            }
            try
            {
                return request switch
                {
                    { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_databaseUserDao, JsonNet.Deserialize<UserCredentials>(request.Payload)),
                    //   { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),

                    // { Method: HttpMethod.Post, ResourcePath: "/messages" } => new AddMessageCommand(_messageManager, GetIdentity(request), checkBody(request.Payload)),
                    { Method: HttpMethod.Get, ResourcePath: "/cards" } => new ListUserCardsCommand(_databaseCardDao, _databaseStacksDao, GetIdentity(request)),
                    { Method: HttpMethod.Get, ResourcePath: var path } when deckRoute(path) => new ListUserDeckCommand(_databaseCardDao, _databaseStacksDao, GetIdentity(request), matchFormat(path)),
                    { Method: HttpMethod.Put, ResourcePath: var path } when deckRoute(path) => new ConfigureDeckCommand(_databaseStacksDao, GetIdentity(request), JsonNet.Deserialize<List<string>>(request.Payload)),
                    { Method: HttpMethod.Get, ResourcePath: var path } when usersRoute(path) => new RetrieveUserDataCommand(_databaseUserDao, GetIdentity(request), matchUsername(path)),                
                    { Method: HttpMethod.Put, ResourcePath: var path } when usersRoute(path) => new UpdateUserDataCommand(_databaseUserDao, GetIdentity(request), matchUsername(path), JsonNet.Deserialize<UserData>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_databaseUserDao, JsonNet.Deserialize<UserCredentials>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/packages" } => new CreatePackagesCommand(_databaseCardDao, _databasePackagesDao, GetIdentity(request), JsonNet.Deserialize<List<Card>>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/transactions/packages" } => new AcquireCardPackageCommand(_databaseCardDao, _databasePackagesDao, _databaseStacksDao, _databaseUserDao, GetIdentity(request)),
                    { Method: HttpMethod.Get, ResourcePath: "/stats" } => new RetrieveUserStatsCommand(GetIdentity(request)),
                    { Method: HttpMethod.Get, ResourcePath: "/scoreboard" } => new RetrieveScoreboardCommand(_databaseUserDao, GetIdentity(request)),
                    { Method: HttpMethod.Post, ResourcePath: "/battles" } => new  EnterToLobbyForBattleCommand(_databaseUserDao, _databaseCardDao, _databaseStacksDao, GetIdentity(request)),


                    //{ Method: HttpMethod.Delete, ResourcePath: var path } when usersRoute(path) => new RemoveMessageCommand(_messageManager, GetIdentity(request), matchUsername(path)),
                    { Method: HttpMethod.Options } => new AllowCorsRequestCommand(),
                    _ => null
                };
            }
            catch (InvalidDataException)
            {
                return null;
            }
        }
        //TODO ERASE IF NOT USED 
       /* private T Deserialize<T>(string? body) where T : class
        {
            var data = body is not null ? JsonConvert.DeserializeObject<T>(body) : null;
            return data ?? throw new InvalidDataException();
        }
       */
       //if request does not give any user, returns null
        private User GetIdentity(HttpRequest request)
        {
            return _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException(); ;
        }
    }


}
