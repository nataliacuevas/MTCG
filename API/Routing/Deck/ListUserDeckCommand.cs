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

namespace MTCG.API.Routing.Deck
{
    internal class ListUserDeckCommand : IRouteCommand
    {

        private readonly User _user;
        private readonly DatabaseCardDao _cardDao;
        private DatabaseStacksDao _stacksDao;
        public ListUserDeckCommand(DatabaseCardDao cardDao, DatabaseStacksDao stacksDao, User user)
        {
            _cardDao = cardDao;
            _stacksDao = stacksDao;
            _user = user;
        }
        public HttpResponse Execute()
        {
            HttpResponse response;

            List<string> requestedCardsIds = _stacksDao.SelectCardsInDeckByUsername(_user.Username);
            if (requestedCardsIds == null)
            {
                response = new HttpResponse(StatusCode.NoContent);
                return response;
            }

            List<Card> UserCards = _cardDao.GetCardsByIdList(requestedCardsIds);

            var payload = JsonNet.Serialize(UserCards);
            response = new HttpResponse(StatusCode.Ok, payload);
            return response;
        }
    }
}
