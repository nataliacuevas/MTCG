﻿using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;

namespace MTCG.API.Routing.MarketDeals
{
    public class DeleteMarketDealCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly IStacksDao _stacksDao;
        private readonly IMarketDao _marketDealsDao;
        private readonly string _dealId;

        public DeleteMarketDealCommand(IStacksDao stacksDao, IMarketDao databaseMarketDealsDao, User user, string dealId)
        {
            _user = user;
            _stacksDao = stacksDao;
            _dealId = dealId;
            _marketDealsDao = databaseMarketDealsDao;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;
            string payload;
            List<string> userCards = _stacksDao.SelectCardsByUsername(_user.Username);
            MarketDeal existing_deal = _marketDealsDao.SelectMarketDealById(_dealId);
            if (existing_deal == null)
            {
                string payload1 = "The provided market deal ID was not found";
                response = new HttpResponse(StatusCode.NotFound, payload1);
                return response;
            }
            if (!userCards.Contains(existing_deal.CardToSell))
            {
                payload = "The deal contains a card that is not owned by the user";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }

            //GOOD REQUEST
            //The deal is deleted and removed from the DB
            _marketDealsDao.DeleteMarketDeal(_dealId);
            payload = $"Market deal successfully deleted";
            response = new HttpResponse(StatusCode.Created, payload);
            return response;

        }
    }
}
