using Json.Net;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;

namespace MTCG.API.Routing.Cards
{
    public class ListUserCardsCommand : IRouteCommand
    {

        private readonly User _user;
        private readonly ICardDao _cardDao;
        private readonly IStacksDao _stacksDao;
        public ListUserCardsCommand(ICardDao cardDao, IStacksDao stacksDao, User user)
        {
            _cardDao = cardDao;
            _stacksDao = stacksDao;
            _user = user;
        }
        public HttpResponse Execute()
        {
            HttpResponse response;

            List<string> requestedCardsIds = _stacksDao.SelectCardsByUsername(_user.Username);
            if (requestedCardsIds == null)
            {
                response = new HttpResponse(StatusCode.NoContent);
                return response;
            }
            List<Card> requestedCards = _cardDao.GetCardsByIdList(requestedCardsIds);

            var payload = JsonNet.Serialize(requestedCards);
            response = new HttpResponse(StatusCode.Ok, payload);
            return response;
        }
    }
}
