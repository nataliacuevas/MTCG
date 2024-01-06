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
using MTCG.DAL.Interfaces;

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
