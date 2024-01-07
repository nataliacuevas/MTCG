using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;

namespace MTCG.API.Routing.TradingDeals
{
    public class DeleteDealCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly IStacksDao _stacksDao;
        private readonly ITradingsDao _tradingDealsDao;
        private readonly string _dealId;

        public DeleteDealCommand(IStacksDao stacksDao, ITradingsDao tradingDealsDao, User user, string dealId)
        {
            _user = user;
            _stacksDao = stacksDao;
            _tradingDealsDao = tradingDealsDao;
            _dealId = dealId;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;
            string payload;
            List<string> userCards = _stacksDao.SelectCardsByUsername(_user.Username);
            TradingDeal existing_deal = _tradingDealsDao.SelectDealById(_dealId);

            if (existing_deal == null)
            {
                string payload1 = "The provided deal ID was not found";
                response = new HttpResponse(StatusCode.NotFound, payload1);
                return response;
            }

            if (!userCards.Contains(existing_deal.CardToTrade))
            {
                payload = "The deal contains a card that is not owned by the user";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }

            //GOOD REQUEST
            //The deal is deleted and removed from the DB
            _tradingDealsDao.DeleteDeal(_dealId);
            payload = $"Trading deal successfully deleted";
            response = new HttpResponse(StatusCode.Created, payload);
            return response;

        }
    }
}
