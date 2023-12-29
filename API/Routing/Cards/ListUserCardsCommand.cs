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

namespace MTCG.API.Routing.Cards
{
    internal class ListUserCardsCommand : IRouteCommand
    {

        private readonly User _user;
        private readonly DatabaseCardDao _cardDao;
        private DatabaseStacksDao _stacksDao;
        public ListUserCardsCommand(DatabaseCardDao cardDao, DatabaseStacksDao stacksDao, User user)
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
            List<Card> requestedCards = new List<Card>();


            foreach (string cardId in requestedCardsIds) 
            {
                requestedCards.Add(_cardDao.GetCardbyId(cardId));
            }

           
            var payload = JsonNet.Serialize(requestedCards);
            response = new HttpResponse(StatusCode.Ok, payload);
            return response;
        }
    }
}
