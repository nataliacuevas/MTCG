using Json.Net;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;

namespace MTCG.API.Routing.Deck
{
    public class ListUserDeckCommand : IRouteCommand
    {

        private readonly User _user;
        private readonly ICardDao _cardDao;
        private readonly IStacksDao _stacksDao;
        private readonly string _format;
        public ListUserDeckCommand(ICardDao cardDao, IStacksDao stacksDao, User user, string format)
        {
            _cardDao = cardDao;
            _stacksDao = stacksDao;
            _user = user;
            _format = format;
        }
        public HttpResponse Execute()
        {
            HttpResponse response;

            List<string> requestedCardsIds = _stacksDao.SelectCardsInDeckByUsername(_user.Username);
            if (requestedCardsIds.Count == 0)
            {
                response = new HttpResponse(StatusCode.NoContent);
                return response;
            }

            List<Card> userCards = _cardDao.GetCardsByIdList(requestedCardsIds);

            //if no format is given, _format is = null, and since JSON is the default format, that format will be returned
            string payload = "";
            if (_format == "json" || _format == null)
            {
                payload = JsonNet.Serialize(userCards);
            }
            else if (_format == "plain")
            {
                foreach (var card in userCards)
                {
                    //the plain format is defined by .PlainFormat function which is "glued" together with \n 
                    payload += card.PlainFormat() + "\n";
                }
            }
            else
            {
                response = new HttpResponse(StatusCode.BadRequest);
                return response;
            }

            response = new HttpResponse(StatusCode.Ok, payload);
            return response;
        }
    }
}
