using System.Collections.Generic;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Response;
using MTCG.Models;
using MTCG.DAL.Interfaces;

namespace MTCG.API.Routing.Battles
{
    /* Class that handles the battles route. When a request comes to this route, 
      it is redirected to the _inMemoryBattleLobbyDao which handles the match-making and databse updates  (ELO, wins, losses)
     * */
    public class EnterToLobbyForBattleCommand : IRouteCommand
    {
        private readonly IStacksDao _stacksDao;
        private readonly User _user;
        private readonly IInMemoryBattleLobbyDao _inMemoryBattleLobbyDao;

        public EnterToLobbyForBattleCommand(IStacksDao stacksDao, User user, IInMemoryBattleLobbyDao inMemoryBattleLobbyDao)
        {
            //_inMemoryBattleLobbyDao holds a reference to cardDao and UserDao
            _stacksDao = stacksDao;
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
