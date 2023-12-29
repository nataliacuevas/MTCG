﻿using Json.Net;
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
        private string _format;
        public ListUserDeckCommand(DatabaseCardDao cardDao, DatabaseStacksDao stacksDao, User user, string format)
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


            string payload = "";
            if(_format == "json" || _format == null)
            {
                payload = JsonNet.Serialize(userCards);
            }
            else if(_format == "plain")
            {
                foreach(var card in userCards)
                {
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
