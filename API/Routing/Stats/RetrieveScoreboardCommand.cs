using Json.Net;
using MTCG.DAL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Stats
{
    internal class RetrieveScoreboardCommand : IRouteCommand
    {
        private readonly User _user;
        private DatabaseUserDao _userDao;

        public RetrieveScoreboardCommand(DatabaseUserDao userdao, User user)
        {
            _user = user;
            _userDao = userdao;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;
            //The SQL query sorts by ELO
            IEnumerable<User> allUsers = _userDao.GetAllUsers();

            List<UserStats> allUsersStats = new List<UserStats>();

            foreach (User user in allUsers)
            {

                var userStats = new UserStats
                {
                    Name = user.Name,
                    Elo = user.Elo,
                    Wins = user.Wins,
                    Losses = user.Losses
                };
                allUsersStats.Add(userStats);
            }
            var payload = JsonNet.Serialize(allUsersStats);
            response = new HttpResponse(StatusCode.Ok, payload);
            return response;
        }
    }
}
