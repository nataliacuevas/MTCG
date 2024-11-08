﻿using Json.Net;
using MTCG.API.Routing.Battles;
using MTCG.API.Routing.Cards;
using MTCG.API.Routing.Cors;
using MTCG.API.Routing.Deck;
using MTCG.API.Routing.MarketDeals;
using MTCG.API.Routing.Packages;
using MTCG.API.Routing.Stats;
using MTCG.API.Routing.TradingDeals;
using MTCG.API.Routing.Users;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer;
using MTCG.HttpServer.Request;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;
using System.IO;

namespace MTCG.API.Routing
{
    public class RequestRouter : IRouter
    {
        private readonly IUserDao _databaseUserDao;
        private readonly ICardDao _databaseCardDao;
        private readonly IPackagesDao _databasePackagesDao;
        private readonly IStacksDao _databaseStacksDao;
        private readonly ITradingsDao _databaseTradingDealsDao;
        private readonly IMarketDao _databaseMarketDealsDao;
        private readonly IInMemoryBattleLobbyDao _inMemoryBattleLobbyDao;
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public RequestRouter(IUserDao userDao, ICardDao cardDao, IPackagesDao packagesDao, IStacksDao databaseStacksDao, ITradingsDao databaseTradingDealsDao, IMarketDao databaseMarketDealsDao, IInMemoryBattleLobbyDao inMemoryBattleLobbyDao)
        {
            _databaseUserDao = userDao;
            _databaseCardDao = cardDao;
            _databasePackagesDao = packagesDao;
            _identityProvider = new IdentityProvider(userDao);
            _routeParser = new IdRouteParser();
            _databaseStacksDao = databaseStacksDao;
            _databaseTradingDealsDao = databaseTradingDealsDao;
            _databaseMarketDealsDao = databaseMarketDealsDao;
            _inMemoryBattleLobbyDao = inMemoryBattleLobbyDao;
        }

        public IRouteCommand Resolve(HttpRequest request)
        {
            // These statement Lambdas are defined for the switch statement below
            // They take care of parsing the correct routes when the route is parametrized
            var usersRoute = (string path) => _routeParser.IsMatch(path, "/users/{username}");
            var matchUsername = (string path) => _routeParser.ParseParameters(path, "/users/{username}")["username"];
            var deckRoute = (string path) => _routeParser.IsMatch(path, "/deck");
            var tradingRoute = (string path) => _routeParser.IsMatch(path, "/tradings/.*");
            var matchTradingId = (string path) => _routeParser.ParseParameters(path, "/tradings/{tradingId}")["tradingId"];
            var marketRoute = (string path) => _routeParser.IsMatch(path, "/market/.*");
            var matchMarketDealId = (string path) => _routeParser.ParseParameters(path, "/market/{marketDealId}")["marketDealId"];
            // this lambda captures the "format" query parameter 
            var matchFormat = (string path) => _routeParser.ParseParameters(path, "/deck")["format"];

            try
            {
                // the request is routed to the corresponding IRouteCommand. If there is no corresponding route it returns null
                return request switch
                {
                    { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_databaseUserDao, JsonNet.Deserialize<UserCredentials>(request.Payload)),
                    { Method: HttpMethod.Get, ResourcePath: "/cards" } => new ListUserCardsCommand(_databaseCardDao, _databaseStacksDao, GetIdentity(request)),
                    { Method: HttpMethod.Get, ResourcePath: var path } when deckRoute(path) => new ListUserDeckCommand(_databaseCardDao, _databaseStacksDao, GetIdentity(request), matchFormat(path)),
                    { Method: HttpMethod.Put, ResourcePath: var path } when deckRoute(path) => new ConfigureDeckCommand(_databaseStacksDao, GetIdentity(request), JsonNet.Deserialize<List<string>>(request.Payload)),
                    { Method: HttpMethod.Get, ResourcePath: var path } when usersRoute(path) => new RetrieveUserDataCommand(_databaseUserDao, GetIdentity(request), matchUsername(path)),
                    { Method: HttpMethod.Put, ResourcePath: var path } when usersRoute(path) => new UpdateUserDataCommand(_databaseUserDao, GetIdentity(request), matchUsername(path), JsonNet.Deserialize<UserData>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_databaseUserDao, JsonNet.Deserialize<UserCredentials>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/packages" } => new CreatePackagesCommand(_databaseCardDao, _databasePackagesDao, GetIdentity(request), JsonNet.Deserialize<List<Card>>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/transactions/packages" } => new AcquireCardPackageCommand(_databasePackagesDao, _databaseStacksDao, _databaseUserDao, GetIdentity(request)),
                    { Method: HttpMethod.Get, ResourcePath: "/stats" } => new RetrieveUserStatsCommand(GetIdentity(request)),
                    { Method: HttpMethod.Get, ResourcePath: "/scoreboard" } => new RetrieveScoreboardCommand(_databaseUserDao, GetIdentity(request)),
                    { Method: HttpMethod.Post, ResourcePath: "/battles" } => new EnterToLobbyForBattleCommand(_databaseStacksDao, GetIdentity(request), _inMemoryBattleLobbyDao),
                    { Method: HttpMethod.Get, ResourcePath: "/tradings" } => new SelectAllDealsCommand(_databaseTradingDealsDao, GetIdentity(request)),
                    { Method: HttpMethod.Post, ResourcePath: "/tradings" } => new CreateDealCommand(_databaseStacksDao, _databaseTradingDealsDao, GetIdentity(request), JsonNet.Deserialize<TradingDeal>(request.Payload)),
                    { Method: HttpMethod.Delete, ResourcePath: var path } when tradingRoute(path) => new DeleteDealCommand(_databaseStacksDao, _databaseTradingDealsDao, GetIdentity(request), matchTradingId(path)),
                    { Method: HttpMethod.Post, ResourcePath: var path } when tradingRoute(path) => new SealDealCommand(_databaseCardDao, _databaseStacksDao, _databaseTradingDealsDao, GetIdentity(request), matchTradingId(path), JsonNet.Deserialize<string>(request.Payload)),
                    { Method: HttpMethod.Get, ResourcePath: "/market" } => new SelectAllMarketDealsCommand(_databaseMarketDealsDao, GetIdentity(request)),
                    { Method: HttpMethod.Post, ResourcePath: "/market" } => new CreateMarketDealCommand(_databaseStacksDao, _databaseMarketDealsDao, GetIdentity(request), JsonNet.Deserialize<MarketDeal>(request.Payload)),
                    { Method: HttpMethod.Delete, ResourcePath: var path } when marketRoute(path) => new DeleteMarketDealCommand(_databaseStacksDao, _databaseMarketDealsDao, GetIdentity(request), matchMarketDealId(path)),
                    { Method: HttpMethod.Post, ResourcePath: var path } when marketRoute(path) => new SealMarketDealCommand(_databaseStacksDao, _databaseTradingDealsDao, _databaseMarketDealsDao, _databaseUserDao, GetIdentity(request), matchMarketDealId(path)),
                    { Method: HttpMethod.Options } => new AllowCorsRequestCommand(),

                    _ => null
                };
            }
            catch (InvalidDataException)
            {
                return null;
            }
        }
        // function to authenticate users by their bearer token 
        // if request does not give any user, returns null
        private User GetIdentity(HttpRequest request)
        {
            return _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException(); ;
        }
    }


}
