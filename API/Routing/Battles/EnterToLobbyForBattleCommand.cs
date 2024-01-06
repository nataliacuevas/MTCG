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
using MTCG.DAL.Interfaces;

namespace MTCG.API.Routing.Battles
{
    public class EnterToLobbyForBattleCommand : IRouteCommand
    {
        private readonly ICardDao _cardDao;
        private readonly IStacksDao _stacksDao;
        private readonly IUserDao _userDao;
        private readonly User _user;
        private readonly IInMemoryBattleLobbyDao _inMemoryBattleLobbyDao;

        public EnterToLobbyForBattleCommand(ICardDao cardDao, IStacksDao stacksDao, IUserDao userDao, User user, IInMemoryBattleLobbyDao inMemoryBattleLobbyDao)
        {
            _cardDao = cardDao;
            _stacksDao = stacksDao;
            _userDao = userDao;
            _user = user;
            _inMemoryBattleLobbyDao = inMemoryBattleLobbyDao;
        }

        public HttpResponse Execute()
        {
            string payload;
            HttpResponse response;
            List<string> deck = _stacksDao.SelectCardsInDeckByUsername(_user.Username);
            if (deck.Count == 0)
            {
                payload = "not possible to battle without a configured deck";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }
            payload = _inMemoryBattleLobbyDao.AddToLobby(_user.Username);
            response = new HttpResponse(StatusCode.Accepted, payload);

            return response;
        }
    }
}
