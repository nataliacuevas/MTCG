using Json.Net;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;

namespace MTCG.API.Routing.Stats
{
    public class RetrieveUserStatsCommand : IRouteCommand
    {
        private readonly User _user;
        public RetrieveUserStatsCommand(User user)
        {
            _user = user;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            var userStats = new UserStats
            {
                Name = _user.Name,
                Elo = _user.Elo,
                Wins = _user.Wins,
                Losses = _user.Losses
            };
            var payload = JsonNet.Serialize(userStats);
            response = new HttpResponse(StatusCode.Ok, payload);
            return response;
        }
    }

}
