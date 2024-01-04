using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.API.Routing.Users;
using MTCG.API.Routing.Cards;
using MTCG.API.Routing.Deck;
using MTCG.DAL;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Response;
using MTCG.Models;

namespace MTCG.API.Routing.Battles
{
    internal class EnterToLobbyForBattleCommand : IRouteCommand
    {
        private DatabaseCardDao _cardDao;
        private DatabaseStacksDao _stacksDao;
        private DatabaseUserDao _userDao;
        private User _user;
        private InMemoryBattleLobbyDao _inMemoryBattleLobbyDao;

        public EnterToLobbyForBattleCommand(DatabaseCardDao cardDao, DatabaseStacksDao stacksDao, DatabaseUserDao userDao, User user, InMemoryBattleLobbyDao inMemoryBattleLobbyDao)
        {
            _cardDao = cardDao;
            _stacksDao = stacksDao;
            _userDao = userDao;
            _user = user;
            _inMemoryBattleLobbyDao = inMemoryBattleLobbyDao;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;
            _inMemoryBattleLobbyDao.AddToLobby(_user.Username);
            response = new HttpResponse(StatusCode.Accepted);

            return response;
        }
    }
}
