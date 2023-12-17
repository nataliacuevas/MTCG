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
using Json.Net;
using MTCG.HttpServer;

namespace MTCG.API.Routing
{
    internal class RequestRouter
    {
        private readonly DatabaseUserDao _databaseUserDao;
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public RequestRouter(DatabaseUserDao databaseUserDao)
        {
            _databaseUserDao = databaseUserDao;
            _identityProvider = new IdentityProvider(databaseUserDao);
            _routeParser = new IdRouteParser();
        }

        public IRouteCommand Resolve(HttpRequest request)
        {
            var isMatch = (string path) => _routeParser.IsMatch(path, "/users/{username}");
            var parseUsername = (string path) => int.Parse(_routeParser.ParseParameters(path, "/users/{username}")["username"]);
            //TODO if required
            //var checkBody = (string payload) => payload ?? throw new InvalidDataException();

            try
            {
                return request switch
                {
                    { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_databaseUserDao, JsonNet.Deserialize<UserCredentials>(request.Payload)),
                    //   { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),

                    // { Method: HttpMethod.Post, ResourcePath: "/messages" } => new AddMessageCommand(_messageManager, GetIdentity(request), checkBody(request.Payload)),
                    // { Method: HttpMethod.Get, ResourcePath: "/messages" } => new ListMessagesCommand(_messageManager, GetIdentity(request)),

                    //{ Method: HttpMethod.Get, ResourcePath: var path } when isMatch(path) => new ShowMessageCommand(_messageManager, GetIdentity(request), parseUsername(path)),
                    //{ Method: HttpMethod.Put, ResourcePath: var path } when isMatch(path) => new UpdateMessageCommand(_messageManager, GetIdentity(request), parseUsername(path), checkBody(request.Payload)),
                    //{ Method: HttpMethod.Delete, ResourcePath: var path } when isMatch(path) => new RemoveMessageCommand(_messageManager, GetIdentity(request), parseUsername(path)),

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
